using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages
{
    public class PersonalVacationPlanningViewModel : ViewModelBase
    {
        #region Props
        private Lazy<Task> _initializeLazy;
        private readonly SampleError _sampleError = new SampleError();
        private CustomCalendar _calendar;
        public CustomCalendar Calendar
        {
            get => _calendar;
            set
            {
                _calendar = value;
                OnPropertyChanged(nameof(Calendar));
            }
        }

        private List<CustomCalendar> _calendars = new List<CustomCalendar>();
        public List<CustomCalendar> Calendars
        {
            get => _calendars;
            set
            {
                _calendars = value;
                OnPropertyChanged(nameof(Calendars));
            }
        }

        private bool _isNextCalendarUnblocked;
        public bool IsNextCalendarUnblocked
        {
            get => _isNextCalendarUnblocked;
            set
            {
                _isNextCalendarUnblocked = value;
                OnPropertyChanged(nameof(IsNextCalendarUnblocked));
            }
        }
        private bool _isNextCalendarPlannedOpen;
        public bool IsNextCalendarPlannedOpen
        {
            get => _isNextCalendarPlannedOpen;
            set
            {
                _isNextCalendarPlannedOpen = value;
                OnPropertyChanged(nameof(IsNextCalendarPlannedOpen));
            }
        }

        private ObservableCollection<Vacation> _vacationsToAproval = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsToAproval
        {
            get => _vacationsToAproval;
            set
            {
                _vacationsToAproval = value;
                OnPropertyChanged(nameof(VacationsToAproval));

            }
        }

        private ObservableCollection<Vacation> _vacationsToAprovalForPerson = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsToAprovalForPerson
        {
            get => _vacationsToAprovalForPerson;
            set
            {
                _vacationsToAprovalForPerson = value;
                OnPropertyChanged(nameof(VacationsToAprovalForPerson));

            }
        }

        private ObservableCollection<Vacation> _vacationsToAprovalFromDataBase = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsToAprovalFromDataBase
        {
            get => _vacationsToAprovalFromDataBase;
            set
            {
                _vacationsToAprovalFromDataBase = value;
                OnPropertyChanged(nameof(VacationsToAprovalFromDataBase));

            }
        }

        private ObservableCollection<VacationAllowanceViewModel> _vacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
        public ObservableCollection<VacationAllowanceViewModel> VacationAllowances
        {
            get => _vacationAllowances;
            set
            {
                _vacationAllowances = value;
                OnPropertyChanged(nameof(VacationAllowances));
            }
        }

        private ObservableCollection<VacationAllowanceViewModel> _vacationAllowancesFromDataBase = new ObservableCollection<VacationAllowanceViewModel>();
        public ObservableCollection<VacationAllowanceViewModel> VacationAllowancesFromDataBase
        {
            get => _vacationAllowancesFromDataBase;
            set
            {
                _vacationAllowancesFromDataBase = value;
                OnPropertyChanged(nameof(VacationAllowancesFromDataBase));
            }
        }

        private List<Subordinate> _subordinates = new List<Subordinate>();
        public List<Subordinate> Subordinates
        {
            get => _subordinates;
            set
            {
                _subordinates = value;
                OnPropertyChanged(nameof(Subordinates));
            }
        }
        private List<Subordinate> _filteredSubordinates = new List<Subordinate>(); 
        public List<Subordinate> FilteredSubordinates
        {
            get => _filteredSubordinates;
            set
            {
                _filteredSubordinates = value;
                OnPropertyChanged(nameof(FilteredSubordinates));
            }
        }

        private ObservableCollection<string> _positionNames = new ObservableCollection<string>();
        public ObservableCollection<string> PositionNames
        {
            get => _positionNames;
            set
            {
                _positionNames = value;
                OnPropertyChanged(nameof(PositionNames));
            }
        }

        private string _selectedPositionName;
        public string SelectedPositionName
        {
            get => _selectedPositionName;
            set
            {
                _selectedPositionName = value;
                OnPropertyChanged(nameof(SelectedPositionName));
                if(SelectedPositionName == null)
                {
                   FilteredSubordinates = Subordinates;
                } else
                {
                   FilteredSubordinates = Subordinates.FindAll(x => x.Position == SelectedPositionName);
                }
                
            }
        }

        private Subordinate _selectedPerson;
        public Subordinate SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
                VacationAllowances.Clear();
                VacationsToAproval.Clear();
                if(SelectedPerson != null)
                {
                   UpdateDataForSubordinate();
                } else
                {
                    UpdateDataForPerson();
                }
            }
        }

        private ObservableCollection<VacationAllowanceViewModel> _vacationAllowancesForPerson = new ObservableCollection<VacationAllowanceViewModel>();
        public ObservableCollection<VacationAllowanceViewModel> VacationAllowancesForPerson
        {
            get => _vacationAllowancesForPerson;
            set
            {
                _vacationAllowancesForPerson = value;
                OnPropertyChanged(nameof(VacationAllowancesForPerson));
            }
        }

        private ObservableCollection<VacationAllowanceViewModel> _vacationAllowancesForSubordinate = new ObservableCollection<VacationAllowanceViewModel>();
        public ObservableCollection<VacationAllowanceViewModel> VacationAllowancesForSubordinate
        {
            get => _vacationAllowancesForSubordinate;
            set
            {
                _vacationAllowancesForSubordinate = value;
                OnPropertyChanged(nameof(VacationAllowancesForSubordinate));
            }
        }

        private ObservableCollection<VacationViewModel> _vacationsForSubordinate = new ObservableCollection<VacationViewModel>();
        public ObservableCollection<VacationViewModel> VacationsForSubordinate
        {
            get => _vacationsForSubordinate;
            set
            {
                _vacationsForSubordinate = value;
                OnPropertyChanged(nameof(VacationsForSubordinate));
            }
        }

        private ObservableCollection<VacationViewModel> _vacationsForPerson = new ObservableCollection<VacationViewModel>();
        public ObservableCollection<VacationViewModel> VacationsForPerson
        {
            get => _vacationsForPerson;
            set
            {
                _vacationsForPerson = value;
                OnPropertyChanged(nameof(VacationsForPerson));
            }
        }

        private DateTime _currentDate = DateTime.Now;
        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }

        private int _currentYear = DateTime.Now.Year;
        public int CurrentYear
        {
            get => _currentYear;
            set
            {
                _currentYear = value;
                OnPropertyChanged(nameof(CurrentYear));
            }
        }

        private string _plannedVacationString;
        public string PlannedVacationString
        {
            get => _plannedVacationString;
            set
            {
                _plannedVacationString = value;
                OnPropertyChanged(nameof(PlannedVacationString));
                IsGapVisible = PlannedVacationString != "";
            }
        }

        private bool _isLoadingCalendarPage;
        public bool IsLoadingCalendarPage
        {
            get => _isLoadingCalendarPage;
            set
            {
                _isLoadingCalendarPage = value;
                OnPropertyChanged(nameof(IsLoadingCalendarPage));
            }
        }

        #region Person props
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
        private string _personName;
        public string PersonName {
            get => _personName;
            set
            {
                _personName = value;
                OnPropertyChanged(nameof(PersonName));
            }
        }

        #endregion Person props

        #region Button Interaction

        private bool _isGapVisible;
        public bool IsGapVisible
        {
            get => _isGapVisible;
            set
            {
                _isGapVisible = value;
                OnPropertyChanged(nameof(IsGapVisible));

            }
        }

        private bool _isSaveComplete;
        public bool IsSaveComplete
        {
            get => _isSaveComplete;
            set
            {
                _isSaveComplete = value;
                OnPropertyChanged(nameof(IsSaveComplete));

            }
        }

        private bool _isSaving;
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                _isSaving = value;
                OnPropertyChanged(nameof(IsSaving));

            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));

            }
        }

        private bool _isNextYearEnabled = true;
        public bool IsNextYearEnabled
        {
            get => _isNextYearEnabled;
            set
            {
                _isNextYearEnabled = value;
                OnPropertyChanged(nameof(IsNextYearEnabled));
            }
        }
        private bool _isPreviousYearEnabled = false;
        public bool IsPreviousYearEnabled
        {
            get => _isPreviousYearEnabled;
            set
            {
                _isPreviousYearEnabled = value;
                OnPropertyChanged(nameof(IsPreviousYearEnabled));
            }
        }
        #endregion Button Interaction

        #region Vacation Allowance props

        private VacationAllowanceViewModel _selectedItemAllowance;
        public VacationAllowanceViewModel SelectedItemAllowance
        {
            get => _selectedItemAllowance;
            set
            {
                _ = SetProperty(ref _selectedItemAllowance, value);
                OnPropertyChanged(nameof(SelectedItemAllowance));
                if(SelectedItemAllowance != null)
                {
                    Calendar.CalendarClickable = SelectedItemAllowance.Vacation_Days_Quantity > 0;
                }
                Calendar.ClearVacationData();
            }
        }

        private int _selectedIndexAllowance;
        public int SelectedIndexAllowance
        {
            get => _selectedIndexAllowance;
            set
            {
                _ = SetProperty(ref _selectedIndexAllowance, value);
                OnPropertyChanged(nameof(SelectedIndexAllowance));
            }
        }
        #endregion Vacation Allowance props

        #region Planned Vacation props
        private Vacation _plannedItem;
        public Vacation PlannedItem
        {
            get => _plannedItem;
            set
            {
                SetProperty(ref _plannedItem, value);
                OnPropertyChanged(nameof(PlannedItem));
            }
        }

        private int _plannedIndex;
        public int PlannedIndex
        {
            get => _plannedIndex;
            set
            {
                SetProperty(ref _plannedIndex, value);
                OnPropertyChanged(nameof(PlannedIndex));
            }
        }
        private Vacation _plannedPersonItem;
        public Vacation PlannedPersonItem
        {
            get => _plannedPersonItem;
            set
            {
                SetProperty(ref _plannedPersonItem, value);
                OnPropertyChanged(nameof(PlannedPersonItem));
            }
        }

        private int _plannedPersonIndex;
        public int PlannedPersonIndex
        {
            get => _plannedPersonIndex;
            set
            {
                SetProperty(ref _plannedPersonIndex, value);
                OnPropertyChanged(nameof(PlannedPersonIndex));
            }
        }
        #endregion Planned Vacation props

        #region Lerning props
        private double _saveProgress;
        public double SaveProgress
        {
            get => _saveProgress;
            set
            {
                _saveProgress = value;
                OnPropertyChanged(nameof(SaveProgress));

            }
        }

        private double _learningProgress;
        public double LearningProgress
        {
            get => _learningProgress;
            set
            {
                _learningProgress = value;
                OnPropertyChanged(nameof(LearningProgress));

            }
        }

        #region QueuesLern
        public SnackbarMessageQueue MessageQueueVacation { get; set; }
        public SnackbarMessageQueue MessageQueueCalendar { get; set; }
        public SnackbarMessageQueue MessageQueueSelectedGap { get; set; }
        public SnackbarMessageQueue MessageQueuePLanedVacations { get; set; }
        #endregion QueuesLern

        #region BorderLern
        private Brush _borderColorVacation;
        public Brush BorderColorVacation
        {
            get => _borderColorVacation;
            set
            {
                SetProperty(ref _borderColorVacation, value);
                OnPropertyChanged(nameof(BorderColorVacation));
            }
        }

        private Brush _borderColorCalendar;
        public Brush BorderColorCalendar
        {
            get => _borderColorCalendar;
            set
            {
                SetProperty(ref _borderColorCalendar, value);
                OnPropertyChanged(nameof(BorderColorCalendar));
            }
        }

        private Brush _borderColorSelectedGap;
        public Brush BorderColorSelectedGap
        {
            get => _borderColorSelectedGap;
            set
            {
                SetProperty(ref _borderColorSelectedGap, value);
                OnPropertyChanged(nameof(BorderColorSelectedGap));
            }
        }

        private Brush _borderColorPLanedVacations;
        public Brush BorderColorPLanedVacations
        {
            get => _borderColorPLanedVacations;
            set
            {
                SetProperty(ref _borderColorPLanedVacations, value);
                OnPropertyChanged(nameof(BorderColorPLanedVacations));
            }
        }
        #endregion BorderLern

        #endregion Lerning props

        #endregion Props

        #region Commands
        public ICommand LoadModel { get; }
        public ICommand SaveDataModel { get; }
        public ICommand StartLearning { get; }
        public ICommand AddToApprovalList { get; }
        public ICommand CancelVacation { get; }
        public ICommand CheckVacations { get; }
        public AnotherCommandImplementation MovePrevYearCommand { get; }
        public AnotherCommandImplementation MoveNextYearCommand { get; }
        #endregion Commands

        #region Constructor
        public PersonalVacationPlanningViewModel()
        {
            MessageQueueVacation = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
            MessageQueueCalendar = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
            MessageQueueSelectedGap = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
            MessageQueuePLanedVacations = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));

            LoadModel = new LoadModelCommand(this);
            CheckVacations = new CheckVacationsCommand(this);
            SaveDataModel = new SaveDataModelCommand(this);
            StartLearning = new StartLearningCommand(this);
            AddToApprovalList = new AddToApprovalListCommand(this);
            CancelVacation = new CancelVacationCommand(this);

            Person = App.API.Person;
            Person.FullName = App.API.Person.ToString();
            PersonName = App.API.Person.ToString();
            MovePrevYearCommand = new AnotherCommandImplementation(
               _ =>
               {
                   IsLoadingCalendarPage = true;
                   IsPreviousYearEnabled = false;
                   IsNextYearEnabled = true;
                   Calendar = Calendars[0];
                   CurrentYear = Calendars[0].CurrentYear;
                   if(SelectedPerson != null)
                   {
                       UpdateDataForSubordinate();
                   } else
                   {
                       UpdateDataForPerson();
                   }
                   

                   //await Task.Run(() => Calendar.UpdateColor());
                   IsLoadingCalendarPage = false;
               });

            MoveNextYearCommand = new AnotherCommandImplementation(
               _ =>
               {
                   IsLoadingCalendarPage = true;
                   IsPreviousYearEnabled = true;
                   IsNextYearEnabled = false;
                   Calendar = Calendars[1];
                   CurrentYear = Calendars[1].CurrentYear;

                   if(SelectedPerson != null)
                   {
                       UpdateDataForSubordinate();
                   } else
                   {
                       UpdateDataForPerson();
                   }

                   //await Task.Run(() => Calendar.UpdateColor());
                   IsLoadingCalendarPage = false;
               });

            _initializeLazy = new Lazy<Task>(async () => await Initialize());
            LoadModel.Execute(new object());
        }
        #endregion Constructor

        #region Task Lazy
        private async Task Initialize()
        {
            await UpdateData();
            UpdateDataForPerson();
            UpdateDataForSubordinate();
            Calendar = Calendars[0];
        }

        private void UpdateDataForSubordinate() {
            if(SelectedPerson != null) {
                foreach(Subordinate item in Subordinates)
                {
                    if(item.Id_SAP == SelectedPerson.Id_SAP)
                    {
                        VacationAllowancesForSubordinate = item.Subordinate_Vacation_Allowances;
                        VacationsForSubordinate = item.Subordinate_Vacations;
                    }
                }
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                                                      VacationAllowancesForSubordinate.Where(f => f.Vacation_Year == CurrentYear));
            }
            
            foreach(VacationViewModel vacationViewModel in VacationsForSubordinate)
            {
                Vacation vacation = new Vacation(vacationViewModel.Name, vacationViewModel.User_Id_SAP, vacationViewModel.Vacation_Id, vacationViewModel.Count, vacationViewModel.Color, vacationViewModel.DateStart, vacationViewModel.DateEnd, vacationViewModel.Status);
                if(!VacationsToAproval.Contains(vacation))
                {
                    VacationsToAproval.Add(vacation);
                }
            }
            VacationsToAproval = new ObservableCollection<Vacation>(VacationsToAproval.Where(f => f.Date_Start.Year == CurrentYear));
            Calendar.UpdateColor();
        }

        private void UpdateDataForPerson()
        {
            VacationAllowancesForPerson = new ObservableCollection<VacationAllowanceViewModel>(
                                                    VacationAllowancesFromDataBase.Where(f => f.Vacation_Year == CurrentYear));
            VacationsToAprovalForPerson = new ObservableCollection<Vacation>(
                                                    VacationsToAprovalFromDataBase.Where(f => f.Date_Start.Year == CurrentYear));
        }
        public async Task UpdateData()
        {
            IsNextCalendarUnblocked = App.API.IsCalendarUnblocked;
            IsNextCalendarPlannedOpen = App.API.IsCalendarPlannedOpen;

            Subordinates.Clear();
            PositionNames.Clear();

            foreach(Subordinate subordinate in App.API.Person.Subordinates)
            {
                Subordinates.Add(subordinate);
                if(!PositionNames.Contains(subordinate.Position))
                {
                    PositionNames.Add(subordinate.Position);
                }
            }
            FilteredSubordinates = Subordinates;

           await prepareCalendar();

            List<string> VacationNames = new List<string> { "Основной", "Вредность", "Ненормированность", "Стаж"};
            foreach(string vacationName in VacationNames)
            {
                VacationAllowanceViewModel defaultAllowances = new VacationAllowanceViewModel(0, vacationName, 0, 0, 0, null);
                VacationAllowances.Add(defaultAllowances);
            }
        }
        public async Task Load()
        {
            try
            {
                await _initializeLazy.Value;
            } catch(Exception)
            {
                _initializeLazy = new Lazy<Task>(async () => await Initialize());
                throw;
            }
        }
        #endregion Task Lazy

        #region OnStartup

        public async Task prepareCalendar() {
            int interactionCount = 1;
            if(IsNextCalendarUnblocked)
            {
                interactionCount = 2;
            }
            for(int k = 0; k < interactionCount; k++)
            {
                foreach(VacationViewModel item in FetchVacations(CurrentDate.Year + k))
                {
                    Vacation vacation = new Vacation(item.Name, item.User_Id_SAP, item.Vacation_Id, item.Count, item.Color, item.DateStart, item.DateEnd, item.Status);
                    if(!VacationsToAprovalFromDataBase.Contains(vacation))
                    {
                        VacationsToAprovalFromDataBase.Add(vacation);
                        VacationsForPerson.Add(item);
                    }
                }

                foreach(VacationAllowanceViewModel item in FetchVacationAllowances(CurrentDate.Year + k))
                {
                    if(item.Vacation_Year == CurrentDate.Year + k)
                    {
                        VacationAllowancesFromDataBase.Add(item);
                        for(int i = 0; i < VacationAllowancesFromDataBase.Count; i++)
                        {
                            if(VacationAllowancesFromDataBase[i].Vacation_Days_Quantity > 0)
                            {
                                SelectedIndexAllowance = i;
                                break;
                            }
                        }
                    }
                }
                await CreateCalendar(k);
            }
        }
        public async Task CreateCalendar(int num)
        {
            //ObservableCollection<Vacation> vacationsToAproval = new ObservableCollection<Vacation>();
            //vacationsToAproval = new ObservableCollection<Vacation>(
            //                                            VacationsToAprovalFromDataBase.Where(f => f.Date_Start.Year == CurrentDate.Year + num));
            if(Calendars.Count < 2)
            {
                Calendar = new CustomCalendar(CurrentDate.Year + num, this);
                await Task.Run(async () => await Calendar.Render());
                Calendars.Add(Calendar);
            }
        }

        private void OnHolidaysChanged(List<HolidayViewModel> obj)
        {
            //Holidays = obj;
            //Task.Run(async () => await Calendar.Render(CurrentDate.Year));
        }

        #endregion OnStartup

        #region Utils
        public IEnumerable<VacationViewModel> FetchVacations(int year)
        {
            App.Current.Dispatcher.Invoke((Action) delegate
            {
                App.SplashScreen.status.Text = "Загружаю ваши отпуска...";
                App.SplashScreen.status.Foreground = Brushes.Black;
            });
            IEnumerable<VacationViewModel> vacations = new ObservableCollection<VacationViewModel>(
                                            Person.User_Vacations.Where(f => f.DateStart.Year == year));

            foreach(VacationViewModel item in vacations)
            {
                yield return item;
            }
        }

        public IEnumerable<VacationAllowanceViewModel> FetchVacationAllowances(int year)
        {
            IEnumerable<VacationAllowanceViewModel> vacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                                            Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == year));
            foreach(VacationAllowanceViewModel item in vacationAllowances)
            {
                yield return item;
            }
        }
        public void ShowAlert(string alert)
        {
            _sampleError.ErrorName.Text = alert;
            Task<object> result = DialogHost.Show(_sampleError, "RootDialog", ExtendedClosingEventHandler);
        }

        public async Task UpdateVacationAllowance(int userIdSAP, int vacation_Id, int year, int count)
        {
            await App.API.UpdateVacationAllowanceAsync(userIdSAP, vacation_Id, year, count);
        }
        public async Task DeleteVacation(Vacation vacation)
        {
            await App.API.DeleteVacationAsync(vacation);
        }
        public void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if(eventArgs.Parameter is bool parameter &&
                parameter == false)
            {
                return;
            }

            eventArgs.Cancel();

            Task.Delay(TimeSpan.FromSeconds(0.3))
                .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                    TaskScheduler.FromCurrentSynchronizationContext());
            //Task.Delay(TimeSpan.FromSeconds(0.1))
            //    .ContinueWith((t, _) => eventArgs.Session.UpdateContent(new SampleError()), null,
            //        TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion Utils
    }
}
