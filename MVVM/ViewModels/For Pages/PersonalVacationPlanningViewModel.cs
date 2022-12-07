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

        public List<bool> WorkingDays = new List<bool>();

        private List<Vacation> _vacations = new List<Vacation>();
        public List<Vacation> Vacations
        {
            get => _vacations;
            set
            {
                _vacations = value;
                OnPropertyChanged(nameof(Vacations));
            }
        }
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

        #region Calendar
        
        #endregion

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

        private int SelectedDay { get; set; }
        private int SelectedMonth { get; set; }
        private int SelectedYear { get; set; }
        public string SelectedNameDay { get; private set; }
        public bool CalendarClickable { get; set; }

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
        public DateTime FirstSelectedDate { get; set; }
        public DateTime SecondSelectedDate { get; set; }
        public string DayAddition { get; set; }
        public int ClicksOnCalendar { get; set; }
        public int CountSelectedDays { get; set; }

        private string _displayedDateString;
        public string DisplayedDateString
        {
            get => _displayedDateString;
            set
            {
                _displayedDateString = value;
                OnPropertyChanged(nameof(DisplayedDateString));
                IsGapVisible = DisplayedDateString != "";
            }
        }

        private bool _isLoadingPage;
        public bool IsLoadingPage
        {
            get => _isLoadingPage;
            set
            {
                _isLoadingPage = value;
                OnPropertyChanged(nameof(IsLoadingPage));
            }
        }

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
        #endregion Lerning props

        #region Vacation props

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
                    CalendarClickable = SelectedItemAllowance.Vacation_Days_Quantity > 0;
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
        #endregion Vacation props

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

        public void UcDays_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(SelectedItemAllowance != null)
            {
                for(int i = 0; i < VacationAllowances.Count; i++)
                {
                    if(VacationAllowances[i].Vacation_Days_Quantity > 0)
                    {
                        CalendarClickable = true;
                        break;
                    } else
                    {
                        CalendarClickable = false;
                    }
                }
                if(CalendarClickable)
                {
                    if(e.OriginalSource is Grid)
                    {
                        Grid source = e.OriginalSource as Grid;
                        UIElementCollection elems = source.Children as UIElementCollection;
                        foreach(object elem in elems)
                        {
                            if(elem is ContentPresenter)
                            {
                                ContentPresenter presenter = elem as ContentPresenter;
                                TextBlock obj = presenter.Content as TextBlock;
                                SelectedDay = Convert.ToInt32(obj.Text);
                                SelectedMonth = Convert.ToInt32(obj.Tag.ToString().Split(".")[0]);
                                SelectedYear = Convert.ToInt32(obj.Tag.ToString().Split(".")[1]);
                                SelectedNameDay = obj.ToolTip.ToString();
                            }
                        }
                    } else if(e.OriginalSource is TextBlock)
                    {
                        TextBlock obj = e.OriginalSource as TextBlock;

                        SelectedDay = Convert.ToInt32(obj.Text);
                        SelectedMonth = Convert.ToInt32(obj.Tag.ToString().Split(".")[0]);
                        SelectedYear = Convert.ToInt32(obj.Tag.ToString().Split(".")[1]);
                        SelectedNameDay = obj.ToolTip.ToString();
                    } else if(e.OriginalSource is Button)
                    {

                    }

                    ClicksOnCalendar++;

                    DateTime newDate = new DateTime();

                    if(ClicksOnCalendar >= 3)
                    {
                        Calendar.BlockAndPaintButtons();
                        FirstSelectedDate = newDate;
                        SecondSelectedDate = newDate;
                        ClicksOnCalendar = 1;
                        Calendar.UpdateColor();
                    }

                    if(ClicksOnCalendar == 1)
                    {
                        FirstSelectedDate = new DateTime(SelectedYear, SelectedMonth, SelectedDay);
                        CountSelectedDays = FirstSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                        if(CountSelectedDays <= SelectedItemAllowance.Vacation_Days_Quantity)
                        {
                            if(SelectedNameDay != "Праздник")
                            {
                                DayAddition = Calendar.GetDayAddition(CountSelectedDays);
                                DisplayedDateString = DayAddition + ": " + FirstSelectedDate.ToString("d.MM.yyyy");
                                _plannedItem = new Vacation(SelectedItemAllowance.Vacation_Name, Person.Id_SAP, SelectedItemAllowance.Vacation_Id, CountSelectedDays, SelectedItemAllowance.Vacation_Color, FirstSelectedDate, FirstSelectedDate, null);
                            } else
                            {
                                ShowAlert("Этот день является праздичным, начните планирование отпуска с другого дня");
                            }
                        } else
                        {
                            ShowAlert("Выбранный промежуток больше доступного колличества дней");
                        }
                    } else
                    {
                        SecondSelectedDate = new DateTime(SelectedYear, SelectedMonth, SelectedDay);
                        if(SecondSelectedDate > FirstSelectedDate)
                        {
                            CountSelectedDays = SecondSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                            if(CountSelectedDays <= SelectedItemAllowance.Vacation_Days_Quantity)
                            {
                                if(SelectedNameDay != "Праздник")
                                {

                                    DayAddition = Calendar.GetDayAddition(CountSelectedDays);
                                    DisplayedDateString = DayAddition + ": " + FirstSelectedDate.ToString("dd.MM.yyyy") + " - " + SecondSelectedDate.ToString("dd.MM.yyyy");
                                    _plannedItem = new Vacation(SelectedItemAllowance.Vacation_Name, Person.Id_SAP, SelectedItemAllowance.Vacation_Id, CountSelectedDays, SelectedItemAllowance.Vacation_Color, FirstSelectedDate, SecondSelectedDate, null);
                                } else
                                {
                                    ShowAlert("Этот день является праздичным, закончите планирование отпуска другим днём");
                                }
                            } else
                            {
                                ShowAlert("Выбранный промежуток больше доступного колличества дней");
                            }
                        } else
                        {
                            CountSelectedDays = FirstSelectedDate.Subtract(SecondSelectedDate).Days + 1;
                            if(CountSelectedDays <= SelectedItemAllowance.Vacation_Days_Quantity)
                            {
                                if(SelectedNameDay != "Праздник")
                                {

                                    DayAddition = Calendar.GetDayAddition(CountSelectedDays);
                                    DisplayedDateString = DayAddition + ": " + SecondSelectedDate.ToString("dd.MM.yyyy") + " - " + FirstSelectedDate.ToString("dd.MM.yyyy");
                                    _plannedItem = new Vacation(SelectedItemAllowance.Vacation_Name, Person.Id_SAP, SelectedItemAllowance.Vacation_Id, CountSelectedDays, SelectedItemAllowance.Vacation_Color, SecondSelectedDate, FirstSelectedDate, null);
                                } else
                                {
                                    ShowAlert("Этот день является праздичным, закончите планирование отпуска с другим днём");
                                }
                            } else
                            {
                                ShowAlert("Выбранный промежуток больше доступного колличества дней");
                            }
                        }
                    }
                    if(CountSelectedDays <= SelectedItemAllowance.Vacation_Days_Quantity)
                    {
                        if(PlannedItem != null)
                        {

                            Range<DateTime> range = Calendar.ReturnRange(PlannedItem);
                            List<bool> isGoToNext = new List<bool>();

                            foreach(DateTime planedDate in range.Step(x => x.AddDays(1)))
                            {
                                for(int i = 0; i < VacationsToAproval.Count; i++)
                                {
                                    Vacation existingVacation = VacationsToAproval[i];

                                    Range<DateTime> rangeExistingVacation = Calendar.ReturnRange(existingVacation); ;

                                    foreach(DateTime existingDate in rangeExistingVacation.Step(x => x.AddDays(1)))
                                    {
                                        if(existingDate == planedDate)
                                        {
                                            isGoToNext.Add(false);
                                        }
                                    }
                                    if(isGoToNext.Contains(false))
                                    {
                                        break;
                                    }
                                }
                            }

                            if(!isGoToNext.Contains(false))
                            {
                                Calendar.BlockAndPaintButtons();
                            } else
                            {
                                ShowAlert("Пересечение отпусков не допустимо");
                                Calendar.BlockAndPaintButtons();
                                FirstSelectedDate = newDate;
                                SecondSelectedDate = newDate;
                                ClicksOnCalendar = 0;
                                CountSelectedDays = 0;
                                DisplayedDateString = "";
                                Calendar.UpdateColor();
                            }
                        }
                    } else
                    {

                        ShowAlert("Выбранный промежуток больше доступного колличества дней");
                        Calendar.BlockAndPaintButtons();
                        FirstSelectedDate = newDate;
                        SecondSelectedDate = newDate;
                        ClicksOnCalendar = 0;
                        CountSelectedDays = 0;
                        DisplayedDateString = "";
                        Calendar.UpdateColor();
                        //PlannedItem = null;
                    }
                }
            } else
            {
                ShowAlert("Выберете тип отпуска");
            }
        }

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
            IsLoadingPage = true;
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
            IsLoadingPage = false;
        }

        private void OnHolidaysChanged(List<HolidayViewModel> obj)
        {
            Holidays = obj;
            //Calendar = new CustomCalendar(CurrentDate, this);
            //Calendar.RenderCalendar();
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
