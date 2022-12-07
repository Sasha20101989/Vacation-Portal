using MaterialDesignThemes.Wpf;
using MiscUtil.Collections;
using MiscUtil.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.ViewModels.Calendar;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages
{
    public class PersonalVacationPlanningViewModel : ViewModelBase
    {
        #region Props
        private Lazy<Task> _initializeLazy;
        private readonly SampleError _sampleError = new SampleError();
        public CustomCalendar Calendar { get; set; }

        private List<HolidayViewModel> _holidays = new List<HolidayViewModel>();
        public List<HolidayViewModel> Holidays
        {
            get => _holidays;
            set
            {
                _holidays = value;
                OnPropertyChanged(nameof(Holidays));
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
        private Person _person = App.API.Person;
        public Person Person
        {
            get => _person;
            set
            {
                _person = value;
                OnPropertyChanged(nameof(Person));

            }
        }

        public string PersonName { get; set; } = App.API.Person.ToString();

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
        public ICommand SaveDataModel { get;}
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
            Calendar = new CustomCalendar(CurrentDate, this);
            //TODO:сделать получение и создание 2 календарей сразу
            MessageQueueCalendar = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
            MessageQueueSelectedGap = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
            MessageQueuePLanedVacations = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));

            LoadModel = new LoadModelCommand(this);
            CheckVacations = new CheckVacationsCommand(this);
            SaveDataModel = new SaveDataModelCommand(this);
            StartLearning = new StartLearningCommand(this);
            AddToApprovalList = new AddToApprovalListCommand(this);
            CancelVacation = new RelayCommand(SelectedCommandHandler, CanExecuteSelectedCommand);

            
            MovePrevYearCommand = new AnotherCommandImplementation(
               async _ =>
               {
                   IsPreviousYearEnabled = false;
                   IsNextYearEnabled = true;
                   CurrentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                   await LoadVacationAllowanceForYearAsync();
               });
            MoveNextYearCommand = new AnotherCommandImplementation(
               async _ =>
               {
                   IsPreviousYearEnabled = true;
                   IsNextYearEnabled = false;
                   CurrentDate = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);
                   await LoadVacationAllowanceForYearAsync();
               });

            _initializeLazy = new Lazy<Task>(async () => await Initialize());
            LoadModel.Execute(new object());
            App.API.OnHolidaysChanged += OnHolidaysChanged;
        }
        #endregion Constructor

        #region Task Lazy
        private async Task Initialize()
        {
            await LoadVacationAllowanceForYearAsync();
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

        public async Task LoadVacationAllowanceForYearAsync()
        {
            IsLoadingCalendarPage = true;
            //await Task.Delay(2000);
            VacationAllowances.Clear();
            VacationsToAproval.Clear();
            IEnumerable<HolidayViewModel> holidays = await App.API.GetHolidaysAsync(Convert.ToInt32(CurrentDate.Year));
            OnHolidaysLoad(holidays);
            IEnumerable<VacationViewModel> vacations = await App.API.LoadVacationAsync(App.API.Person.Id_SAP, CurrentDate.Year);
            OnVacationsLoad(vacations);
            IEnumerable<VacationAllowanceViewModel> vacationAllowances = await App.API.GetVacationAllowanceAsync(App.API.Person.Id_SAP, Convert.ToInt32(CurrentDate.Year));
            OnVacationAllowanceLoad(vacationAllowances);
            Calendar.UpdateColor();
            IsLoadingCalendarPage = false;
        }

        private void OnHolidaysChanged(List<HolidayViewModel> obj)
        {
            Holidays = obj;
            //Calendar = new CustomCalendar(CurrentDate, this);
            Calendar.Render(CurrentDate);
        }
        private void OnHolidaysLoad(IEnumerable<HolidayViewModel> holidays)
        {
            foreach(HolidayViewModel holiday in holidays)
            {
                if(!Holidays.Contains(new HolidayViewModel(holiday.Id, holiday.TypeOfHoliday, holiday.Date, Convert.ToInt32(holiday.Date.Year))))
                {
                    Holidays.Add(new HolidayViewModel(holiday.Id, holiday.TypeOfHoliday, holiday.Date, Convert.ToInt32(holiday.Date.Year)));
                    App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
                }
            }
            Task.Run(() => Calendar.Render(CurrentDate));
        }
        private void OnVacationsLoad(IEnumerable<VacationViewModel> vacations)
        {
            foreach(VacationViewModel item in vacations)
            {
                Vacation vacation = new Vacation(item.Name, item.User_Id_SAP, item.Vacation_Id, item.Count, item.Color, item.DateStart, item.DateEnd, item.Status);
                if(!VacationsToAproval.Contains(vacation))
                {
                    App.API.Person.Vacations.Add(item);
                    VacationsToAproval.Add(vacation);
                }
            }
        }
        private void OnVacationAllowanceLoad(IEnumerable<VacationAllowanceViewModel> vacationAllowances)
        {
            foreach(VacationAllowanceViewModel item in vacationAllowances)
            {
                VacationAllowances.Add(item);
            }
            for(int i = 0; i < VacationAllowances.Count; i++)
            {
                if(VacationAllowances[i].Vacation_Days_Quantity > 0)
                {
                    SelectedIndexAllowance = i;
                    break;
                }
            }
        }

        
        #endregion OnStartup

        #region Utils
        public void ShowAlert(string alert)
        {
            _sampleError.ErrorName.Text = alert;
            Task<object> result = DialogHost.Show(_sampleError, "RootDialog", ExtendedClosingEventHandler);
        }
        private void SelectedCommandHandler(object data)
        {
            Vacation deletedItem = (Vacation) data;
            if(deletedItem != null)
            {
                int index = 0;
                for(int i = 0; i < VacationAllowances.Count; i++)
                {
                    if(VacationAllowances[i].Vacation_Name == deletedItem.Name)
                    {
                        index = i;
                        break;
                    }
                }
                VacationAllowances[index].Vacation_Days_Quantity += deletedItem.Count;
                Task.Run(async () => await UpdateVacationAllowance(deletedItem.Vacation_Id, deletedItem.Date_Start.Year, VacationAllowances[index].Vacation_Days_Quantity));
                Task.Run(async () => await DeleteVacation(deletedItem));
                VacationsToAproval.Remove(deletedItem);
                PlannedIndex = 0;
                Calendar.UpdateColor();
            }
        }

        public async Task UpdateVacationAllowance(int vacation_Id, int year, int count)
        {
            await App.API.UpdateVacationAllowanceAsync(vacation_Id, year, count);
        }
        private async Task DeleteVacation(Vacation vacation)
        {
            await App.API.DeleteVacationAsync(vacation);
        }
        private bool CanExecuteSelectedCommand(object data)
        {
            return true;
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
