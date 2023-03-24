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
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers {
    public class LunchRepository : ILunchRepository {
        public SqlTableDependency<HolidayDTO> tableDependencyHoliday;
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;
        public ICommand LoadHolidays { get; } = new LoadHolidaysCommand();
        public ICommand LoadHolidayTypes { get; } = new LoadHolidayTypesCommand();
        public ICommand Login { get; } = new LoginCommand();

        #region PropChange
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T member, T value, [CallerMemberName] string propertyName = null) {
            if(EqualityComparer<T>.Default.Equals(member, value)) {
                return false;
            }
            member = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public LunchRepository(SqlDbConnectionFactory sqlDbConnectionFactory) {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
            AllStatuses = GetStatuses();
        }
        private ObservableCollection<Subordinate> _personsWithVacationsOnApproval = new ObservableCollection<Subordinate>();
        public ObservableCollection<Subordinate> PersonsWithVacationsOnApproval {
            get => _personsWithVacationsOnApproval;
            set {
                _personsWithVacationsOnApproval = value;
                OnPropertyChanged(nameof(PersonsWithVacationsOnApproval));
            }
        }
        private ObservableCollection<Vacation> _processedVacations = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> ProcessedVacations {
            get => _processedVacations;
            set {
                _processedVacations = value;
                OnPropertyChanged(nameof(ProcessedVacations));
            }
        }

        private ObservableCollection<Vacation> _vacationsOnApproval = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsOnApproval {
            get => _vacationsOnApproval;
            set {
                _vacationsOnApproval = value;
                OnPropertyChanged(nameof(VacationsOnApproval));
            }
        }

        #region Props
        public Person Person { get; set; }
        public DateTime DateUnblockNextCalendar { get; set; }
        public bool IsCalendarUnblocked { get; set; }

        public bool CheckDateUnblockedCalendarAsync() {
            IEnumerable<CalendarSettings> calendarSettings = GetSettingsForCalendar();
            foreach(CalendarSettings settings in calendarSettings) {
                if(settings.Setting_Name == "NextCalendarUnlock") {
                    DateUnblockNextCalendar = settings.Setting_Date;
                    IsCalendarUnblocked = DateUnblockNextCalendar <= DateTime.UtcNow;
                    return IsCalendarUnblocked;
                }
            }
            return false;
        }

        public bool CheckNextCalendarPlanningUnlock() {
            IEnumerable<CalendarSettings> calendarSettings = GetSettingsForCalendar();
            foreach(CalendarSettings settings in calendarSettings) {
                if(settings.Setting_Name == "NextCalendarPlanningUnlock") {
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
        public ObservableCollection<HolidayViewModel> Holidays {
            get => _holidays;
            set {
                _holidays = value;
                OnPropertyChanged(nameof(Holidays));
            }
        }
        private ObservableCollection<Holiday> _holidayTypes = new ObservableCollection<Holiday>();
        public ObservableCollection<Holiday> HolidayTypes {
            get => _holidayTypes;
            set {
                _holidayTypes = value;
                OnPropertyChanged(nameof(HolidayTypes));
            }
        }
        private readonly ObservableCollection<Vacation> Vacations = new ObservableCollection<Vacation>();
        private readonly ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
        public Action<List<VacationViewModel>> OnVacationsChanged { get; set; }
        public Action<Person> OnLoginSuccess { get; set; }
        public ObservableCollection<PersonDTO> Persons { get; set; } = new ObservableCollection<PersonDTO>();
        public ObservableCollection<Person> FullPersons { get; set; } = new ObservableCollection<Person>();

        public List<Status> AllStatuses { get; set; }
        public ObservableCollection<SvApprovalStateViewModel> PersonStates { get; set; } = new ObservableCollection<SvApprovalStateViewModel>();

        #endregion
        public List<Status> GetStatuses() {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try {
                IEnumerable<Status> statusDTOs = database.Query<Status>("usp_Get_Statuses", commandType: CommandType.StoredProcedure);
                List<Status> allStatuses = statusDTOs.ToList();
                return allStatuses;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        #region Person

        public async IAsyncEnumerable<Vacation> FetchVacationsAsync(int sapId) {
            IEnumerable<Vacation> vacations = await LoadVacationsAsync(sapId);

            foreach(Vacation item in vacations) {
                yield return item;
            }
        }
        public async IAsyncEnumerable<VacationAllowanceViewModel> FetchVacationAllowancesAsync(int sapId) {
            IEnumerable<VacationAllowanceViewModel> vacationAllowances = await GetVacationAllowanceAsync(sapId);
            foreach(VacationAllowanceViewModel item in vacationAllowances) {
                yield return item;
            }
        }
        public async Task<Person> LoginAsync(string account) {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try {
                object parametersPerson = new {
                    Account = account
                };

                BrushConverter converter = new System.Windows.Media.BrushConverter();
                App.SplashScreen.status.Text = "В поисках сотрудников";
                IEnumerable<PersonDTO> fullPersonDTOs = await database.QueryAsync<PersonDTO>("usp_Get_Users", parametersPerson, commandType: CommandType.StoredProcedure);

                
                App.SplashScreen.status.Foreground = Brushes.Black;
                App.SplashScreen.progressBar.Value = 10;
                App.SplashScreen.status.Text = "В поисках отпусков";
                foreach(PersonDTO personDTO in fullPersonDTOs) {
                    await foreach(Vacation vacation in FetchVacationsAsync(personDTO.User_Id_SAP)) {
                        
                        Brush brushColor;
                        if(vacation.Color == null) {
                            brushColor = Brushes.Gray;
                        } else {
                            brushColor = vacation.Color;
                        }

                        if(!Vacations.Contains(vacation)) {
                            vacation.User_Name = personDTO.User_Name;
                            vacation.User_Surname = personDTO.User_Surname;
                            Vacations.Add(vacation);
                        }
                    }

                    await foreach(VacationAllowanceViewModel vacationAllowance in FetchVacationAllowancesAsync(personDTO.User_Id_SAP)) {
                        VacationAllowanceViewModel vacationAllowanceViewModel = new VacationAllowanceViewModel(vacationAllowance.User_Id_SAP, vacationAllowance.Vacation_Name, vacationAllowance.Vacation_Id, vacationAllowance.Vacation_Year, vacationAllowance.Vacation_Days_Quantity, vacationAllowance.Vacation_Color);
                        if(!VacationAllowances.Contains(vacationAllowanceViewModel)) {
                            VacationAllowances.Add(vacationAllowanceViewModel);
                        }
                    }

                }
                App.SplashScreen.status.Text = "собираю общий список с персонами и их отпусками";
                //собираю общий список с персонами и их отпусками
                foreach(PersonDTO item in fullPersonDTOs) {
                    ObservableCollection<Vacation> VacationsForPerson = new ObservableCollection<Vacation>();
                    ObservableCollection<VacationAllowanceViewModel> VacationAllowancesForPerson = new ObservableCollection<VacationAllowanceViewModel>();
                    foreach(Vacation vacationForPerson in Vacations) {
                        if(vacationForPerson.User_Id_SAP == item.User_Id_SAP) {
                            VacationsForPerson.Add(vacationForPerson);
                        }
                    }
                    foreach(VacationAllowanceViewModel vacationAllowanceForPerson in VacationAllowances) {
                        if(vacationAllowanceForPerson.User_Id_SAP == item.User_Id_SAP) {
                            if(!VacationAllowancesForPerson.Contains(vacationAllowanceForPerson)) {
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
                    if(item.User_App_Color == null) {
                        brushColor = Brushes.Gray;
                    } else {
                        brushColor = (Brush) converter.ConvertFromString(item.User_App_Color);
                    }
                    if(!FullPersons.Contains(person)) {
                        FullPersons.Add(person);
                    }
                }

                FullPersons = new ObservableCollection<Person>(FullPersons.OrderBy(i => i.Surname));

                for(int i = 0; i < FullPersons.Count; i++) {
                    if(FullPersons[i].Id_Account == Environment.UserName) {
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
                for(int i = 0; i < FullPersons.Count; i++) {
                    if(FullPersons[i].Id_Account != Environment.UserName) {
                        Subordinate subordinate = new Subordinate(
                            FullPersons[i].Id_SAP,
                            FullPersons[i].Name,
                            FullPersons[i].Surname,
                            FullPersons[i].Patronymic,
                            FullPersons[i].Position,
                            FullPersons[i].Department_Name,
                            FullPersons[i].Virtual_Department_Name,
                            FullPersons[i].User_Vacations,
                            FullPersons[i].User_Vacation_Allowances);
                        
                        Person.Subordinates.Add(subordinate);
                    }
                }

                PersonStates = await GetStateVacationsOnApproval(Person.Id_SAP);
                Person.Subordinates.ToList().ForEach(subordinate => subordinate.UpdateStatesCount());
                OnLoginSuccess?.Invoke(Person);
                return Person;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
                Application.Current.Dispatcher.Invoke((Action) delegate {
                    App.SplashScreen.status.Text = "Вас нет в базе данных";
                    App.SplashScreen.status.Foreground = Brushes.Red;
                });
                return null;
            }
        }
        public void GetPersonsWithVacationsOnApproval() {
            PersonsWithVacationsOnApproval.Clear();
            foreach(Subordinate subordinate in App.API.Person.Subordinates) {
                foreach(Vacation vacation in subordinate.Subordinate_Vacations) {
                    if(vacation.Vacation_Status_Name == MyEnumExtensions.ToDescriptionString(Statuses.OnApproval)) {

                        if(!PersonsWithVacationsOnApproval.Contains(subordinate)) {
                            PersonsWithVacationsOnApproval.Add(subordinate);
                        }
                    }
                }
            }

            //TODO: когда PersonsWithVacationsOnApproval.Count == 0
            //идем в таблицу SV_approval_state
            //ищем все записи со своим SAP ID 
            //удаляем эти записи в таблицу SV_approval_state

            //TODO: если PersonsWithVacationsOnApproval.Count > 0
            //добавляем эти записи в таблицу SV_approval_state




            //отображение кнопки когда в state table нет states со статусом 2
            //при старте проги из draft отпуска со статусом 2 для текущего SV переносятся в SV_approval_state только те которых нет
            //SV при изменении статуса меняет статус только в state table
            //regected: поменяли статус в state table
            //approval: меняется только в state table 
            //взаимодействие пользователя с кнопкой из state меняет статусы отпускам
            //после этого удаляет этот state
            //уведомление рядом с кнопкой что прибриближается закрытие планирования и в случае если вы не нажмете ее сами мы нажмем ее за вас, при этом заапрувятся тке которые были отменены
            //добавить dateTimeStamp в драфт таблицу
        }
        public IEnumerable<CalendarSettings> GetSettingsForCalendar() {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try {
                IEnumerable<CalendarSettingsDTO> settingsDTOs = database.Query<CalendarSettingsDTO>("usp_Load_Settings_For_Calendar", commandType: CommandType.StoredProcedure);
                return settingsDTOs.Select(ToSettings);
            } catch(Exception) {
                return null;
            }
        }
        #endregion

        #region Holidays
        public async Task<IEnumerable<Holiday>> GetHolidayTypesAsync() {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try {
                IEnumerable<HolidayDTO> holidayTypesDTOs = await database.QueryAsync<HolidayDTO>("usp_Load_Holiday_Types", commandType: CommandType.StoredProcedure);

                return holidayTypesDTOs.Select(ToHolidayTypes);
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public async Task<IEnumerable<HolidayViewModel>> GetHolidaysAsync(int yearCurrent, int yearNext) {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new {
                Date_Year_Current = yearCurrent,
                Date_Year_Next = yearNext
            };
            try {

                IEnumerable<HolidayDTO> holidayDTOs = await database.QueryAsync<HolidayDTO>("usp_Load_Holidays", parameters, commandType: CommandType.StoredProcedure);
                return holidayDTOs.Select(ToHolidays);
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public async Task DeleteHolidayAsync(HolidayViewModel holiday) {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new {
                Holiday_Id = holiday.Id
            };
            _ = await database.QueryAsync<HolidayDTO>("usp_Delete_Holiday", parameters, commandType: CommandType.StoredProcedure);
        }
        public async Task AddHolidayAsync(HolidayViewModel holiday) {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new {
                Holiday_Id = holiday.Id,
                Holiday_Date = holiday.Date,
                Holiday_Year = holiday.Date.Year
            };
            _ = await database.QueryAsync<HolidayDTO>("usp_Add_Holiday", parameters, commandType: CommandType.StoredProcedure);
        }
        #endregion

        #region Vacations

        public async Task UpdateStateStatusAsync(SvApprovalStateViewModel state)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();

            object parameters = new
            {
                Id = state.Id,
                Status_Id = state.StatusId
            };
            try
            {
                await database.QueryAsync("usp_Update_Sv_Approval_State_By_Id", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task<ObservableCollection<SvApprovalStateViewModel>> GetStateVacationsOnApproval(int UserIdSAP)
        {
            
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new
            {
                User_Id_SAP = UserIdSAP
            };
            try
            {
                ObservableCollection<SvApprovalStateViewModel> personStateViewModels = new ObservableCollection<SvApprovalStateViewModel>();

                IEnumerable<SvApprovalStateDTO> svApprovalStateViewModelsDTOs = await database.QueryAsync<SvApprovalStateDTO>("usp_Get_Sv_Approval_State_By_Sv", parameters, commandType: CommandType.StoredProcedure);
                
                foreach(SvApprovalStateDTO state in svApprovalStateViewModelsDTOs)
                {
                    var vacations = Person.Subordinates.Select(s => s.Subordinate_Vacations);

                    foreach(Subordinate subordinate in Person.Subordinates)
                    {
                        foreach(Vacation vacation in subordinate.Subordinate_Vacations)
                        {
                            if(vacation.Id == state.Vacation_Record_Id)
                            {
                                personStateViewModels.Add(
                                    new SvApprovalStateViewModel(
                                        state.Id,
                                        state.Vacation_Record_Id,
                                        state.Supervisor_Id,
                                        state.Status_Id, 
                                        vacation)
                                    );
                            }
                        }
                    }
                }
                return personStateViewModels;
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public async Task<IEnumerable<VacationAllowanceViewModel>> GetVacationAllowanceAsync(int UserIdSAP) {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new {
                User_Id_SAP = UserIdSAP
            };
            try {
                IEnumerable<VacationAllowanceDTO> vacationAllowanceDTOs = await database.QueryAsync<VacationAllowanceDTO>("usp_Load_Vacation_Allowance_For_User", parameters, commandType: CommandType.StoredProcedure);
                return vacationAllowanceDTOs.Select(ToVacationAllowance);
            } catch(Exception) {
                return null;
            }
        }
        public async Task UpdateVacationAllowanceAsync(int userIdSAP, int vacation_Id, int year, int count) {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new {
                User_Id_SAP = userIdSAP,
                Vacation_Id = vacation_Id,
                Vacation_Year = year,
                Quantity = count
            };
            try {
                await database.QueryAsync("usp_Update_Vacation_Allowance", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public async Task UpdateVacationStatusAsync(Vacation vacation) {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();

            object parameters = new {
                User_Id_SAP = vacation.User_Id_SAP,
                Vacation_Id = vacation.Vacation_Id,
                Vacation_Year = vacation.Date_Start.Year,
                Vacation_Start_Date = vacation.Date_Start,
                Vacation_End_Date = vacation.Date_end,
                Vacation_Status_Id = vacation.Vacation_Status_Id
            };
            try {
                await database.QueryAsync("usp_Update_Vacation_Status", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task<Vacation> AddVacationAsync(Vacation vacation) {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();

            var parameters = new DynamicParameters();
            parameters.Add("@User_Id_SAP", vacation.User_Id_SAP);
            parameters.Add("@Vacation_Id", vacation.Vacation_Id);
            parameters.Add("@Vacation_Year", vacation.Date_Start.Year);
            parameters.Add("@Vacation_Start_Date", vacation.Date_Start);
            parameters.Add("@Vacation_End_Date", vacation.Date_end);
            parameters.Add("@Vacation_Status_Id", vacation.Vacation_Status_Id);
            parameters.Add("@Creator_Id", Person.Id_Account);
            parameters.Add("@InsertedId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            try {
                await database.ExecuteAsync(
                    "usp_Add_Vacation", parameters, commandType: CommandType.StoredProcedure);

                vacation.Id = parameters.Get<int>("@InsertedId");

                return vacation;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public async Task<IEnumerable<VacationDTO>> GetConflictingVacationAsync(Vacation vacation) {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();

            object parameters = new {
                User_Id_SAP = vacation.User_Id_SAP,
                Vacation_Id = vacation.Vacation_Id,
                Vacation_Year = vacation.Date_Start.Year,
                Vacation_Date_Start = vacation.Date_Start,
                Vacation_Date_End = vacation.Date_end,
                Vacation_Status_Id = vacation.Vacation_Status_Id
            };
            try {
                IEnumerable<VacationDTO> vacationDTOs = await database.QueryAsync<VacationDTO>("usp_Load_Conflicting_Vacation_For_User", parameters, commandType: CommandType.StoredProcedure);
                return vacationDTOs;
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public async Task DeleteVacationAsync(Vacation vacation) {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new {
                Id = vacation.Id
            };
            try {
                _ = await database.QueryAsync<Vacation>("usp_Delete_Vacation", parameters, commandType: CommandType.StoredProcedure);
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task<IEnumerable<Vacation>> LoadVacationsAsync(int UserIdSAP) {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            object parameters = new {
                User_Id_SAP = UserIdSAP
            };
            try {
                IEnumerable<VacationDTO> vacationDTOs = await database.QueryAsync<VacationDTO>("usp_Load_Vacation_For_User", parameters, commandType: CommandType.StoredProcedure);
                return vacationDTOs.Select(ToVacation);
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Vacation>> LoadAllVacationsAsync() {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();

            try {
                IEnumerable<VacationDTO> vacationDTOs = await database.QueryAsync<VacationDTO>("usp_Load_All_Vacations", commandType: CommandType.StoredProcedure);
                return vacationDTOs.Select(ToVacation);
            } catch(Exception ex) {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        #endregion

        private string ReturnUserName(int sapId) {
            foreach(Person person in FullPersons) {
                if(person.Id_SAP == sapId) {
                    return person.Name;
                }
            }
            return null;
        }

        private string ReturnUserSurname(int sapId) {
            foreach(Person person in FullPersons) {
                if(person.Id_SAP == sapId) {
                    return person.Surname;
                }
            }
            return null;
        }

        #region ToObj
        private Vacation ToVacation(VacationDTO dto) {
            BrushConverter converter = new BrushConverter();
            Brush brushColor = (Brush) converter.ConvertFromString(dto.Vacation_Color);
            return new Vacation(dto.Id, dto.Vacation_Name, dto.User_Id_SAP, ReturnUserName(dto.User_Id_SAP), ReturnUserSurname(dto.User_Id_SAP), dto.Vacation_Id, dto.Count, brushColor, dto.Vacation_Start_Date, dto.Vacation_End_Date, dto.Vacation_Status_Id, dto.Creator_Id);
        }
        private VacationAllowanceViewModel ToVacationAllowance(VacationAllowanceDTO dto) {
            BrushConverter converter = new System.Windows.Media.BrushConverter();
            Brush brushColor = (Brush) converter.ConvertFromString(dto.Vacation_Color);
            return new VacationAllowanceViewModel(dto.User_Id_SAP, dto.Vacation_Name, dto.Vacation_Id, dto.Vacation_Year, dto.Vacation_Days_Quantity, brushColor);
        }
        private HolidayViewModel ToHolidays(HolidayDTO dto) {
            return new HolidayViewModel(dto.Id, dto.Holiday_Name, dto.Holiday_Date, dto.Holiday_Year);
        }
        private Holiday ToHolidayTypes(HolidayDTO dto) {
            return new Holiday(dto.Id, dto.Holiday_Name);
        }
        private CalendarSettings ToSettings(CalendarSettingsDTO dto) {
            return new CalendarSettings(dto.Static_Date_Name, dto.Static_Date);
        }

        #endregion

    }
}
