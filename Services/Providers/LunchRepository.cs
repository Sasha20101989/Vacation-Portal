﻿using Dapper;
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
        public Person Person { 
            get; 
            set; 
        }

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
        private readonly ObservableCollection<VacationViewModel> Vacations = new ObservableCollection<VacationViewModel>();
        private readonly ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
        public Action<List<VacationViewModel>> OnVacationsChanged { get; set; }
        public Action<Access> OnAccessChanged { get; set; }
        public Action<Person> OnLoginSuccess { get; set; }
        public List<PersonDTO> Persons { get; set; } = new List<PersonDTO>();
        public List<Person> FullPersons { get; set; } = new List<Person>();

        #endregion

        #region Person
        public async IAsyncEnumerable<VacationViewModel> FetchVacationsAsync(int sapId)
        {
            App.Current.Dispatcher.Invoke((Action) delegate
            {
                App.SplashScreen.status.Text = "Загружаю ваши отпуска...";
                App.SplashScreen.status.Foreground = Brushes.Black;
            });
            IEnumerable<VacationViewModel> vacations = await LoadVacationAsync(sapId);

            foreach(VacationViewModel item in vacations)
            {
                yield return item;
            }
        }
        public async IAsyncEnumerable<VacationAllowanceViewModel> FetchVacationAllowances(int sapId)
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
                BrushConverter converter = new System.Windows.Media.BrushConverter();
                IEnumerable<PersonDTO> fullPersonDTOs = await database.QueryAsync<PersonDTO>("usp_Get_Users", parametersPerson, commandType: CommandType.StoredProcedure);
                //собираю общие списки с отпусками и доступными днями
                foreach(PersonDTO personDTO in fullPersonDTOs)
                {

                    await foreach(VacationViewModel vacation in FetchVacationsAsync(personDTO.User_Id_SAP))
                    {
                        Brush brushColor;
                        if(vacation.Color == null)
                        {
                            brushColor = Brushes.Gray;
                        } else
                        {
                            brushColor = vacation.Color;
                        }
                        VacationViewModel vacationViewModel = new VacationViewModel(vacation.Name,
                                                            vacation.User_Id_SAP, vacation.Vacation_Id,
                                                            brushColor, vacation.DateStart, vacation.DateEnd,
                                                            vacation.Status, vacation.Creator_Id);
                        if(!Vacations.Contains(vacationViewModel))
                        {
                            Vacations.Add(vacationViewModel);
                        }
                    }

                    await foreach(VacationAllowanceViewModel vacationAllowance in FetchVacationAllowances(personDTO.User_Id_SAP))
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
                    ObservableCollection<VacationViewModel> VacationsForPerson = new ObservableCollection<VacationViewModel>();
                    ObservableCollection<VacationAllowanceViewModel> VacationAllowancesForPerson = new ObservableCollection<VacationAllowanceViewModel>();
                    foreach(VacationViewModel vacationForPerson in Vacations)
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
                                               item.User_Patronymic_Name, item.User_Department_Id, item.User_Virtual_Department_Id,
                                               item.User_Sub_Department_Id, item.Role_Name, item.User_App_Color,
                                               item.User_Supervisor_Id_SAP,item.Position, VacationsForPerson, VacationAllowancesForPerson);
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
                // выделяю текущего пользователя из общего списка персон, если имя компьютера равно итерируемому id то персон
                for(int i = 0; i < FullPersons.Count; i++)
                {
                    if(FullPersons[i].Id_Account == Environment.UserName)
                    {
                        Person = new Person(
                            FullPersons[i].Id_SAP,
                            FullPersons[i].Id_Account,
                            FullPersons[i].Name,
                            FullPersons[i].Surname,
                            FullPersons[i].Patronymic,
                            FullPersons[i].User_Department_Id,
                            FullPersons[i].User_Virtual_Department_Id,
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
        public async Task AddVacationAsync(Vacation vacation)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            int status = 0;
            if(vacation.Status == "Новый")
            {
                status = 1;
            } else if(vacation.Status == "На согласовании")
            {
                status = 2;
            } else if(vacation.Status == "Согласован")
            {
                status = 3;
            } else if(vacation.Status == "Перешел в отдел кадров")
            {
                status = 4;
            } else if(vacation.Status == "Проведён")
            {
                status = 5;
            } else if(vacation.Status == "Удалён")
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
            if(vacation.Status == "Новый")
            {
                status = 1;
            } else if(vacation.Status == "На согласовании")
            {
                status = 2;
            } else if(vacation.Status == "Согласован")
            {
                status = 3;
            } else if(vacation.Status == "Перешел в отдел кадров")
            {
                status = 4;
            } else if(vacation.Status == "Проведён")
            {
                status = 5;
            } else if(vacation.Status == "Удалён")
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
        public async Task<IEnumerable<VacationViewModel>> LoadVacationAsync(int UserIdSAP)
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
        #endregion

        #region ToObj
        private VacationViewModel ToVacation(VacationDTO dto)
        {
            BrushConverter converter = new System.Windows.Media.BrushConverter();
            Brush brushColor = (Brush) converter.ConvertFromString(dto.Vacation_Color);
            return new VacationViewModel(dto.Vacation_Name, dto.User_Id_SAP, dto.Vacation_Id, brushColor, dto.Vacation_Start_Date, dto.Vacation_End_Date, dto.Vacation_Status_Id, dto.Creator_Id);
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
