using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

namespace Vacation_Portal.Services.Providers
{
    public class LunchRepository : ILunchRepository
    {
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

        private ObservableCollection<Vacation> _vacations;
        public ObservableCollection<Vacation> Vacations
        {
            get
            {
                return _vacations;
            }
            set
            {
                _vacations = value;
                OnPropertyChanged(nameof(Vacations));
            }
        }
        public Action<Person> OnLoginSuccess { get; set; }
        public Action<Person> PersonUpdated { get; set; }

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

        public async Task<Person> GetPersonAsync(string account)
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                object parametersPerson = new
                {
                    Account = account
                };

                IEnumerable<PersonDTO> fullPersonDTOs = await GetFullPersonDTOs(database, parametersPerson);
                ObservableCollection<Vacation> vacations = await LoadVacations(fullPersonDTOs);
                ObservableCollection<VacationAllowanceViewModel> vacationAlowances = await GetVacationAllowances(fullPersonDTOs);
                ObservableCollection<Person> fullPersons = CreateFullPersons(fullPersonDTOs, vacations, vacationAlowances);

                if(fullPersons == null || fullPersons.Count == 0)
                {
                    return null;
                }

                FullPersons = new ObservableCollection<Person>(fullPersons.OrderBy(i => i.Surname));
                Person = FullPersons.FirstOrDefault(p => p.Id_Account == Environment.UserName);
                Person.Subordinates = new ObservableCollection<Subordinate>(CreateSubordinates(FullPersons));

                App.StateAPI.PersonStates = await App.StateAPI.GetStateVacationsOnApproval(Person.Id_SAP);

                Person.Subordinates.ToList().ForEach(subordinate => subordinate.UpdateStatesCount(false));

                return Person;

            } catch(Exception ex)
            {
                Application.Current.Dispatcher.Invoke((Action) delegate
                {
                    App.SplashScreen.status.Text = ex.Message;
                    App.SplashScreen.status.Foreground = Brushes.Red;
                });
                return null;
            }
        }
        
        public async Task<Person> LoginAsync(string account)
        {
            Person = await GetPersonAsync(account);
            if(Person == null)
            {
                return null;
            }

            OnLoginSuccess?.Invoke(Person);
            return Person;
        }

        private async Task<IEnumerable<PersonDTO>> GetFullPersonDTOs(IDbConnection database, object parametersPerson)
        {
            BrushConverter converter = new System.Windows.Media.BrushConverter();
            App.SplashScreen.status.Text = "В поисках сотрудников";
            IEnumerable<PersonDTO> fullPersonDTOs = await database.QueryAsync<PersonDTO>("usp_Get_Users", parametersPerson, commandType: CommandType.StoredProcedure);

            App.SplashScreen.status.Foreground = Brushes.Black;
            App.SplashScreen.progressBar.Value = 10;
            App.SplashScreen.status.Text = "В поисках отпусков";
            return fullPersonDTOs;
        }
        private ObservableCollection<Person> CreateFullPersons(IEnumerable<PersonDTO> fullPersonDTOs, ObservableCollection<Vacation> vacations, ObservableCollection<VacationAllowanceViewModel> vacationAlowances)
        {
            return new ObservableCollection<Person>(fullPersonDTOs.Select(dto =>
            {
                var userVacations = vacations.Where(v => v.UserId == dto.Id).ToList();
                Vacations = new ObservableCollection<Vacation>(
                    userVacations.Select( v => new Vacation(
                        v.Source,
                        v.Id,
                        v.Name,
                        v.UserId,
                        dto.Name,
                        dto.Surname,
                        v.VacationId,
                        v.Count,
                        v.Color,
                        v.DateStart,
                        v.DateEnd,
                        v.VacationStatusId,
                        v.CreatorId
                    )));
                //Vacations.CollectionChanged += async (sender, e) =>
                //{
                //    if(e.Action == NotifyCollectionChangedAction.Remove)
                //    {
                //        foreach(Vacation vacation in e.OldItems)
                //        {
                //            await App.VacationAPI.DeleteVacationAsync(vacation);
                //        }
                //        Person = await GetPersonAsync(Environment.UserName);
                //    }
                //};

                var userVacationAllowances = vacationAlowances.Where(va => va.User_Id_SAP == dto.Id)
                                                               .OrderBy(va => va.Vacation_Id)
                                                               .Distinct()
                                                               .ToList();

                string brushColor = string.IsNullOrWhiteSpace(dto.App_Color) ? "#FF808080" : dto.App_Color;

                return new Person(dto.Id, dto.Account_Id, dto.Name, dto.Surname, dto.Patronymic_Name,
                                      dto.Department_Id, dto.Department_Name, dto.Virtual_Department_Id,
                                      dto.Virtual_Department_Name, dto.Sub_Department_Id, dto.Role_Name,
                                      brushColor, dto.Supervisor_Id, dto.Position,
                                      Vacations,
                                      new ObservableCollection<VacationAllowanceViewModel>(userVacationAllowances));
            })
            .OrderBy(p => p.Surname)
            .ToList());
        }
        private IEnumerable<Subordinate> CreateSubordinates(ObservableCollection<Person> fullPersons)
        {
            return new ObservableCollection<Subordinate>(fullPersons
                .Where(p => p.Id_Account != Environment.UserName)
                .Select(p => new Subordinate(
                        p.Id_SAP,
                        p.Name,
                        p.Surname,
                        p.Patronymic,
                        p.Position,
                        p.Department_Name,
                        p.Virtual_Department_Name,
                        p.User_Vacations,
                        p.User_Vacation_Allowances))
                );
        }
        
        private async Task<ObservableCollection<Vacation>> LoadVacations(IEnumerable<PersonDTO> fullPersonDTOs)
        {
            var vacationsTasks = fullPersonDTOs.Select(dto => App.VacationAPI.LoadVacationsAsync(dto.Id));
            var IEnumerablevacations = await Task.WhenAll(vacationsTasks);
            return new ObservableCollection<Vacation>(IEnumerablevacations.SelectMany(v => v));
        }
        private async Task<ObservableCollection<VacationAllowanceViewModel>> GetVacationAllowances(IEnumerable<PersonDTO> fullPersonDTOs)
        {
            var vacationAlowanceTasks = fullPersonDTOs.Select(dto => App.VacationAllowanceAPI.GetVacationAllowanceAsync(dto.Id));
            var IEnumerablevacationAlowances = await Task.WhenAll(vacationAlowanceTasks);
            return new ObservableCollection<VacationAllowanceViewModel>(IEnumerablevacationAlowances.SelectMany(v => v));
        }

        public void GetPersonsWithVacationsOnApproval()
        {
            PersonsWithVacationsOnApproval.Clear();
            foreach(Subordinate subordinate in App.API.Person.Subordinates)
            {
                foreach(Vacation vacation in subordinate.Subordinate_Vacations)
                {
                    if(vacation.VacationStatusName == MyEnumExtensions.ToDescriptionString(Statuses.OnApproval))
                    {

                        if(!PersonsWithVacationsOnApproval.Contains(subordinate))
                        {
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
