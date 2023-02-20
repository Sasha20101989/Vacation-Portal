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
        private ObservableCollection<string> _filteredPositionNames = new ObservableCollection<string>();
        public ObservableCollection<string> FilteredPositionNames
        {
            get => _filteredPositionNames;
            set
            {
                _filteredPositionNames = value;
                OnPropertyChanged(nameof(FilteredPositionNames));
            }
        }
        private ObservableCollection<string> _departments = new ObservableCollection<string>();
        public ObservableCollection<string> Departments
        {
            get => _departments;
            set
            {
                _departments = value;
                OnPropertyChanged(nameof(Departments));
            }
        }

        private ObservableCollection<string> _virtualDepartments = new ObservableCollection<string>();
        public ObservableCollection<string> VirtualDepartments
        {
            get => _virtualDepartments;
            set
            {
                _virtualDepartments = value;
                OnPropertyChanged(nameof(VirtualDepartments));
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

        private string _selectedDepartmentName;
        public string SelectedDepartmentName
        {
            get => _selectedDepartmentName;
            set
            {
                _selectedDepartmentName = value;
                OnPropertyChanged(nameof(SelectedDepartmentName));

                SelectedPositionName = null;
                SelectedSubordinate = null;

                if(SelectedDepartmentName == null)
                {
                    FilteredSubordinates = App.API.Person.Subordinates;
                    FilteredPositionNames = PositionNames;
                } else
                {
                    FilteredSubordinates = App.API.Person.Subordinates.FindAll(x => x.Department_Name == SelectedDepartmentName);
                    UpdatePositionNames();
                }
            }
        }

        private string _selectedVirtualDepartmentName;
        public string SelectedVirtualDepartmentName
        {
            get => _selectedVirtualDepartmentName;
            set
            {
                _selectedVirtualDepartmentName = value;
                OnPropertyChanged(nameof(SelectedVirtualDepartmentName));

                SelectedPositionName = null;
                SelectedSubordinate = null;

                if(SelectedVirtualDepartmentName == null)
                {
                    FilteredSubordinates = App.API.Person.Subordinates;
                    FilteredPositionNames = PositionNames;
                } else
                {
                    FilteredSubordinates = App.API.Person.Subordinates.FindAll(x => x.Virtual_Department_Name == SelectedVirtualDepartmentName);
                    UpdatePositionNames();
                }
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
                    FilteredSubordinates = App.API.Person.Subordinates;
                } else
                {
                    if(SelectedDepartmentName != null)
                    {
                        FilteredSubordinates = App.API.Person.Subordinates.FindAll(x => x.Position == SelectedPositionName && x.Department_Name == SelectedDepartmentName);
                    } else if(SelectedVirtualDepartmentName != null)
                    {
                        FilteredSubordinates = App.API.Person.Subordinates.FindAll(x => x.Position == SelectedPositionName && x.Virtual_Department_Name == SelectedVirtualDepartmentName);

                    } else
                    {
                        FilteredSubordinates = App.API.Person.Subordinates.FindAll(x => x.Position == SelectedPositionName);
                    }
                }
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

        private string _personName;
        public string PersonName
        {
            get => _personName;
            set
            {
                _personName = value;
                OnPropertyChanged(nameof(PersonName));
            }
        }

        private Subordinate _selectedSubordinate;
        public Subordinate SelectedSubordinate
        {
            get => _selectedSubordinate;
            set
            {
                _selectedSubordinate = value;
                OnPropertyChanged(nameof(SelectedSubordinate));
                VacationAllowancesForSubordinate = Clone(DefaultVacationAllowances);
                VacationsToAprovalForSubordinate.Clear();
                if(SelectedSubordinate != null)
                {
                    UpdateDataForSubordinate();
                }
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

        private ObservableCollection<VacationAllowanceViewModel> _defaultVacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
        public ObservableCollection<VacationAllowanceViewModel> DefaultVacationAllowances
        {
            get => _defaultVacationAllowances;
            set
            {
                _defaultVacationAllowances = value;
                OnPropertyChanged(nameof(DefaultVacationAllowances));
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
                if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate) || App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.HR_GOD))
                {
                    Calendar.ClearVacationData(VacationsToAprovalForSubordinate);
                } else if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Personal))
                {
                    Calendar.ClearVacationData(VacationsToAprovalForPerson);
                }
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
        private ObservableCollection<Vacation> _vacationsToAprovalForSubordinate = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsToAprovalForSubordinate
        {
            get => _vacationsToAprovalForSubordinate;
            set
            {
                _vacationsToAprovalForSubordinate = value;
                OnPropertyChanged(nameof(VacationsToAprovalForSubordinate));

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
        public ICommand CompensateVacation { get; }
        public ICommand ShiftVacations { get; }
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
            CompensateVacation = new CompensateVacationCommand(this);
            ShiftVacations = new ShiftVacationsCommand(this);

            PersonName = App.API.Person.ToString();
            MovePrevYearCommand = new AnotherCommandImplementation(
               _ =>
               {
                   if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate) || App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.HR_GOD))
                   {
                       if(SelectedSubordinate != null)
                       {
                           IsLoadingCalendarPage = true;
                           IsPreviousYearEnabled = false;
                           IsNextYearEnabled = true;
                           Calendar = Calendars[0];
                           CurrentYear = Calendars[0].CurrentYear;
                           UpdateDataForSubordinate();
                           IsLoadingCalendarPage = false;
                       } else
                       {
                           ShowAlert("Сначала выберите сотрудника");
                       }
                   } else if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Personal))
                   {
                       IsLoadingCalendarPage = true;
                       IsPreviousYearEnabled = false;
                       IsNextYearEnabled = true;
                       Calendar = Calendars[0];
                       CurrentYear = Calendars[0].CurrentYear;
                       UpdateDataForPerson();
                       IsLoadingCalendarPage = false;
                   }
               });

            MoveNextYearCommand = new AnotherCommandImplementation(
               _ =>
               {
                   if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate) ||
                      App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.HR_GOD) ||
                      App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.ACCOUNTING))
                   {
                       if(SelectedSubordinate != null)
                       {
                           IsLoadingCalendarPage = true;
                           IsPreviousYearEnabled = true;
                           IsNextYearEnabled = false;
                           Calendar = Calendars[1];
                           CurrentYear = Calendars[1].CurrentYear;
                           UpdateDataForSubordinate();
                           IsLoadingCalendarPage = false;
                       } else
                       {
                           ShowAlert("Сначала выберете сотрудника");
                       }
                   } else if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Personal))
                   {
                       IsLoadingCalendarPage = true;
                       IsPreviousYearEnabled = true;
                       IsNextYearEnabled = false;
                       Calendar = Calendars[1];
                       CurrentYear = Calendars[1].CurrentYear;
                       UpdateDataForPerson();
                       IsLoadingCalendarPage = false;
                   }
               });

            _initializeLazy = new Lazy<Task>(async () => await Initialize());
            LoadModel.Execute(new object());
        }
        #endregion Constructor

        #region Task Lazy
        private async Task Initialize()
        {
            IsNextCalendarUnblocked = App.API.CheckDateUnblockedCalendarAsync();
            IsNextCalendarPlannedOpen = App.API.CheckNextCalendarPlanningUnlock();
            if(App.API.Person.Is_HR_GOD)
            {
                PrepareDepartments();
            } else if(App.API.Person.Is_Accounting)
            {
                PrepareVirtualDepartments();
            }

            PreparePositionsAndSubordinateNames();
            await prepareCalendar();
            UpdateDataForPerson();
            Calendar = Calendars[0];
        }

        public void UpdateDataForSubordinate()
        {
            VacationAllowancesForSubordinate = new ObservableCollection<VacationAllowanceViewModel>(SelectedSubordinate.Subordinate_Vacation_Allowances.Where(f => f.Vacation_Year == CurrentYear));
            if(VacationAllowancesForSubordinate.Count == 0)
            {
                VacationAllowancesForSubordinate = Clone(DefaultVacationAllowances);
            }

            VacationsToAprovalForSubordinate = new ObservableCollection<Vacation>(SelectedSubordinate.Subordinate_Vacations.Where(f => f.Date_Start.Year == CurrentYear));
            Calendar.UpdateColor(VacationsToAprovalForSubordinate);
        }

        public void UpdateDataForPerson()
        {
            VacationAllowancesForPerson = new ObservableCollection<VacationAllowanceViewModel>(
                                                    App.API.Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == CurrentYear));
            if(VacationAllowancesForPerson.Count == 0)
            {
                VacationAllowancesForPerson = Clone(DefaultVacationAllowances);
            }
            VacationsToAprovalForPerson = new ObservableCollection<Vacation>(
                                                    App.API.Person.User_Vacations.Where(f => f.Date_Start.Year == CurrentYear));
        }
        private void UpdatePositionNames()
        {
            FilteredPositionNames.Clear();
            foreach(Subordinate subordinate in FilteredSubordinates)
            {
                if(!FilteredPositionNames.Contains(subordinate.Position))
                {
                    FilteredPositionNames.Add(subordinate.Position);
                }
            }
            FilteredPositionNames = new ObservableCollection<string>(FilteredPositionNames.OrderBy(i => i));
        }
        public void PrepareVirtualDepartments()
        {
            VirtualDepartments.Clear();
            foreach(Subordinate subordinate in App.API.Person.Subordinates)
            {
                if(!VirtualDepartments.Contains(subordinate.Virtual_Department_Name))
                {
                    VirtualDepartments.Add(subordinate.Virtual_Department_Name);
                }
            }
            VirtualDepartments = new ObservableCollection<string>(VirtualDepartments.OrderBy(i => i));
        }
        public void PrepareDepartments()
        {
            Departments.Clear();
            foreach(Subordinate subordinate in App.API.Person.Subordinates)
            {
                if(!Departments.Contains(subordinate.Department_Name))
                {
                    Departments.Add(subordinate.Department_Name);
                }
            }
            Departments = new ObservableCollection<string>(Departments.OrderBy(i => i));
        }
        public void PreparePositionsAndSubordinateNames()
        {
            PositionNames.Clear();

            foreach(Subordinate subordinate in App.API.Person.Subordinates)
            {
                if(!PositionNames.Contains(subordinate.Position))
                {
                    PositionNames.Add(subordinate.Position);
                }
            }
            PositionNames = new ObservableCollection<string>(PositionNames.OrderBy(i => i));
            FilteredPositionNames = Clone(PositionNames);
            FilteredSubordinates = App.API.Person.Subordinates;

            List<string> VacationNames = new List<string> { "Основной", "Вредность", "Ненормированность", "Стаж" };
            foreach(string vacationName in VacationNames)
            {
                VacationAllowanceViewModel defaultAllowance = new VacationAllowanceViewModel(0, vacationName, 0, 0, 0, null);
                DefaultVacationAllowances.Add(defaultAllowance);
            }
            VacationAllowancesForSubordinate = Clone(DefaultVacationAllowances);
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

        public async Task prepareCalendar()
        {
            int interactionCount = 1;
            if(IsNextCalendarUnblocked)
            {
                interactionCount = 2;
            }
            for(int k = 0; k < interactionCount; k++)
            {
                await CreateCalendar(k);
            }
        }
        public async Task CreateCalendar(int num)
        {
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
        static ObservableCollection<T> Clone<T>(ObservableCollection<T> listToClone) where T : ICloneable
        {
            return new ObservableCollection<T>(listToClone.Select(item => (T) item.Clone()));
        }
        #endregion Utils
    }
}
