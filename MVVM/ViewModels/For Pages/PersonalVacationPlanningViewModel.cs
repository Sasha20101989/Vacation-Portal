﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages {
    public class PersonalVacationPlanningViewModel : ViewModelBase {

        #region Props
        private Lazy<Task> _initializeLazy;
        private readonly SampleError _sampleError = new SampleError();
        private CustomCalendar _calendar;
        public CustomCalendar Calendar {
            get => _calendar;
            set {
                _calendar = value;
                OnPropertyChanged(nameof(Calendar));
            }
        }
        private bool _isFlipped;
        public bool IsFlipped
        {
            get
            {
                return _isFlipped;
            }
            set
            {
                _isFlipped = value;
                OnPropertyChanged(nameof(IsFlipped));
            }
        }
        private List<CustomCalendar> _calendars = new List<CustomCalendar>();
        public List<CustomCalendar> Calendars {
            get => _calendars;
            set {
                _calendars = value;
                OnPropertyChanged(nameof(Calendars));
            }
        }

        private bool _isNextCalendarUnblocked;
        public bool IsNextCalendarUnblocked {
            get => _isNextCalendarUnblocked;
            set {
                _isNextCalendarUnblocked = value;
                OnPropertyChanged(nameof(IsNextCalendarUnblocked));
            }
        }
        private bool _isNextCalendarPlannedOpen;
        public bool IsNextCalendarPlannedOpen {
            get => _isNextCalendarPlannedOpen;
            set {
                _isNextCalendarPlannedOpen = value;
                OnPropertyChanged(nameof(IsNextCalendarPlannedOpen));
            }
        }

        private ObservableCollection<Subordinate> _filteredSubordinates = new ObservableCollection<Subordinate>();
        public ObservableCollection<Subordinate> FilteredSubordinates {
            get => _filteredSubordinates;
            set {
                _filteredSubordinates = value;
                OnPropertyChanged(nameof(FilteredSubordinates));
            }
        }
        private ObservableCollection<string> _filteredPositionNames = new ObservableCollection<string>();
        public ObservableCollection<string> FilteredPositionNames {
            get => _filteredPositionNames;
            set {
                _filteredPositionNames = value;
                OnPropertyChanged(nameof(FilteredPositionNames));
            }
        }
        private ObservableCollection<string> _departments = new ObservableCollection<string>();
        public ObservableCollection<string> Departments {
            get => _departments;
            set {
                _departments = value;
                OnPropertyChanged(nameof(Departments));
            }
        }

        private ObservableCollection<string> _virtualDepartments = new ObservableCollection<string>();
        public ObservableCollection<string> VirtualDepartments {
            get => _virtualDepartments;
            set {
                _virtualDepartments = value;
                OnPropertyChanged(nameof(VirtualDepartments));
            }
        }

        private ObservableCollection<string> _positionNames = new ObservableCollection<string>();
        public ObservableCollection<string> PositionNames {
            get => _positionNames;
            set {
                _positionNames = value;
                OnPropertyChanged(nameof(PositionNames));
            }
        }

        private string _selectedDepartmentName;
        public string SelectedDepartmentName {
            get => _selectedDepartmentName;
            set {
                _selectedDepartmentName = value;
                OnPropertyChanged(nameof(SelectedDepartmentName));

                SelectedPositionName = null;
                SelectedSubordinate = null;

                if(SelectedDepartmentName == null) {
                    FilteredSubordinates = App.API.Person.Subordinates;
                    FilteredPositionNames = PositionNames;
                } else {
                    FilteredSubordinates = new ObservableCollection<Subordinate>( App.API.Person.Subordinates.Where(x => x.Department_Name == SelectedDepartmentName));
                    UpdatePositionNames();
                }
            }
        }

        private string _selectedVirtualDepartmentName;
        public string SelectedVirtualDepartmentName {
            get => _selectedVirtualDepartmentName;
            set {
                _selectedVirtualDepartmentName = value;
                OnPropertyChanged(nameof(SelectedVirtualDepartmentName));

                SelectedPositionName = null;
                SelectedSubordinate = null;

                if(SelectedVirtualDepartmentName == null) {
                    FilteredSubordinates = App.API.Person.Subordinates;
                    FilteredPositionNames = PositionNames;
                } else {
                    FilteredSubordinates = new ObservableCollection<Subordinate>(App.API.Person.Subordinates.Where(x => x.Virtual_Department_Name == SelectedVirtualDepartmentName));
                    UpdatePositionNames();
                }
            }
        }
        public object SelectedVacation { get; set; }
        private string _selectedPositionName;
        public string SelectedPositionName {
            get => _selectedPositionName;
            set {
                _selectedPositionName = value;
                OnPropertyChanged(nameof(SelectedPositionName));

                if(SelectedPositionName == null) {
                    FilteredSubordinates = App.API.Person.Subordinates;
                } else {
                    if(SelectedDepartmentName != null) {
                        FilteredSubordinates = new ObservableCollection<Subordinate>(App.API.Person.Subordinates.Where(x => x.Position == SelectedPositionName && x.Department_Name == SelectedDepartmentName));

                    } else if(SelectedVirtualDepartmentName != null) {
                        FilteredSubordinates = new ObservableCollection<Subordinate>(App.API.Person.Subordinates.Where(x => x.Position == SelectedPositionName && x.Virtual_Department_Name == SelectedVirtualDepartmentName));

                    } else {
                        FilteredSubordinates = new ObservableCollection<Subordinate>(App.API.Person.Subordinates.Where(x => x.Position == SelectedPositionName));
                    }
                }
            }
        }

        private DateTime _currentDate = DateTime.Now;
        public DateTime CurrentDate {
            get => _currentDate;
            set {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }

        private int _currentYear = DateTime.Now.Year;
        public int CurrentYear {
            get => _currentYear;
            set {
                _currentYear = value;
                OnPropertyChanged(nameof(CurrentYear));
            }
        }

        private string _plannedVacationString;
        public string PlannedVacationString {
            get => _plannedVacationString;
            set {
                _plannedVacationString = value;
                OnPropertyChanged(nameof(PlannedVacationString));
                IsGapVisible = PlannedVacationString != "";
            }
        }

        private bool _isLoadingCalendarPage;
        public bool IsLoadingCalendarPage {
            get => _isLoadingCalendarPage;
            set {
                _isLoadingCalendarPage = value;
                OnPropertyChanged(nameof(IsLoadingCalendarPage));
            }
        }

        #region Person props

        private string _personName;
        public string PersonName {
            get => _personName;
            set {
                _personName = value;
                OnPropertyChanged(nameof(PersonName));
            }
        }

        private Subordinate _selectedSubordinate;
        public Subordinate SelectedSubordinate {
            get => _selectedSubordinate;
            set {
                _selectedSubordinate = value;
                OnPropertyChanged(nameof(SelectedSubordinate));
                VacationAllowancesForSubordinate = Clone(DefaultVacationAllowances);
                VacationsToAprovalForSubordinate.Clear();
                if(SelectedSubordinate != null) {
                    Task.Run(async () => {
                        await UpdateDataForSubordinateAsync();
                    });
                }
            }
        }
        #endregion Person props

        #region Button Interaction

        private bool _isGapVisible;
        public bool IsGapVisible {
            get => _isGapVisible;
            set {
                _isGapVisible = value;
                OnPropertyChanged(nameof(IsGapVisible));

            }
        }

        private bool _isSaveComplete;
        public bool IsSaveComplete {
            get => _isSaveComplete;
            set {
                _isSaveComplete = value;
                OnPropertyChanged(nameof(IsSaveComplete));

            }
        }

        private bool _isSaving;
        public bool IsSaving {
            get => _isSaving;
            set {
                _isSaving = value;
                OnPropertyChanged(nameof(IsSaving));

            }
        }

        private bool _isEnabled;
        public bool IsEnabled {
            get => _isEnabled;
            set {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));

            }
        }

        private bool _isNextYearEnabled = true;
        public bool IsNextYearEnabled {
            get => _isNextYearEnabled;
            set {
                _isNextYearEnabled = value;
                OnPropertyChanged(nameof(IsNextYearEnabled));
            }
        }
        private bool _isPreviousYearEnabled = false;
        public bool IsPreviousYearEnabled {
            get => _isPreviousYearEnabled;
            set {
                _isPreviousYearEnabled = value;
                OnPropertyChanged(nameof(IsPreviousYearEnabled));
            }
        }
        #endregion Button Interaction

        #region Vacation Allowance props

        private ObservableCollection<VacationAllowanceViewModel> _defaultVacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
        public ObservableCollection<VacationAllowanceViewModel> DefaultVacationAllowances {
            get => _defaultVacationAllowances;
            set {
                _defaultVacationAllowances = value;
                OnPropertyChanged(nameof(DefaultVacationAllowances));
            }
        }

        private ObservableCollection<VacationAllowanceViewModel> _vacationAllowancesForPerson = new ObservableCollection<VacationAllowanceViewModel>();
        public ObservableCollection<VacationAllowanceViewModel> VacationAllowancesForPerson {
            get => _vacationAllowancesForPerson;
            set {
                _vacationAllowancesForPerson = value;
                OnPropertyChanged(nameof(VacationAllowancesForPerson));
            }
        }

        private ObservableCollection<VacationAllowanceViewModel> _vacationAllowancesForSubordinate = new ObservableCollection<VacationAllowanceViewModel>();
        public ObservableCollection<VacationAllowanceViewModel> VacationAllowancesForSubordinate {
            get => _vacationAllowancesForSubordinate;
            set {
                _vacationAllowancesForSubordinate = value;
                OnPropertyChanged(nameof(VacationAllowancesForSubordinate));
            }
        }

        private VacationAllowanceViewModel _selectedItemAllowance;
        public VacationAllowanceViewModel SelectedItemAllowance {
            get => _selectedItemAllowance;
            set {
                _selectedItemAllowance = value;
                OnPropertyChanged(nameof(SelectedItemAllowance));
                if(SelectedItemAllowance != null) {
                    Calendar.CalendarClickable = SelectedItemAllowance.Vacation_Days_Quantity > 0;
                }
                if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                    App.Current.Dispatcher.Invoke(async () => {
                        await Calendar.ClearVacationData(VacationsToAprovalForSubordinate);
                    });
                } else if(App.SelectedMode == WindowMode.Personal) {
                    App.Current.Dispatcher.Invoke(async () => {
                        await Calendar.ClearVacationData(VacationsToAprovalForPerson);
                    });
                }
            }
        }

        private int _selectedIndexAllowance;
        public int SelectedIndexAllowance {
            get => _selectedIndexAllowance;
            set {
               _selectedIndexAllowance = value;
                OnPropertyChanged(nameof(SelectedIndexAllowance));
            }
        }
        #endregion Vacation Allowance props

        #region Planned Vacation props
        private ObservableCollection<Vacation> _vacationsToAprovalForSubordinate = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsToAprovalForSubordinate {
            get => _vacationsToAprovalForSubordinate;
            set {
                _vacationsToAprovalForSubordinate = value;
                OnPropertyChanged(nameof(VacationsToAprovalForSubordinate));

            }
        }

        private int _countAllowancesForSubordinate; 
        public int CountAllowancesForSubordinate
        {
            get => _countAllowancesForSubordinate;
            set
            {
                _countAllowancesForSubordinate = value;
                OnPropertyChanged(nameof(CountAllowancesForSubordinate));

            }
        }

        private int _countAllowancesForPerson;
        public int CountAllowancesForPerson
        {
            get => _countAllowancesForPerson;
            set
            {
                _countAllowancesForPerson = value;
                OnPropertyChanged(nameof(CountAllowancesForPerson));

            }
        }

        private ObservableCollection<Vacation> _vacationsToAprovalForPerson = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsToAprovalForPerson {
            get => _vacationsToAprovalForPerson;
            set {
                _vacationsToAprovalForPerson = value;
                OnPropertyChanged(nameof(VacationsToAprovalForPerson));

            }
        }

        private ObservableCollection<Vacation> _vacationsTransferedForPerson = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsTransferedForPerson
        {
            get => _vacationsTransferedForPerson;
            set
            {
                _vacationsTransferedForPerson = value;
                OnPropertyChanged(nameof(VacationsTransferedForPerson));

            }
        }

        private ObservableCollection<Vacation> _vacationsTransferedForSubordinate = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsTransferedForSubordinate
        {
            get => _vacationsTransferedForSubordinate;
            set
            {
                _vacationsTransferedForSubordinate = value;
                OnPropertyChanged(nameof(VacationsTransferedForSubordinate));

            }
        }

        private Vacation _plannedItem;
        public Vacation PlannedItem {
            get => _plannedItem;
            set {
                _plannedItem= value;
                SelectedVacation = value;
                OnPropertyChanged(nameof(PlannedItem));
            }
        }

        private int _plannedIndex;
        public int PlannedIndex {
            get => _plannedIndex;
            set {
                _plannedIndex = value;
                OnPropertyChanged(nameof(PlannedIndex));
            }
        }
        private Vacation _plannedPersonItem;
        public Vacation PlannedPersonItem {
            get => _plannedPersonItem;
            set {
                _plannedPersonItem = value;
                OnPropertyChanged(nameof(PlannedPersonItem));
            }
        }

        private int _plannedPersonIndex;
        public int PlannedPersonIndex {
            get => _plannedPersonIndex;
            set {
                _plannedPersonIndex = value;
                OnPropertyChanged(nameof(PlannedPersonIndex));
            }
        }
        #endregion Planned Vacation props

        #region Lerning props
        private double _saveProgress;
        public double SaveProgress {
            get => _saveProgress;
            set {
                _saveProgress = value;
                OnPropertyChanged(nameof(SaveProgress));

            }
        }

        private double _learningProgress;
        public double LearningProgress {
            get => _learningProgress;
            set {
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
        public Brush BorderColorVacation {
            get => _borderColorVacation;
            set {
                _borderColorVacation = value;
                OnPropertyChanged(nameof(BorderColorVacation));
            }
        }

        private Brush _borderColorCalendar;
        public Brush BorderColorCalendar {
            get => _borderColorCalendar;
            set {
                _borderColorCalendar = value;
                OnPropertyChanged(nameof(BorderColorCalendar));
            }
        }

        private Brush _borderColorSelectedGap;
        public Brush BorderColorSelectedGap {
            get => _borderColorSelectedGap;
            set {
                _borderColorSelectedGap = value;
                OnPropertyChanged(nameof(BorderColorSelectedGap));
            }
        }

        private Brush _borderColorPLanedVacations;
        public Brush BorderColorPLanedVacations {
            get => _borderColorPLanedVacations;
            set {
                _borderColorPLanedVacations = value;
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
        public ICommand OpenTransferWindow { get; }
        public ICommand CloseTransferWindow { get; }
        
        public ICommand TransferVacationCommand { get; }
        public ICommand CancelTransferVacationCommand { get; }

        public ICommand CompensateVacation { get; }
        public ICommand ShiftVacations { get; }
        public ICommand CheckVacations { get; }
        public AnotherCommandImplementation MovePrevYearCommand { get; }
        public AnotherCommandImplementation MoveNextYearCommand { get; }
        public AnotherCommandImplementation UpdateData { get; }
        #endregion Commands

        #region Constructor
        public PersonalVacationPlanningViewModel() {
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
            OpenTransferWindow = new OpenTransferWindowCommand(this);
            CloseTransferWindow = new CloseTransferWindowCommand(this);
            TransferVacationCommand = new TransferVacationCommand(this);
            CancelTransferVacationCommand = new CancelTransferVacationCommand(this);
            CompensateVacation = new CompensateVacationCommand(this);
            ShiftVacations = new ShiftVacationsCommand(this);

            PersonName = App.API.Person.ToString();

            MovePrevYearCommand = new AnotherCommandImplementation(
               async _ => {
                   if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD) {
                       if(SelectedSubordinate != null) {
                           IsLoadingCalendarPage = true;
                           IsPreviousYearEnabled = false;
                           IsNextYearEnabled = true;
                           Calendar = Calendars[0];
                           CurrentYear = Calendars[0].CurrentYear;
                           await UpdateDataForSubordinateAsync();
                           IsLoadingCalendarPage = false;
                       } else {
                           ShowAlert("Сначала выберите сотрудника");
                       }
                   } else if(App.SelectedMode == WindowMode.Personal) {
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
               async _ => {
                   if(App.SelectedMode == WindowMode.Subordinate ||
                      App.SelectedMode == WindowMode.HR_GOD ||
                      App.SelectedMode == WindowMode.Accounting) {
                       if(SelectedSubordinate != null) {
                           IsLoadingCalendarPage = true;
                           IsPreviousYearEnabled = true;
                           IsNextYearEnabled = false;
                           Calendar = Calendars[1];
                           CurrentYear = Calendars[1].CurrentYear;
                           await UpdateDataForSubordinateAsync();
                           IsLoadingCalendarPage = false;
                       } else {
                           ShowAlert("Сначала выберете сотрудника");
                       }
                   } else if(App.SelectedMode == WindowMode.Personal) {
                       IsLoadingCalendarPage = true;
                       IsPreviousYearEnabled = true;
                       IsNextYearEnabled = false;
                       Calendar = Calendars[1];
                       CurrentYear = Calendars[1].CurrentYear;
                       UpdateDataForPerson();
                       IsLoadingCalendarPage = false;
                   }
               });

            UpdateData = new AnotherCommandImplementation(
               async _ => {
                   if(App.SelectedMode == WindowMode.Subordinate ||
                      App.SelectedMode == WindowMode.HR_GOD ||
                      App.SelectedMode == WindowMode.Accounting) {
                       if(SelectedSubordinate != null) {
                           await Initialize();
                           await UpdateDataForSubordinateAsync();
                       } else {
                           ShowAlert("Сначала выберете сотрудника");
                       }
                   } else if(App.SelectedMode == WindowMode.Personal) {
                       await Initialize();
                       UpdateDataForPerson();
                   }
               });


            _initializeLazy = new Lazy<Task>(() => Initialize());
            LoadModel.Execute(new object());
            App.API.PersonUpdated += onPersonUpdated;
            IsFlipped = false;
        }

        public async void onPersonUpdated(Person obj)
        {
            if(App.SelectedMode == WindowMode.Subordinate ||
                       App.SelectedMode == WindowMode.HR_GOD ||
                       App.SelectedMode == WindowMode.Accounting)
            {
                if(SelectedSubordinate != null)
                {
                    await Initialize();
                    await UpdateDataForSubordinateAsync();
                } else
                {
                    ShowAlert("Сначала выберете сотрудника");
                }
            } else if(App.SelectedMode == WindowMode.Personal)
            {
                await Initialize();
                UpdateDataForPerson();
            }
        }
        #endregion Constructor

        #region Task Lazy
        private async Task Initialize() {
            IsNextCalendarUnblocked = App.CalendarAPI.CheckDateUnblockedCalendarAsync();
            IsNextCalendarPlannedOpen = App.CalendarAPI.CheckNextCalendarPlanningUnlock();
            if(App.API.Person.Is_HR_GOD) {
                PrepareDepartments();
            } else if(App.API.Person.Is_Accounting) {
                PrepareVirtualDepartments();
            }

            PreparePositionsAndSubordinateNames();
            await prepareCalendar();
            UpdateDataForPerson();
            Calendar = Calendars[0];
        }

        public async Task UpdateDataForSubordinateAsync() {
            VacationAllowancesForSubordinate = new ObservableCollection<VacationAllowanceViewModel>(SelectedSubordinate.Subordinate_Vacation_Allowances.Where(f => f.Vacation_Year == CurrentYear));
            if(VacationAllowancesForSubordinate.Count == 0) {
                VacationAllowancesForSubordinate = Clone(DefaultVacationAllowances);
            }

            VacationsToAprovalForSubordinate = new ObservableCollection<Vacation>(SelectedSubordinate.Subordinate_Vacations.Where(f => f.DateStart.Year == CurrentYear).OrderBy(f => f.DateStart));
            await Calendar.UpdateColorAsync(VacationsToAprovalForSubordinate);
            CountAllowancesForSubordinate = VacationAllowancesForSubordinate.Sum(va => va.Vacation_Days_Quantity);
        }

        public void UpdateDataForPerson() {
            VacationAllowancesForPerson = new ObservableCollection<VacationAllowanceViewModel>(
                                                    App.API.Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == CurrentYear));
            if(VacationAllowancesForPerson.Count == 0 || VacationAllowancesForPerson.Count < 4) {
                VacationAllowancesForPerson = Clone(DefaultVacationAllowances);
            }
            VacationsToAprovalForPerson = new ObservableCollection<Vacation>(
                                                    App.API.Person.User_Vacations.Where(f => f.DateStart.Year == CurrentYear).OrderBy(f => f.DateStart));
            //VacationsToAprovalForPerson.CollectionChanged += async (sender, e) =>
            //{
            //    if(e.Action == NotifyCollectionChangedAction.Remove)
            //    {
            //        foreach(Vacation vacation in e.OldItems)
            //        {
            //            await App.VacationAPI.DeleteVacationAsync(vacation);
            //        }
            //        App.API.Person = await App.API.GetPersonAsync(Environment.UserName);
            //    }
            //};
            CountAllowancesForPerson = VacationAllowancesForPerson.Sum(va => va.Vacation_Days_Quantity);
        }
        private void UpdatePositionNames() {
            FilteredPositionNames.Clear();
            foreach(Subordinate subordinate in FilteredSubordinates) {
                if(!FilteredPositionNames.Contains(subordinate.Position)) {
                    FilteredPositionNames.Add(subordinate.Position);
                }
            }
            FilteredPositionNames = new ObservableCollection<string>(FilteredPositionNames.OrderBy(i => i));
        }
        public void PrepareVirtualDepartments() {
            VirtualDepartments.Clear();
            foreach(Subordinate subordinate in App.API.Person.Subordinates) {
                if(!VirtualDepartments.Contains(subordinate.Virtual_Department_Name)) {
                    VirtualDepartments.Add(subordinate.Virtual_Department_Name);
                }
            }
            VirtualDepartments = new ObservableCollection<string>(VirtualDepartments.OrderBy(i => i));
        }
        public void PrepareDepartments() {
            Departments.Clear();
            foreach(Subordinate subordinate in App.API.Person.Subordinates) {
                if(!Departments.Contains(subordinate.Department_Name)) {
                    Departments.Add(subordinate.Department_Name);
                }
            }
            Departments = new ObservableCollection<string>(Departments.OrderBy(i => i));
        }
        public void PreparePositionsAndSubordinateNames() {
            PositionNames.Clear();

            foreach(Subordinate subordinate in App.API.Person.Subordinates) {
                if(!PositionNames.Contains(subordinate.Position)) {
                    PositionNames.Add(subordinate.Position);
                }
            }
            PositionNames = new ObservableCollection<string>(PositionNames.OrderBy(i => i));
            FilteredPositionNames = Clone(PositionNames);
            FilteredSubordinates = App.API.Person.Subordinates;

            List<string> VacationNames = new List<string> {
                MyEnumExtensions.ToDescriptionString(VacationName.Principal),
                MyEnumExtensions.ToDescriptionString(VacationName.Harmfulness),
                MyEnumExtensions.ToDescriptionString(VacationName.Irregularity),
                MyEnumExtensions.ToDescriptionString(VacationName.Experience)
            };

            VacationNames.ForEach(vacationName => {
                VacationAllowanceViewModel defaultAllowance = new VacationAllowanceViewModel(0, vacationName, 0, 0, 0, null);
                if(!DefaultVacationAllowances.Any(da => da.Vacation_Name == vacationName)) {
                    DefaultVacationAllowances.Add(defaultAllowance);
                }
            });
            VacationAllowancesForSubordinate = Clone(DefaultVacationAllowances);
        }
        public async Task Load() {
            try {
                await _initializeLazy.Value;
            } catch(Exception) {
                _initializeLazy = new Lazy<Task>(() => Initialize());
                throw;
            }
        }
        #endregion Task Lazy

        #region OnStartup

        public async Task prepareCalendar() {
            int interactionCount = 1;
            if(IsNextCalendarUnblocked) {
                interactionCount = 2;
            }
            for(int k = 0; k < interactionCount; k++) {
                await CreateCalendar(k);
            }
        }
        public async Task CreateCalendar(int num) {
            if(Calendars.Count < 2) {
                Calendar = new CustomCalendar(CurrentDate.Year + num, this);
                await Task.Run(async () => await Calendar.Render());
                Calendars.Add(Calendar);
            }
        }

        private void OnHolidaysChanged(List<HolidayViewModel> obj) {
            //Holidays = obj;
            //Task.Run(async () => await Calendar.Render(CurrentDate.Year));
        }

        #endregion OnStartup

        #region Utils

        public void ShowAlert(string alert) {
            _sampleError.ErrorName.Text = alert;
            Task<object> result = DialogHost.Show(_sampleError, "RootDialog", ExtendedClosingEventHandler);
        }

        public async Task UpdateVacationAllowance(int userIdSAP, int vacation_Id, int year, int count) {
            await App.VacationAllowanceAPI.UpdateVacationAllowanceAsync(userIdSAP, vacation_Id, year, count);
        }
        public async Task DeleteVacation(Vacation vacation) {
            await App.VacationAPI.DeleteVacationAsync(vacation);
        }
        public async Task TransferVacation(Vacation vacation)
        {
            await App.VacationAPI.TransferVacationAsync(vacation);
        }
        public void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs) {
            if(eventArgs.Parameter is bool parameter &&
                parameter == false) {
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

        private static ObservableCollection<T> Clone<T>(ObservableCollection<T> listToClone) where T : ICloneable {
            return new ObservableCollection<T>(listToClone.Select(item => (T) item.Clone()));
        }
        #endregion Utils
    }
}
