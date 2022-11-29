using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using MiscUtil.Collections;
using MiscUtil.Collections.Extensions;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands;
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
        private SampleError _sampleError = new SampleError();

        public BindingList<DateTime> DayOffs { get; set; } = new BindingList<DateTime>();

        private List<HolidayViewModel> _holidays = new List<HolidayViewModel>();
        public List<HolidayViewModel> Holidays
        {
            get
            {
                return _holidays;
            }
            set
            {
                _holidays = value;
                OnPropertyChanged(nameof(Holidays));
            }
        }

        public List<ObservableCollection<DayControl>> Year { get; set; } = new List<ObservableCollection<DayControl>>();
        public ObservableCollection<CalendarViewModel> FullYear { get; set; } = new ObservableCollection<CalendarViewModel>();

        private ObservableCollection<DayControl> _daysJanuary = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysJanuary
        {
            get
            {
                return _daysJanuary;
            }
            set
            {
                _daysJanuary = value;
                OnPropertyChanged(nameof(DaysJanuary));

            }
        }
        public int ColumnOfWeekJanuary { get; set; }
        public int CountRowsJanuary { get; set; }

        private ObservableCollection<DayControl> _daysFebruary = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysFebruary
        {
            get
            {
                return _daysFebruary;
            }
            set
            {
                _daysFebruary = value;
                OnPropertyChanged(nameof(DaysFebruary));

            }
        }
        public int ColumnOfWeekFebruary { get; set; }
        public int CountRowsFebruary { get; set; }

        private ObservableCollection<DayControl> _daysMarch = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysMarch
        {
            get
            {
                return _daysMarch;
            }
            set
            {
                _daysMarch = value;
                OnPropertyChanged(nameof(DaysMarch));

            }
        }
        public int ColumnOfWeekMarch { get; set; }
        public int CountRowsMarch { get; set; }

        private ObservableCollection<DayControl> _daysApril = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysApril
        {
            get
            {
                return _daysApril;
            }
            set
            {
                _daysApril = value;
                OnPropertyChanged(nameof(DaysApril));

            }
        }
        public int ColumnOfWeekApril { get; set; }
        public int CountRowsApril { get; set; }

        private ObservableCollection<DayControl> _daysMay = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysMay
        {
            get
            {
                return _daysMay;
            }
            set
            {
                _daysMay = value;
                OnPropertyChanged(nameof(DaysMay));

            }
        }
        public int ColumnOfWeekMay { get; set; }
        public int CountRowsMay { get; set; }

        private ObservableCollection<DayControl> _daysJune = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysJune
        {
            get
            {
                return _daysJune;
            }
            set
            {
                _daysJune = value;
                OnPropertyChanged(nameof(DaysJune));

            }
        }
        public int ColumnOfWeekJune { get; set; }
        public int CountRowsJune { get; set; }

        private ObservableCollection<DayControl> _daysJuly = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysJuly
        {
            get
            {
                return _daysJuly;
            }
            set
            {
                _daysJuly = value;
                OnPropertyChanged(nameof(DaysJuly));

            }
        }
        public int ColumnOfWeekJuly { get; set; }
        public int CountRowsJuly { get; set; }

        private ObservableCollection<DayControl> _daysAugust = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysAugust
        {
            get
            {
                return _daysAugust;
            }
            set
            {
                _daysAugust = value;
                OnPropertyChanged(nameof(DaysAugust));

            }
        }
        public int ColumnOfWeekAugust { get; set; }
        public int CountRowsAugust { get; set; }

        private ObservableCollection<DayControl> _daysSeptember = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysSeptember
        {
            get
            {
                return _daysSeptember;
            }
            set
            {
                _daysSeptember = value;
                OnPropertyChanged(nameof(DaysSeptember));

            }
        }
        public int ColumnOfWeekSeptember { get; set; }
        public int CountRowsSeptember { get; set; }

        private ObservableCollection<DayControl> _daysOktober = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysOktober
        {
            get
            {
                return _daysOktober;
            }
            set
            {
                _daysOktober = value;
                OnPropertyChanged(nameof(DaysOktober));

            }
        }
        public int ColumnOfWeekOktober { get; set; }
        public int CountRowsOktober { get; set; }

        private ObservableCollection<DayControl> _daysNovember = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysNovember
        {
            get
            {
                return _daysNovember;
            }
            set
            {
                _daysNovember = value;
                OnPropertyChanged(nameof(DaysNovember));

            }
        }
        public int ColumnOfWeekNovember { get; set; }
        public int CountRowsNovember { get; set; }

        private ObservableCollection<DayControl> _daysDecember = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysDecember
        {
            get
            {
                return _daysDecember;
            }
            set
            {
                _daysDecember = value;
                OnPropertyChanged(nameof(DaysDecember));

            }
        }
        public int ColumnOfWeekDecember { get; set; }
        public int CountRowsDecember { get; set; }


        private ObservableCollection<VacationViewModel> _allDaysToAproval;
        public ObservableCollection<VacationViewModel> AllDaysToAproval
        {
            get
            {
                return _allDaysToAproval;
            }
            set
            {
                _allDaysToAproval = value;
                OnPropertyChanged(nameof(AllDaysToAproval));

            }
        }

        private ObservableCollection<Vacation> _vacationsToAproval;
        public ObservableCollection<Vacation> VacationsToAproval
        {
            get
            {
                return _vacationsToAproval;
            }
            set
            {
                _vacationsToAproval = value;
                OnPropertyChanged(nameof(VacationsToAproval));

            }
        }
        private ObservableCollection<Vacation> _vacationTypes;
        public ObservableCollection<Vacation> VacationTypes
        {
            get
            {
                return _vacationTypes;
            }
            set
            {
                _vacationTypes = value;
                OnPropertyChanged(nameof(VacationTypes));
            }
        }


        #region Person props
        private Person _person;
        public Person Person
        {
            get
            {
                return _person;
            }
            set
            {
                _person = value;
                OnPropertyChanged(nameof(Person));

            }
        }

        public string PersonName { get; set; }

        private bool _isHR = false;
        public bool IsHR
        {
            get
            {
                return _isHR;
            }
            set
            {
                _isHR = value;
                OnPropertyChanged(nameof(IsHR));

            }
        }

        private bool _isSupervisor = false;
        public bool IsSupervisor
        {
            get
            {
                return _isSupervisor;
            }
            set
            {
                _isSupervisor = value;
                OnPropertyChanged(nameof(IsSupervisor));

            }
        }

        private bool _isEmployee = false;
        public bool IsEmployee
        {
            get
            {
                return _isEmployee;
            }
            set
            {
                _isEmployee = value;
                OnPropertyChanged(nameof(IsEmployee));

            }
        }
        #endregion Person props

        private int SelectedDay { get; set; }
        private int SelectedMonth { get; set; }
        public string SelectedNameDay { get; private set; }
        private DateTime CurrentDate { get; set; } = DateTime.Now;
        private DateTime FirstSelectedDate { get; set; }
        private DateTime SecondSelectedDate { get; set; }
        private string DayAddition { get; set; }
        public int ClicksOnCalendar { get; set; }
        public int CountSelectedDays { get; set; }
        private string _displayedDateString;
        public string DisplayedDateString
        {
            get { return _displayedDateString; }
            set
            {
                _displayedDateString = value;
                OnPropertyChanged(nameof(DisplayedDateString));
                if (DisplayedDateString != "")
                {
                    IsGapVisible = true;
                }
                else
                {
                    IsGapVisible = false;
                }
            }
        }

        #region Button Interaction
        private bool _isGapVisible;
        public bool IsGapVisible
        {
            get
            {
                return _isGapVisible;
            }
            set
            {
                _isGapVisible = value;
                OnPropertyChanged(nameof(IsGapVisible));

            }
        }

        private bool _isSaveComplete;
        public bool IsSaveComplete
        {
            get
            {
                return _isSaveComplete;
            }
            set
            {
                _isSaveComplete = value;
                OnPropertyChanged(nameof(IsSaveComplete));

            }
        }

        private bool _isSaving;
        public bool IsSaving
        {
            get
            {
                return _isSaving;
            }
            set
            {
                _isSaving = value;
                OnPropertyChanged(nameof(IsSaving));

            }
        }

        private bool _isEnabled;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));

            }
        }
        #endregion Button Interaction

        #region Lerning props
        private double _saveProgress;
        public double SaveProgress
        {
            get
            {
                return _saveProgress;
            }
            set
            {
                _saveProgress = value;
                OnPropertyChanged(nameof(SaveProgress));

            }
        }

        private double _learningProgress;
        public double LearningProgress
        {
            get
            {
                return _learningProgress;
            }
            set
            {
                _learningProgress = value;
                OnPropertyChanged(nameof(LearningProgress));

            }
        }
        #endregion Lerning props

        #region Vacation props
        private Vacation _selectedItem;
        public Vacation SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnPropertyChanged(nameof(SelectedItem));
                if (SelectedItem.Count > 0)
                {
                    CalendarClickable = true;
                }
                else
                {
                    CalendarClickable = false;
                }
                clearVacationData();
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                SetProperty(ref _selectedIndex, value);
                OnPropertyChanged(nameof(SelectedIndex));
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
        private Vacation _prevPlannedItem;
        public Vacation PrevPlannedItem
        {
            get => _prevPlannedItem;
            set
            {
                SetProperty(ref _prevPlannedItem, value);
                OnPropertyChanged(nameof(PrevPlannedItem));
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
        public ICommand LoadModel { get; set; }
        public ICommand SaveDataModel { get; set; }
        public ICommand StartLearning { get; set; }
        public ICommand AddToApprovalList { get; set; }
        public ICommand CancelVacation { get; set; }
        public ICommand CheckVacations { get; set; }

        #endregion Commands
        public bool CalendarClickable { get; set; }

        private void SelectedCommandHandler(object data)
        {
            var deletedItem = (Vacation)data;
            if (deletedItem != null)
            {
                int index = 0;
                for (int i = 0; i < VacationTypes.Count; i++)
                {
                    if (VacationTypes[i].Name == deletedItem.Name)
                    {
                        index = i;
                        break;
                    }
                }
                VacationTypes[index].Count += deletedItem.Count;
                VacationsToAproval.Remove(deletedItem);
                PlannedIndex = 0;
                SelectedItem = VacationTypes[index];
                clearColorAndBlocked();
            }
        }

        private bool CanExecuteSelectedCommand(object data) => true;

        #region Constructor
        public PersonalVacationPlanningViewModel()
        {
            _vacationsToAproval = new ObservableCollection<Vacation>();
            _vacationTypes = new ObservableCollection<Vacation>();
            _allDaysToAproval = new ObservableCollection<VacationViewModel>();
            PersonName = App.API.Person.ToString();
            Person = App.API.Person;
            IsEmployee = true;
            LoadModel = new LoadModelCommand(this);
            CheckVacations = new CheckVacationsCommand(this);
            SaveDataModel = new SaveDataModelCommand(this);
            StartLearning = new StartLearningCommand(this);
            AddToApprovalList = new AddToApprovalListCommand(this);
            CancelVacation = new RelayCommand(SelectedCommandHandler, CanExecuteSelectedCommand);
            _initializeLazy = new Lazy<Task>(async () => await Initialize());
            LoadModel.Execute(new object());
            App.API.OnHolidaysChanged += OnHolidaysChanged;
        }
        public void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter is bool parameter &&
                parameter == false) return;
            eventArgs.Cancel();

            Task.Delay(TimeSpan.FromSeconds(0.3))
                .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                    TaskScheduler.FromCurrentSynchronizationContext());
            //Task.Delay(TimeSpan.FromSeconds(0.1))
            //    .ContinueWith((t, _) => eventArgs.Session.UpdateContent(new SampleError()), null,
            //        TaskScheduler.FromCurrentSynchronizationContext());
        }
        private void OnHolidaysChanged(List<HolidayViewModel> obj)
        {
            Holidays = obj;
            OnCalendarDatesLoaded();
        }
        #endregion Constructor
        public List<bool> WorkingDays = new List<bool>();
        #region Calendar Interaction
        private void UcDays_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < VacationTypes.Count; i++)
            {
                if (VacationTypes[i].Count > 0)
                {
                    CalendarClickable = true;
                    break;
                }
                else
                {
                    CalendarClickable = false;
                }
            }
            if (CalendarClickable)
            {
                if (e.OriginalSource is Grid)
                {
                    Grid source = e.OriginalSource as Grid;
                    UIElementCollection elems = source.Children as UIElementCollection;
                    foreach (var elem in elems)
                    {
                        if (elem is ContentPresenter)
                        {
                            ContentPresenter presenter = elem as ContentPresenter;
                            TextBlock obj = presenter.Content as TextBlock;
                            SelectedDay = Convert.ToInt32(obj.Text);
                            SelectedMonth = Convert.ToInt32(obj.Tag);
                            SelectedNameDay = obj.ToolTip.ToString();
                        }
                    }
                }
                else if (e.OriginalSource is TextBlock)
                {
                    TextBlock obj = e.OriginalSource as TextBlock;

                    SelectedDay = Convert.ToInt32(obj.Text);
                    SelectedMonth = Convert.ToInt32(obj.Tag);
                    SelectedNameDay = obj.ToolTip.ToString();
                }
                else if (e.OriginalSource is Button)
                {

                }

                ClicksOnCalendar++;

                DateTime newDate = new DateTime();

                if (ClicksOnCalendar >= 3)
                {
                    blockAndPaintButtons();
                    FirstSelectedDate = newDate;
                    SecondSelectedDate = newDate;
                    ClicksOnCalendar = 1;
                    clearColorAndBlocked();
                }

                if (ClicksOnCalendar == 1)
                {
                    FirstSelectedDate = new DateTime(CurrentDate.Year, SelectedMonth, SelectedDay);
                    CountSelectedDays = FirstSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                    if (CountSelectedDays <= SelectedItem.Count)
                    {
                        if (SelectedNameDay != "Праздник")
                        {
                            DayAddition = getDayAddition(CountSelectedDays);
                            DisplayedDateString = DayAddition + ": " + FirstSelectedDate.ToString("d.MM.yyyy");
                            _plannedItem = new Vacation(SelectedItem.Name, CountSelectedDays, SelectedItem.Color, FirstSelectedDate, FirstSelectedDate);
                        }
                        else
                        {
                            ShowAlert("Этот день является праздичным, начните планирование отпуска с другого дня");
                        }
                    }
                    else
                    {
                        ShowAlert("Выбранный промежуток больше доступного колличества дней");
                    }
                }
                else
                {
                    SecondSelectedDate = new DateTime(CurrentDate.Year, SelectedMonth, SelectedDay);
                    if (SecondSelectedDate > FirstSelectedDate)
                    {
                        CountSelectedDays = SecondSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                        if (CountSelectedDays <= SelectedItem.Count)
                        {
                            if (SelectedNameDay != "Праздник")
                            {
                                
                                DayAddition = getDayAddition(CountSelectedDays);
                                DisplayedDateString = DayAddition + ": " + FirstSelectedDate.ToString("dd.MM.yyyy") + " - " + SecondSelectedDate.ToString("dd.MM.yyyy");
                                _plannedItem = new Vacation(SelectedItem.Name, CountSelectedDays, SelectedItem.Color, FirstSelectedDate, SecondSelectedDate);
                            }
                            else
                            {
                                ShowAlert("Этот день является праздичным, закончите планирование отпуска другим днём");
                            }
                        }
                        else
                        {
                            ShowAlert("Выбранный промежуток больше доступного колличества дней");
                        }
                    }
                    else
                    {
                        CountSelectedDays = FirstSelectedDate.Subtract(SecondSelectedDate).Days + 1;
                        if (CountSelectedDays <= SelectedItem.Count)
                        {
                            if (SelectedNameDay != "Праздник")
                            {
                                
                                DayAddition = getDayAddition(CountSelectedDays);
                                DisplayedDateString = DayAddition + ": " + SecondSelectedDate.ToString("dd.MM.yyyy") + " - " + FirstSelectedDate.ToString("dd.MM.yyyy");
                                _plannedItem = new Vacation(SelectedItem.Name, CountSelectedDays, SelectedItem.Color, SecondSelectedDate, FirstSelectedDate);
                            }
                            else
                            {
                                ShowAlert("Этот день является праздичным, закончите планирование отпуска с другим днём");
                            }
                        }
                        else
                        {
                            ShowAlert("Выбранный промежуток больше доступного колличества дней");
                        }
                    }

                }
                if (CountSelectedDays <= SelectedItem.Count)
                {
                    if (PlannedItem != null)
                    {

                        Range<DateTime> range = ReturnRange(PlannedItem);
                        List<bool> isGoToNext = new List<bool>();

                        foreach (DateTime planedDate in range.Step(x => x.AddDays(1)))
                        {
                            for (int i = 0; i < VacationsToAproval.Count; i++)
                            {
                                Vacation existingVacation = VacationsToAproval[i];

                                Range<DateTime> rangeExistingVacation = ReturnRange(existingVacation); ;

                                foreach (DateTime existingDate in rangeExistingVacation.Step(x => x.AddDays(1)))
                                {
                                    if (existingDate == planedDate)
                                    {
                                        isGoToNext.Add(false);
                                    }
                                }
                                if (isGoToNext.Contains(false))
                                {
                                    break;
                                }
                            }
                        }

                        if (!isGoToNext.Contains(false))
                        {
                            blockAndPaintButtons();
                        }
                        else
                        {
                            ShowAlert("Пересечение отпусков не допустимо");
                            blockAndPaintButtons();
                            FirstSelectedDate = newDate;
                            SecondSelectedDate = newDate;
                            ClicksOnCalendar = 0;
                            CountSelectedDays = 0;
                            DisplayedDateString = "";
                            clearColorAndBlocked();
                        }

                    }
                }
                else
                {

                    ShowAlert("Выбранный промежуток больше доступного колличества дней");
                    blockAndPaintButtons();
                    FirstSelectedDate = newDate;
                    SecondSelectedDate = newDate;
                    ClicksOnCalendar = 0;
                    CountSelectedDays = 0;
                    DisplayedDateString = "";
                    clearColorAndBlocked();
                    //PlannedItem = null;
                }
            }
        }

        private void clearColorAndBlocked()
        {
            foreach (ObservableCollection<DayControl> month in Year)
            {
                foreach (DayControl item in month)
                {
                    Grid parentItem = item.Content as Grid;
                    UIElementCollection buttons = parentItem.Children as UIElementCollection;
                    foreach (var elem in buttons)
                    {
                        Button button = elem as Button;
                        button.Background = Brushes.Transparent;
                        //button.IsEnabled = true;
                        fillPlanedDays(button);
                    }
                }
            }
        }
        private void fillPlanedDays(Button button)
        {
            TextBlock textBlock = button.Content as TextBlock;
            for (int i = 0; i < VacationsToAproval.Count; i++)
            {
                Range<DateTime> range = ReturnRange(VacationsToAproval[i]);
                foreach (DateTime date in range.Step(x => x.AddDays(1)))
                {
                    if (date.Day == Convert.ToInt32(textBlock.Text) && date.Month == Convert.ToInt32(textBlock.Tag))
                    {
                        button.Background = VacationsToAproval[i].Color;
                        //button.IsEnabled = false;
                    }
                }
            }
        }
        private void PaintButtons()
        {
            foreach (var vacation in VacationsToAproval)
            {
                Range<DateTime> range = ReturnRange(vacation);
                foreach (DateTime date in range.Step(x => x.AddDays(1)))
                {
                    foreach (ObservableCollection<DayControl> month in Year)
                    {
                        foreach (DayControl itemContol in month)
                        {
                            Grid parentItem = itemContol.Content as Grid;
                            UIElementCollection buttons = parentItem.Children as UIElementCollection;
                            foreach (var elem in buttons)
                            {
                                Button button = elem as Button;
                                var buttonTextBlock = button.Content as TextBlock;
                                int buttonDay = Convert.ToInt32(buttonTextBlock.Text);
                                int buttonMonth = Convert.ToInt32(buttonTextBlock.Tag);
                                if (date.Day == buttonDay && date.Month == buttonMonth)
                                {
                                    //button.IsEnabled = false;
                                    button.Background = vacation.Color;
                                }
                            }
                        }
                    }
                }
            }



        }
        private void blockAndPaintButtons()
        {
            if (PlannedItem != null)
            {
                Range<DateTime> range = ReturnRange(PlannedItem);

                foreach (DateTime date in range.Step(x => x.AddDays(1)))
                {
                    foreach (ObservableCollection<DayControl> month in Year)
                    {
                        foreach (DayControl item in month)
                        {
                            Grid parentItem = item.Content as Grid;
                            UIElementCollection buttons = parentItem.Children as UIElementCollection;
                            foreach (var elem in buttons)
                            {
                                Button button = elem as Button;
                                var buttonTextBlock = button.Content as TextBlock;
                                int buttonDay = Convert.ToInt32(buttonTextBlock.Text);
                                int buttonMonth = Convert.ToInt32(buttonTextBlock.Tag);
                                string buttonNameOfDay = buttonTextBlock.ToolTip.ToString();
                                if (date.Day == buttonDay && date.Month == buttonMonth)
                                {
                                    //button.IsEnabled = false;
                                    if (buttonNameOfDay == "Праздник")
                                    {
                                        PlannedItem.Count--;
                                        CountSelectedDays--;
                                        DayAddition = getDayAddition(CountSelectedDays);
                                        DisplayedDateString = DayAddition + ": " + SecondSelectedDate.ToString("dd.MM.yyyy") + " - " + FirstSelectedDate.ToString("dd.MM.yyyy");
                                    }

                                    button.Background = PlannedItem.Color;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void clearVacationData()
        {
            DisplayedDateString = "";
            ClicksOnCalendar = 0;
            clearColorAndBlocked();
        }
        #endregion Calendar Interaction

        #region Task Lazy
        private async Task Initialize()
        {

            loadVacationTypes();
            await Task.Run(async () => await RenderCalendars());

        }

        public async Task Load()
        {
            try
            {
                await _initializeLazy.Value;
            }
            catch (Exception)
            {
                _initializeLazy = new Lazy<Task>(async () => await Initialize());
                throw;
            }
        }
        #endregion Task Lazy

        #region OnStartup

        private void CheckRang()
        {
            if (App.API.Person.Is_Supervisor)
            {
                IsSupervisor = true;
            }
            else if (App.API.Person.Is_HR)
            {
                IsHR = true;
            }
            else
            {
                IsEmployee = true;
            }

            //Holidays = await _метод получения данных
            //Weekends = await _метод получения данных


        }

        private void loadVacationTypes()
        {
            //VacationTypes = await _метод получения данных
            var converter = new System.Windows.Media.BrushConverter();
            VacationTypes.Add(new Vacation("Основной", 28, (Brush)converter.ConvertFromString("#9B4F2D"), DateTime.Now, DateTime.Now));
            VacationTypes.Add(new Vacation("Ненормированность", 3, (Brush)converter.ConvertFromString("#2D6D9B"), DateTime.Now, DateTime.Now));
            VacationTypes.Add(new Vacation("Вредность", 7, (Brush)converter.ConvertFromString("#9B2D84"), DateTime.Now, DateTime.Now));
            VacationTypes.Add(new Vacation("Стаж", 2, (Brush)converter.ConvertFromString("#9B8C2D"), DateTime.Now, DateTime.Now));

            //VacationTypess.Add(new Vacation("Основной", 28, (Brush)converter.ConvertFromString("#9B4F2D"), DateTime.Now, DateTime.Now));
            //VacationTypess.Add(new Vacation("Ненормированность", 3, (Brush)converter.ConvertFromString("#2D6D9B"), DateTime.Now, DateTime.Now));
            //VacationTypess.Add(new Vacation("Вредность", 0, (Brush)converter.ConvertFromString("#9B2D84"), DateTime.Now, DateTime.Now));
            //VacationTypess.Add(new Vacation("Стаж", 2, (Brush)converter.ConvertFromString("#9B8C2D"), DateTime.Now, DateTime.Now));
            //VacationTypess.Add(new Vacation("Основной", 28, (Brush)converter.ConvertFromString("#9B4F2D"), DateTime.Now, DateTime.Now));
            //VacationTypess.Add(new Vacation("Ненормированность", 3, (Brush)converter.ConvertFromString("#2D6D9B"), DateTime.Now, DateTime.Now));
            //VacationTypess.Add(new Vacation("Вредность", 0, (Brush)converter.ConvertFromString("#9B2D84"), DateTime.Now, DateTime.Now));
            //VacationTypess.Add(new Vacation("Стаж", 2, (Brush)converter.ConvertFromString("#9B8C2D"), DateTime.Now, DateTime.Now));
            //VacationTypess.Add(new Vacation("Основной", 28, (Brush)converter.ConvertFromString("#9B4F2D"), DateTime.Now, DateTime.Now));
            //VacationTypess.Add(new Vacation("Ненормированность", 3, (Brush)converter.ConvertFromString("#2D6D9B"), DateTime.Now, DateTime.Now));
            //VacationTypess.Add(new Vacation("Вредность", 0, (Brush)converter.ConvertFromString("#9B2D84"), DateTime.Now, DateTime.Now));
            //VacationTypess.Add(new Vacation("Стаж", 2, (Brush)converter.ConvertFromString("#9B8C2D"), DateTime.Now, DateTime.Now));
        }

        private async Task RenderCalendars()
        {

            //IEnumerable<HolidayViewModel> calendarDates = await App.API.GetHolidaysAsync(new DateTime(DateTime.Now.Year, 1, 1), new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12)));
            //Holidays.AddRange(calendarDates);
            //OnCalendarDatesLoaded(calendarDates);
            OnCalendarDatesLoaded();

            //Weekends.Add(DateTime.Today.AddDays(1));  
        }

        //private void OnCalendarDatesLoaded(IEnumerable<HolidayViewModel> calendarDates)
        public void OnCalendarDatesLoaded()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                FullYear.Clear();
                Year.Clear();
                DaysJanuary.Clear();
                DaysFebruary.Clear();
                DaysMarch.Clear();
                DaysApril.Clear();
                DaysMay.Clear();
                DaysJune.Clear();
                DaysJuly.Clear();
                DaysAugust.Clear();
                DaysSeptember.Clear();
                DaysOktober.Clear();
                DaysNovember.Clear();
                DaysDecember.Clear();



                for (int j = 1; j <= 12; j++)
                {
                    int days = DateTime.DaysInMonth(CurrentDate.Year, j);
                    string daysOfWeekString = new DateTime(CurrentDate.Year, j, 1).DayOfWeek.ToString("d");
                    int daysOfWeek = 6;
                    if (daysOfWeekString != "0")
                    {
                        daysOfWeek = Convert.ToInt32(daysOfWeekString) - 1;
                    }

                    int WorkDaysCount = 0;
                    int DayOffCount = 0;
                    int HolidaysCount = 0;
                    int WorkingOnHolidayCount = 0;
                    int UnscheduledCount = 0;

                    ObservableCollection<DayViewModel> FullDays = new ObservableCollection<DayViewModel>();
                    ObservableCollection<DayControl> Days = new ObservableCollection<DayControl>();
                    for (int i = 1; i <= days; i++)
                    {
                        DayControl ucDays = new DayControl();
                        DateTime date = new DateTime(CurrentDate.Year, j, i);

                        if (date.DayOfWeek.ToString("d") == "6" || date.DayOfWeek.ToString("d") == "0")
                        {
                            DayOffCount++;
                            WorkDaysCount = days - DayOffCount;
                        }

                        ucDays.day(date);
                        ucDays.daysOff(date);
                        ucDays.PreviewMouseLeftButtonDown += UcDays_PreviewMouseLeftButtonDown;


                        for (int k = 0; k < App.API.Holidays.Count; k++)
                        {
                            HolidayViewModel holiday = App.API.Holidays[k];
                            if (holiday.Date.Month == j && holiday.Date.Day == i)
                            {
                                if (holiday.TypeOfHoliday == "Выходной")
                                {
                                    ucDays.dayOff(date);
                                    DayOffCount++;
                                    WorkDaysCount = days - DayOffCount;
                                }
                                else if (holiday.TypeOfHoliday == "Праздник")
                                {
                                    ucDays.holiday(date);
                                    string d = date.DayOfWeek.ToString("d");
                                    if (d != "6" && d != "0")
                                    {
                                        DayOffCount++;//если не выходной
                                    }
                                    HolidaysCount++;
                                    WorkDaysCount = days - DayOffCount;
                                }
                                else if (holiday.TypeOfHoliday == "Внеплановый")
                                {
                                    // DayOffCount++;
                                    DayOffCount++;
                                    UnscheduledCount++;
                                    WorkDaysCount = days - DayOffCount;
                                    ucDays.dayOffNotInPlan(date);
                                }
                                else if (holiday.TypeOfHoliday == "Рабочий в выходной")
                                {
                                    WorkingOnHolidayCount++;
                                    ucDays.dayWork(date);
                                    DayOffCount--;
                                    WorkDaysCount = days - DayOffCount;
                                }
                            }
                        }
                        Days.Add(ucDays);
                    }
                    FullDays.Add(new DayViewModel(Days, WorkDaysCount, DayOffCount, HolidaysCount, WorkingOnHolidayCount, UnscheduledCount));
                    int countRows = 5;
                    if (Days.Count == 31 && daysOfWeek >= 5)
                    {
                        countRows = 6;
                    }
                    FullYear.Add(new CalendarViewModel(FullDays, daysOfWeek, countRows));
                    Year.Add(Days);
                }
                PaintButtons();
            });
        }


        #endregion OnStartup

        #region Utils

        public void ShowAlert(string alert)
        {
            _sampleError.ErrorName.Text = alert;
            Task<object> result = DialogHost.Show(_sampleError, "RootDialog", ExtendedClosingEventHandler);
        }

        private string getDayAddition(int days)
        {
            if (days < 0)
            {
                days *= -1;
            }
            int preLastDigit = days % 100 / 10;

            if (preLastDigit == 1)
            {
                return days + " Дней";
            }

            switch (days % 10)
            {
                case 1:
                    return days + " День";
                case 2:
                case 3:
                case 4:
                    return days + " Дня";
                default:
                    return days + " Дней";
            }

        }
        public Range<DateTime> ReturnRange(Vacation Item)
        {
            Range<DateTime> range;

            if (Item.Date_end > Item.Date_Start)
            {
                range = Item.Date_Start.To(Item.Date_end);
            }
            else
            {
                range = Item.Date_end.To(Item.Date_Start);
            }
            return range;
        }
        #endregion Utils
    }
}
