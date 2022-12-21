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
        public bool IsCalendarUnblocked { get; set; } = true;
        //public bool IsCalendarUnblocked => DateUnblockNextCalendar <= DateTime.Now;

        public DateTime DateUnblockPlanning { get; set; }
        public bool IsCalendarPlannedOpen { get; set; } = true;
        //public bool IsCalendarPlannedOpen => DateUnblockPlanning <= DateTime.Now;

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
        private readonly List<VacationViewModel> Vacations = new List<VacationViewModel>();
        public Action<List<VacationViewModel>> OnVacationsChanged { get; set; }
        public Action<Access> OnAccessChanged { get; set; }
        public Action<Person> OnLoginSuccess { get; set; }
        public List<PersonDTO> Persons { get; set; } = new List<PersonDTO>();
        public List<Person> FullPersons { get; set; } = new List<Person>();

        #endregion

        #region Person

        //public async Task<Person> Login(string account)
        //{
        //    using IDbConnection database = _sqlDbConnectionFactory.Connect();
        //    try
        //    {
        //        object parametersPerson = new
        //        {
        //            Account = account
        //        };
        //        using(var multi = await database.QueryMultipleAsync("usp_Get_User_new", parametersPerson, commandType: CommandType.StoredProcedure))
        //        {
        //            BrushConverter converter = new System.Windows.Media.BrushConverter();
        //            IEnumerable<FullPersonDTO> fullPersonDTOs = await multi.ReadAsync<FullPersonDTO>();
        //            foreach(FullPersonDTO item in fullPersonDTOs)
        //            {
        //                Brush brushColor = (Brush) converter.ConvertFromString(item.Vacation_Color);
        //                Vacations.Add(new VacationViewModel(item.Vacation_Name, 
        //                                                    item.User_Id_SAP, item.Vacation_Id, brushColor, item.Vacation_Start_Date, 
        //                                                    item.Vacation_End_Date, item.Vacation_Status, item.Creator_Id));
        //            }
        //            foreach(FullPersonDTO item in fullPersonDTOs)
        //            {
        //                Person person = new Person(item.User_Id_SAP, item.User_Id_Account, item.User_Name, item.User_Surname, 
        //                                           item.User_Patronymic_Name, item.User_Department_Id, item.User_Virtual_Department_Id, 
        //                                           item.User_Sub_Department_Id, item.User_Position_Id, item.Role_Name, item.User_App_Color, 
        //                                           item.User_Supervisor_Id_SAP, Vacations);
        //                if(!FullPersons.Contains(person))
        //                {
        //                    FullPersons.Add(person);
        //                }
        //            }
        //            isNext = multi.IsConsumed;
        //            Person = new Person(FullPersons[0].Id_SAP,
        //                        FullPersons[0].Id_Account,
        //                        FullPersons[0].Name,
        //                        FullPersons[0].Surname,
        //                        FullPersons[0].Patronymic,
        //                        FullPersons[0].User_Department_Id,
        //                        FullPersons[0].User_Virtual_Department_Id,
        //                        FullPersons[0].User_Sub_Department_Id,
        //                        FullPersons[0].Position,
        //                        FullPersons[0].User_Role,
        //                        FullPersons[0].User_App_Color,
        //                        FullPersons[0].User_Supervisor_Id_SAP,
        //                        FullPersons[0].User_Vacations);
        //            return Person;
        //        }
        //    } catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        Application.Current.Dispatcher.Invoke((Action) delegate
        //        {
        //            App.SplashScreen.status.Text = "Вас нет в базе данных";
        //            App.SplashScreen.status.Foreground = Brushes.Red;
        //        });
        //        return null;
        //    }
        //}
        public async Task<Person> LoginAsyncNew(string account)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                object parametersPerson = new
                {
                    Account = account
                };
                using(SqlMapper.GridReader multi = await database.QueryMultipleAsync("usp_Get_User_new", parametersPerson, commandType: CommandType.StoredProcedure))
                {
                    BrushConverter converter = new System.Windows.Media.BrushConverter();
                    //IEnumerable<FullPersonDTO> fullPersonDTOs = await multi.ReadAsync<FullPersonDTO>();
                    IEnumerable<PersonDTO> persons = await multi.ReadAsync<PersonDTO>();
                    IEnumerable<VacationDTO> vacations = await multi.ReadAsync<VacationDTO>();
                    IEnumerable<IGrouping<int, PersonDTO>> partitioned = persons.OrderBy(data => data.User_Id_SAP).GroupBy(data => data.User_Id_SAP);
                    foreach(IGrouping<int, PersonDTO> grouping in partitioned)
                    {
                        Console.WriteLine(grouping.Key);
                        foreach(PersonDTO personItem in grouping)
                        {
                            Person = new Person(personItem.User_Id_SAP,
                                                       personItem.User_Id_Account,
                                                       personItem.Role_Name,
                                                       personItem.User_Surname,
                                                       personItem.User_Patronymic_Name,
                                                       personItem.User_Department_Id,
                                                       personItem.User_Virtual_Department_Id,
                                                       personItem.User_Sub_Department_Id,
                                                       personItem.User_Position_Id,
                                                       personItem.Role_Name,
                                                       personItem.User_App_Color,
                                                       personItem.User_Supervisor_Id_SAP,
                                                       personItem.User_Vacations);

                            Console.WriteLine(Person);
                            Person.User_Vacations = new List<VacationViewModel>();
                            foreach(VacationDTO vacation in vacations)
                            {
                                if(vacation.User_Id_SAP == grouping.Key)
                                {
                                    foreach(PersonDTO data in grouping)
                                    {
                                        Brush brushColor = (Brush) converter.ConvertFromString(vacation.Vacation_Color);
                                        VacationViewModel vacationItem = new VacationViewModel(vacation.Vacation_Name,
                                                                            vacation.User_Id_SAP,
                                                                            vacation.Vacation_Id,
                                                                            brushColor,
                                                                            vacation.Vacation_Start_Date,
                                                                            vacation.Vacation_End_Date,
                                                                            vacation.Vacation_Status,
                                                                            vacation.Creator_Id);
                                        Person.User_Vacations.Add(vacationItem);
                                        Console.WriteLine(vacationItem);
                                    }
                                }
                            }
                            Console.WriteLine(Person);
                        }
                        Console.WriteLine("--");
                    }

                    OnLoginSuccess?.Invoke(Person);

                    return Person;
                }
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
        public async Task<IEnumerable<PersonDTO>> LoginAsync(string account)
        {
            BrushConverter converter = new System.Windows.Media.BrushConverter();
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                object parametersPerson = new
                {
                    Account = account
                };
                IEnumerable<PersonDTO> userDTOs = await database.QueryAsync<PersonDTO>("usp_Get_User", parametersPerson, commandType: CommandType.StoredProcedure);
                Persons.AddRange(userDTOs);
                object parametersVacation = new
                {
                    User_Id_SAP = Persons[0].User_Id_SAP,
                    Year = 2022
                };
                IEnumerable<VacationDTO> vacationDTOs = await database.QueryAsync<VacationDTO>("usp_Load_Vacation_For_User", parametersVacation, commandType: CommandType.StoredProcedure);

                foreach(VacationDTO item in vacationDTOs)
                {
                    Brush brushColor = (Brush) converter.ConvertFromString(item.Vacation_Color);
                    Vacations.Add(new VacationViewModel(item.Vacation_Name, item.User_Id_SAP, item.Vacation_Id, brushColor, item.Vacation_Start_Date, item.Vacation_End_Date, item.Vacation_Status, item.Creator_Id));
                }
                Person = new Person(Persons[0].User_Id_SAP,
                                    Persons[0].User_Id_Account,
                                    Persons[0].User_Name,
                                    Persons[0].User_Surname,
                                    Persons[0].User_Patronymic_Name,
                                    Persons[0].User_Department_Id,
                                    Persons[0].User_Virtual_Department_Id,
                                    Persons[0].User_Sub_Department_Id,
                                    Persons[0].User_Position_Id,
                                    Persons[0].Role_Name,
                                    Persons[0].User_App_Color,
                                    Persons[0].User_Supervisor_Id_SAP,
                                    Vacations);

                OnLoginSuccess?.Invoke(Person);

                return userDTOs;
            } catch(Exception)
            {
                Application.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.status.Text = "Вас нет в базе данных";
                    App.SplashScreen.status.Foreground = Brushes.Red;
                });
                return null;
            }
        }
        public async Task<IEnumerable<Settings>> GetSettingsAsync(string account)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Account = account
            };
            try
            {
                IEnumerable<SettingsDTO> settingsDTOs = await database.QueryAsync<SettingsDTO>("usp_Load_Settings_For_User", parameters, commandType: CommandType.StoredProcedure);
                return settingsDTOs.Select(ToSettings);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public async Task<IEnumerable<Access>> GetAccessAsync(string account)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                Account = account
            };
            try
            {
                IEnumerable<AccessDTO> accessDTOs = await database.QueryAsync<AccessDTO>("usp_Load_Access_For_User", parameters, commandType: CommandType.StoredProcedure);
                return accessDTOs.Select(ToAccess);
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
        public async Task<IEnumerable<VacationAllowanceViewModel>> GetVacationAllowanceAsync(int UserIdSAP, int year)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = UserIdSAP,
                Year = year
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
        public async Task UpdateVacationAllowanceAsync(int vacation_Id, int year, int count)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = Person.Id_SAP,
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
        public async Task AddVacationAsync(Vacation vacation)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = vacation.User_Id_SAP,
                Vacation_Id = vacation.Vacation_Id,
                Vacation_Year = vacation.Date_Start.Year,
                Vacation_Start_Date = vacation.Date_Start,
                Vacation_End_Date = vacation.Date_end,
                Vacation_Status = vacation.Status,
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
            object parameters = new
            {
                User_Id_SAP = vacation.User_Id_SAP,
                Vacation_Id = vacation.Vacation_Id,
                Vacation_Year = vacation.Date_Start.Year,
                Vacation_Date_Start = vacation.Date_Start,
                Vacation_Date_End = vacation.Date_end,
                Vacation_Status = vacation.Status
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
        public async Task<IEnumerable<VacationViewModel>> LoadVacationAsync(int UserIdSAP, int year)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = UserIdSAP,
                Year = year
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
        #endregion

        #region ToObj
        private VacationViewModel ToVacation(VacationDTO dto)
        {
            BrushConverter converter = new System.Windows.Media.BrushConverter();
            Brush brushColor = (Brush) converter.ConvertFromString(dto.Vacation_Color);
            return new VacationViewModel(dto.Vacation_Name, dto.User_Id_SAP, dto.Vacation_Id, brushColor, dto.Vacation_Start_Date, dto.Vacation_End_Date, dto.Vacation_Status, dto.Creator_Id);
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
        private Settings ToSettings(SettingsDTO dto)
        {
            return new Settings(dto.User_Id_Account, dto.Color);
        }
        private Access ToAccess(AccessDTO dto)
        {
            return new Access(dto.Is_Worker, dto.Is_HR, dto.Is_Accounting, dto.Is_Supervisor);
        }

        #endregion

    }
}
