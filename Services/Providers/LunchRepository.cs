using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TableDependency.SqlClient;
using Vacation_Portal.Commands;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class LunchRepository : ILunchRepository
    {
        public SqlTableDependency<HolidayDTO> tableDependencyHoliday;
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;
        public ICommand LoadHolidays { get; } = new LoadHolidaysCommand();
        public ICommand LoadHolidayTypes { get; } = new LoadHolidayTypesCommand();
        public ICommand Login { get; } = new LoginCommand();

        #region PropChange
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null)
        {
            if(EqualityComparer<T>.Default.Equals(member, value))
            {
                return false;
            }
            member = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public LunchRepository(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }

        #region Props
        public Person Person { get; set; }
        public DateTime DateUnblockNextCalendar { get; set; }
        public bool IsCalendarUnblocked { get; set; }

        public bool CheckDateUnblockedCalendarAsync()
        {
            IEnumerable<CalendarSettings> calendarSettings = GetSettingsForCalendar();
            foreach(CalendarSettings settings in calendarSettings)
            {
                if(settings.Setting_Name == "NextCalendarUnlock")
                {
                    DateUnblockNextCalendar = settings.Setting_Date;
                    IsCalendarUnblocked = DateUnblockNextCalendar <= DateTime.UtcNow;
                    return IsCalendarUnblocked;
                }
            }
            return false;
        }

        public bool CheckNextCalendarPlanningUnlock()
        {
            IEnumerable<CalendarSettings> calendarSettings = GetSettingsForCalendar();
            foreach(CalendarSettings settings in calendarSettings)
            {
                if(settings.Setting_Name == "NextCalendarPlanningUnlock")
                {
                    DateUnblockPlanning = settings.Setting_Date;
                    IsCalendarPlannedOpen = DateUnblockPlanning <= DateTime.UtcNow;
                    return IsCalendarPlannedOpen;
                }
            }
            return false;
        }

        public DateTime DateUnblockPlanning { get; set; }
        public bool IsCalendarPlannedOpen { get; set; } = true;

        public Action<ObservableCollection<HolidayViewModel>> OnHolidaysChanged { get; set; }
        public Action<ObservableCollection<Holiday>> OnHolidayTypesChanged { get; set; }

        private ObservableCollection<HolidayViewModel> _holidays = new ObservableCollection<HolidayViewModel>();
        public ObservableCollection<HolidayViewModel> Holidays
        {
            get => _holidays;
            set
            {
                _holidays = value;
                OnPropertyChanged(nameof(Holidays));
            }
        }
        private ObservableCollection<Holiday> _holidayTypes = new ObservableCollection<Holiday>();
        public ObservableCollection<Holiday> HolidayTypes
        {
            get => _holidayTypes;
            set
            {
                _holidayTypes = value;
                OnPropertyChanged(nameof(HolidayTypes));
            }
        }
        private readonly ObservableCollection<Vacation> Vacations = new ObservableCollection<Vacation>();
        private readonly ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
        public Action<List<VacationViewModel>> OnVacationsChanged { get; set; }
        public Action<Person> OnLoginSuccess { get; set; }
        public List<PersonDTO> Persons { get; set; } = new List<PersonDTO>();
        public List<Person> FullPersons { get; set; } = new List<Person>();

        #endregion

        #region Person

        public async IAsyncEnumerable<Vacation> FetchVacationsAsync(int sapId)
        {
            IEnumerable<Vacation> vacations = await LoadVacationsAsync(sapId);

            foreach(Vacation item in vacations)
            {
                yield return item;
            }
        }
        public async IAsyncEnumerable<VacationAllowanceViewModel> FetchVacationAllowancesAsync(int sapId)
        {
            IEnumerable<VacationAllowanceViewModel> vacationAllowances = await GetVacationAllowanceAsync(sapId);
            foreach(VacationAllowanceViewModel item in vacationAllowances)
            {
                yield return item;
            }
        }
        public async Task<Person> LoginAsync(string account)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                object parametersPerson = new
                {
                    Account = account
                };
                bool isHrAdmin = false;
                BrushConverter converter = new System.Windows.Media.BrushConverter();
                IEnumerable<PersonDTO> fullPersonDTOs = await database.QueryAsync<PersonDTO>("usp_Get_Users", parametersPerson, commandType: CommandType.StoredProcedure);

                foreach(PersonDTO personDTO in fullPersonDTOs)
                {
                    if(personDTO.User_Id_Account == account && personDTO.Role_Name == "HR Admin")
                    {
                        isHrAdmin = true;
                    }
                }

                App.SplashScreen.status.Text = "В поисках ваших отпусков";
                App.SplashScreen.status.Foreground = Brushes.Black;

                foreach(PersonDTO personDTO in fullPersonDTOs)
                {
                    await foreach(Vacation vacation in FetchVacationsAsync(personDTO.User_Id_SAP))
                    {
                        Brush brushColor;
                        if(vacation.Color == null)
                        {
                            brushColor = Brushes.Gray;
                        } else
                        {
                            brushColor = vacation.Color;
                        }

                        if(!Vacations.Contains(vacation))
                        {
                            Vacations.Add(vacation);
                        }
                    }

                    await foreach(VacationAllowanceViewModel vacationAllowance in FetchVacationAllowancesAsync(personDTO.User_Id_SAP))
                    {
                        VacationAllowanceViewModel vacationAllowanceViewModel = new VacationAllowanceViewModel(vacationAllowance.User_Id_SAP, vacationAllowance.Vacation_Name, vacationAllowance.Vacation_Id, vacationAllowance.Vacation_Year, vacationAllowance.Vacation_Days_Quantity, vacationAllowance.Vacation_Color);
                        if(!VacationAllowances.Contains(vacationAllowanceViewModel))
                        {
                            VacationAllowances.Add(vacationAllowanceViewModel);
                        }
                    }

                }
                //собираю общий список с персонами и их отпусками
                foreach(PersonDTO item in fullPersonDTOs)
                {
                    ObservableCollection<Vacation> VacationsForPerson = new ObservableCollection<Vacation>();
                    ObservableCollection<VacationAllowanceViewModel> VacationAllowancesForPerson = new ObservableCollection<VacationAllowanceViewModel>();
                    foreach(Vacation vacationForPerson in Vacations)
                    {
                        if(vacationForPerson.User_Id_SAP == item.User_Id_SAP)
                        {
                            VacationsForPerson.Add(vacationForPerson);
                        }
                    }
                    foreach(VacationAllowanceViewModel vacationAllowanceForPerson in VacationAllowances)
                    {
                        if(vacationAllowanceForPerson.User_Id_SAP == item.User_Id_SAP)
                        {
                            if(!VacationAllowancesForPerson.Contains(vacationAllowanceForPerson))
                            {
                                VacationAllowancesForPerson.Add(vacationAllowanceForPerson);
                            }
                        }
                    }
                    VacationAllowancesForPerson = new ObservableCollection<VacationAllowanceViewModel>(VacationAllowancesForPerson.OrderBy(i => i.Vacation_Id));

                    Person person = new Person(item.User_Id_SAP, item.User_Id_Account, item.User_Name, item.User_Surname,
                                               item.User_Patronymic_Name, item.User_Department_Id, item.Department_Name, item.User_Virtual_Department_Id,
                                               item.Virtual_Department_Name, item.User_Sub_Department_Id, item.Role_Name, item.User_App_Color,
                                               item.User_Supervisor_Id_SAP, item.Position, VacationsForPerson, VacationAllowancesForPerson);
                    Brush brushColor;
                    if(item.User_App_Color == null)
                    {
                        brushColor = Brushes.Gray;
                    } else
                    {
                        brushColor = (Brush) converter.ConvertFromString(item.User_App_Color);
                    }
                    if(!FullPersons.Contains(person))
                    {
                        FullPersons.Add(person);
                    }
                }

                FullPersons = new List<Person>(FullPersons.OrderBy(i => i.Surname));

                for(int i = 0; i < FullPersons.Count; i++)
                {
                    if(FullPersons[i].Id_Account == Environment.UserName)
                    {
                        App.SplashScreen.status.Foreground = Brushes.Black;
                        Person = new Person(
                            FullPersons[i].Id_SAP,
                            FullPersons[i].Id_Account,
                            FullPersons[i].Name,
                            FullPersons[i].Surname,
                            FullPersons[i].Patronymic,
                            FullPersons[i].User_Department_Id,
                            FullPersons[i].Department_Name,
                            FullPersons[i].User_Virtual_Department_Id,
                            FullPersons[i].Virtual_Department_Name,
                            FullPersons[i].User_Sub_Department_Id,
                            FullPersons[i].User_Role,
                            FullPersons[i].User_App_Color,
                            FullPersons[i].User_Supervisor_Id_SAP,
                            FullPersons[i].Position,
                            FullPersons[i].User_Vacations,
                            FullPersons[i].User_Vacation_Allowances);
                        break;
                    }
                }
                // выделяю подчиненных из общего списка персон, если имя компьютера не равен итерируемому id то подчинённый
                for(int i = 0; i < FullPersons.Count; i++)
                {
                    if(FullPersons[i].Id_Account != Environment.UserName)
                    {
                        Person.Subordinates.Add(new Subordinate(
                            FullPersons[i].Id_SAP,
                            FullPersons[i].Name,
                            FullPersons[i].Surname,
                            FullPersons[i].Patronymic,
                            FullPersons[i].Position,
                            FullPersons[i].Department_Name,
                            FullPersons[i].Virtual_Department_Name,
                            FullPersons[i].User_Vacations,
                            FullPersons[i].User_Vacation_Allowances));
                    }
                }

                OnLoginSuccess?.Invoke(Person);
                return Person;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.status.Text = "Вас нет в базе данных";
                    App.SplashScreen.status.Foreground = Brushes.Red;
                });
                return null;
            }
        }

        public IEnumerable<CalendarSettings> GetSettingsForCalendar()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                IEnumerable<CalendarSettingsDTO> settingsDTOs = database.Query<CalendarSettingsDTO>("usp_Load_Settings_For_Calendar", commandType: CommandType.StoredProcedure);
                return settingsDTOs.Select(ToSettings);
            } catch(Exception)
            {
                return null;
            }
        }
        #endregion

        #region Holidays
        public async Task<IEnumerable<Holiday>> GetHolidayTypesAsync()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                IEnumerable<HolidayDTO> holidayTypesDTOs = await database.QueryAsync<HolidayDTO>("usp_Load_Holiday_Types", commandType: CommandType.StoredProcedure);

                return holidayTypesDTOs.Select(ToHolidayTypes);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public async Task<IEnumerable<HolidayViewModel>> GetHolidaysAsync(int yearCurrent, int yearNext)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Date_Year_Current = yearCurrent,
                Date_Year_Next = yearNext
            };
            try
            {

                IEnumerable<HolidayDTO> holidayDTOs = await database.QueryAsync<HolidayDTO>("usp_Load_Holidays", parameters, commandType: CommandType.StoredProcedure);
                return holidayDTOs.Select(ToHolidays);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public async Task DeleteHolidayAsync(HolidayViewModel holiday)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Holiday_Id = holiday.Id
            };
            _ = await database.QueryAsync<HolidayDTO>("usp_Delete_Holiday", parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task AddHolidayAsync(HolidayViewModel holiday)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Holiday_Id = holiday.Id,
                Holiday_Date = holiday.Date,
                Holiday_Year = holiday.Date.Year
            };
            _ = await database.QueryAsync<HolidayDTO>("usp_Add_Holiday", parameters, commandType: CommandType.StoredProcedure);
        }
        #endregion

        #region Vacations
        public async Task<IEnumerable<VacationAllowanceViewModel>> GetVacationAllowanceAsync(int UserIdSAP)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = UserIdSAP
            };
            try
            {
                IEnumerable<VacationAllowanceDTO> vacationAllowanceDTOs = await database.QueryAsync<VacationAllowanceDTO>("usp_Load_Vacation_Allowance_For_User", parameters, commandType: CommandType.StoredProcedure);
                return vacationAllowanceDTOs.Select(ToVacationAllowance);
            } catch(Exception)
            {
                return null;
            }
        }
        public async Task UpdateVacationAllowanceAsync(int userIdSAP, int vacation_Id, int year, int count)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = userIdSAP,
                Vacation_Id = vacation_Id,
                Vacation_Year = year,
                Quantity = count
            };
            try
            {
                await database.QueryAsync("usp_Update_Vacation_Allowance", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task UpdateVacationStatusAsync(Vacation vacation)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            int status = 0;
            if(vacation.Vacation_Status_Name == "Planned")
            {
                status = 1;
            } else if(vacation.Vacation_Status_Name == "On Approval")
            {
                status = 2;
            } else if(vacation.Vacation_Status_Name == "Approved")
            {
                status = 3;
            } else if(vacation.Vacation_Status_Name == "Passed to HR")
            {
                status = 4;
            } else if(vacation.Vacation_Status_Name == "Commited")
            {
                status = 5;
            } else if(vacation.Vacation_Status_Name == "Deleted")
            {
                status = 6;
            }
            object parameters = new
            {
                User_Id_SAP = vacation.User_Id_SAP,
                Vacation_Id = vacation.Vacation_Id,
                Vacation_Year = vacation.Date_Start.Year,
                Vacation_Start_Date = vacation.Date_Start,
                Vacation_End_Date = vacation.Date_end,
                Vacation_Status_Id = status
            };
            try
            {
                await database.QueryAsync("usp_Update_Vacation_Status", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task AddVacationAsync(Vacation vacation)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            int status = 0;
            if(vacation.Vacation_Status_Name == "Planned")
            {
                status = 1;
            } else if(vacation.Vacation_Status_Name == "On Approval")
            {
                status = 2;
            } else if(vacation.Vacation_Status_Name == "Approved")
            {
                status = 3;
            } else if(vacation.Vacation_Status_Name == "Passed to HR")
            {
                status = 4;
            } else if(vacation.Vacation_Status_Name == "Commited")
            {
                status = 5;
            } else if(vacation.Vacation_Status_Name == "Deleted")
            {
                status = 6;
            }
            object parameters = new
            {
                User_Id_SAP = vacation.User_Id_SAP,
                Vacation_Id = vacation.Vacation_Id,
                Vacation_Year = vacation.Date_Start.Year,
                Vacation_Start_Date = vacation.Date_Start,
                Vacation_End_Date = vacation.Date_end,
                Vacation_Status_Id = status,
                Creator_Id = Person.Id_Account
            };
            try
            {
                _ = await database.QueryAsync<Vacation>("usp_Add_Vacation", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task<IEnumerable<VacationDTO>> GetConflictingVacationAsync(Vacation vacation)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            int status = 0;
            if(vacation.Vacation_Status_Name == "Planned")
            {
                status = 1;
            } else if(vacation.Vacation_Status_Name == "On Approval")
            {
                status = 2;
            } else if(vacation.Vacation_Status_Name == "Approved")
            {
                status = 3;
            } else if(vacation.Vacation_Status_Name == "Passed to HR")
            {
                status = 4;
            } else if(vacation.Vacation_Status_Name == "Commited")
            {
                status = 5;
            } else if(vacation.Vacation_Status_Name == "Deleted")
            {
                status = 6;
            }
            object parameters = new
            {
                User_Id_SAP = vacation.User_Id_SAP,
                Vacation_Id = vacation.Vacation_Id,
                Vacation_Year = vacation.Date_Start.Year,
                Vacation_Date_Start = vacation.Date_Start,
                Vacation_Date_End = vacation.Date_end,
                Vacation_Status_Id = status
            };
            try
            {
                IEnumerable<VacationDTO> vacationDTOs = await database.QueryAsync<VacationDTO>("usp_Load_Conflicting_Vacation_For_User", parameters, commandType: CommandType.StoredProcedure);
                return vacationDTOs;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public async Task DeleteVacationAsync(Vacation vacation)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = vacation.User_Id_SAP,
                Vacation_Id = vacation.Vacation_Id,
                Vacation_Year = vacation.Date_Start.Year,
                Vacation_Start_Date = vacation.Date_Start,
                Vacation_End_Date = vacation.Date_end
            };
            try
            {
                _ = await database.QueryAsync<Vacation>("usp_Delete_Vacation", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task<IEnumerable<Vacation>> LoadVacationsAsync(int UserIdSAP)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = UserIdSAP
            };
            try
            {
                IEnumerable<VacationDTO> vacationDTOs = await database.QueryAsync<VacationDTO>("usp_Load_Vacation_For_User", parameters, commandType: CommandType.StoredProcedure);
                return vacationDTOs.Select(ToVacation);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Vacation>> LoadAllVacationsAsync()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();

            try
            {
                IEnumerable<VacationDTO> vacationDTOs = await database.QueryAsync<VacationDTO>("usp_Load_All_Vacations", commandType: CommandType.StoredProcedure);
                return vacationDTOs.Select(ToVacation);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        #endregion

        #region ToObj
        private Vacation ToVacation(VacationDTO dto)
        {
            BrushConverter converter = new System.Windows.Media.BrushConverter();
            Brush brushColor = (Brush) converter.ConvertFromString(dto.Vacation_Color);
            return new Vacation(dto.Vacation_Name, dto.User_Id_SAP, dto.Vacation_Id, dto.Count, brushColor, dto.Vacation_Start_Date, dto.Vacation_End_Date, dto.Vacation_Status_Name, dto.Creator_Id);
        }
        private VacationAllowanceViewModel ToVacationAllowance(VacationAllowanceDTO dto)
        {
            BrushConverter converter = new System.Windows.Media.BrushConverter();
            Brush brushColor = (Brush) converter.ConvertFromString(dto.Vacation_Color);
            return new VacationAllowanceViewModel(dto.User_Id_SAP, dto.Vacation_Name, dto.Vacation_Id, dto.Vacation_Year, dto.Vacation_Days_Quantity, brushColor);
        }
        private HolidayViewModel ToHolidays(HolidayDTO dto)
        {
            return new HolidayViewModel(dto.Id, dto.Holiday_Name, dto.Holiday_Date, dto.Holiday_Year);
        }
        private Holiday ToHolidayTypes(HolidayDTO dto)
        {
            return new Holiday(dto.Id, dto.Holiday_Name);
        }
        private CalendarSettings ToSettings(CalendarSettingsDTO dto)
        {
            return new CalendarSettings(dto.Static_Date_Name, dto.Static_Date);
        }

        #endregion

    }
}
