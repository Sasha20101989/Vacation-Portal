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

        #region Props
        private Person _person;
        public Person Person
        {
            get => _person;
            set
            {
                _person = value;
                OnPropertyChanged(nameof(Person));
            }
        }

        public Action<Person> OnLoginSuccess { get; set; }

        public ObservableCollection<PersonDTO> Persons { get; set; } = new ObservableCollection<PersonDTO>();

        public ObservableCollection<Person> FullPersons { get; set; } = new ObservableCollection<Person>();

        private ObservableCollection<Subordinate> _personsWithVacationsOnApproval = new ObservableCollection<Subordinate>();
        public ObservableCollection<Subordinate> PersonsWithVacationsOnApproval
        {
            get => _personsWithVacationsOnApproval;
            set
            {
                _personsWithVacationsOnApproval = value;
                OnPropertyChanged(nameof(PersonsWithVacationsOnApproval));
            }
        }

        #endregion

        public ICommand Login { get; } = new LoginCommand();

        public LunchRepository(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }

        public async IAsyncEnumerable<Vacation> FetchVacationsAsync(int sapId) {
            IEnumerable<Vacation> vacations = await App.VacationAPI.LoadVacationsAsync(sapId);

            foreach(Vacation item in vacations) {
                yield return item;
            }
        }
        public async IAsyncEnumerable<VacationAllowanceViewModel> FetchVacationAllowancesAsync(int sapId) {
            IEnumerable<VacationAllowanceViewModel> vacationAllowances = await App.VacationAllowanceAPI.GetVacationAllowanceAsync(sapId);
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
                    await foreach(Vacation vacation in FetchVacationsAsync(personDTO.Id)) {
                        
                        Brush brushColor;
                        if(vacation.Color == null) {
                            brushColor = Brushes.Gray;
                        } else {
                            brushColor = vacation.Color;
                        }

                        if(!App.VacationAPI.Vacations.Contains(vacation)) {
                            vacation.User_Name = personDTO.Name;
                            vacation.User_Surname = personDTO.Surname;
                            App.VacationAPI.Vacations.Add(vacation);
                        }
                    }

                    await foreach(VacationAllowanceViewModel vacationAllowance in FetchVacationAllowancesAsync(personDTO.Id)) {
                        VacationAllowanceViewModel vacationAllowanceViewModel = new VacationAllowanceViewModel(vacationAllowance.User_Id_SAP, vacationAllowance.Vacation_Name, vacationAllowance.Vacation_Id, vacationAllowance.Vacation_Year, vacationAllowance.Vacation_Days_Quantity, vacationAllowance.Vacation_Color);
                        if(!App.VacationAllowanceAPI.VacationAllowances.Contains(vacationAllowanceViewModel)) {
                            App.VacationAllowanceAPI.VacationAllowances.Add(vacationAllowanceViewModel);
                        }
                    }

                }
                App.SplashScreen.status.Text = "собираю общий список с персонами и их отпусками";
                //собираю общий список с персонами и их отпусками
                foreach(PersonDTO item in fullPersonDTOs) {
                    ObservableCollection<Vacation> VacationsForPerson = new ObservableCollection<Vacation>();
                    ObservableCollection<VacationAllowanceViewModel> VacationAllowancesForPerson = new ObservableCollection<VacationAllowanceViewModel>();
                    foreach(Vacation vacationForPerson in App.VacationAPI.Vacations) {
                        if(vacationForPerson.User_Id_SAP == item.Id) {
                            VacationsForPerson.Add(vacationForPerson);
                        }
                    }
                    foreach(VacationAllowanceViewModel vacationAllowanceForPerson in App.VacationAllowanceAPI.VacationAllowances) {
                        if(vacationAllowanceForPerson.User_Id_SAP == item.Id) {
                            if(!VacationAllowancesForPerson.Contains(vacationAllowanceForPerson)) {
                                VacationAllowancesForPerson.Add(vacationAllowanceForPerson);
                            }
                        }
                    }
                    VacationAllowancesForPerson = new ObservableCollection<VacationAllowanceViewModel>(VacationAllowancesForPerson.OrderBy(i => i.Vacation_Id));

                    Person person = new Person(item.Id, item.Account_Id, item.Name, item.Surname,
                                               item.Patronymic_Name, item.Department_Id, item.Department_Name, item.Virtual_Department_Id,
                                               item.Virtual_Department_Name, item.Sub_Department_Id, item.Role_Name, item.App_Color,
                                               item.Supervisor_Id, item.Position, VacationsForPerson, VacationAllowancesForPerson);
                    Brush brushColor;
                    if(item.App_Color == null) {
                        brushColor = Brushes.Gray;
                    } else {
                        brushColor = (Brush) converter.ConvertFromString(item.App_Color);
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

                App.StateAPI.PersonStates = await App.StateAPI.GetStateVacationsOnApproval(Person.Id_SAP);
                Person.Subordinates.ToList().ForEach(subordinate => subordinate.UpdateStatesCount());
                OnLoginSuccess?.Invoke(Person);
                return Person;
            } catch(Exception ex) {
                Application.Current.Dispatcher.Invoke((Action) delegate {
                    App.SplashScreen.status.Text = ex.Message;
                    App.SplashScreen.status.Foreground = Brushes.Red;
                    //App.SplashScreen.Close();
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
    }
}
