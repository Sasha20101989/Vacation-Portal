using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using MiscUtil.Collections;
using MiscUtil.Collections.Extensions;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.Views;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages
{
    public class PersonalVacationPlanningViewModel : ViewModelBase
    {
        #region Props
        private Lazy<Task> _initializeLazy;

        public BindingList<DateTime> Holidays { get; set; } = new BindingList<DateTime>();
        public BindingList<DateTime> Weekends { get; set; } = new BindingList<DateTime>();
        public BindingList<Calendar> Calendars { get; set; } = new BindingList<Calendar>();

        public BindingList<DayBlankControl> BlankDays { get; set; } = new BindingList<DayBlankControl>();
        public List<DayControl> DaysJanuary { get; set; } = new List<DayControl>();
        public List<DayControl> DaysFebruary { get; set; } = new List<DayControl>();
        public List<DayControl> DaysMath { get; set; } = new List<DayControl>();
        public List<DayControl> DaysApril { get; set; } = new List<DayControl>();
        public List<DayControl> DaysMay { get; set; } = new List<DayControl>();
        public List<DayControl> DaysJune { get; set; } = new List<DayControl>();
        public List<DayControl> DaysJuly { get; set; } = new List<DayControl>();
        public List<DayControl> DaysAugust { get; set; } = new List<DayControl>();
        public List<DayControl> DaysSeptember { get; set; } = new List<DayControl>();
        public List<DayControl> DaysOktober { get; set; } = new List<DayControl>();
        public List<DayControl> DaysNovember { get; set; } = new List<DayControl>();
        public List<DayControl> DaysDecember { get; set; } = new List<DayControl>();
        public List<List<DayControl>> Year;
        public int ColumnOfWeek { get; set; }
        public int SelectedDay { get; set; }
        public int SelectedMonth { get; set; }
        public DateTime Day { get; set; }
        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }
        public DateTime FirstSelectedDate { get; set; }
        public DateTime SecondSelectedDate { get; set; }
        private List<Vacation> _selectedVacation;
        public List<Vacation> SelectedVacation
        {
            get { return _selectedVacation; }
            set
            {
                _selectedVacation = value;
                OnPropertyChanged(nameof(SelectedVacation));
            }
        }
        private ObservableCollection<Vacation> _vacationsToAproval;
        public ObservableCollection<Vacation> VacationsToAproval
        {
            get { return _vacationsToAproval; }
            set
            {
                _vacationsToAproval = value;
                OnPropertyChanged(nameof(VacationsToAproval));
            }
        }

        private ObservableCollection<Vacation> _selectedDates;
        public ObservableCollection<Vacation> SelectedDates
        {
            get { return _selectedDates; }
            set
            {
                _selectedDates = value;
                OnPropertyChanged(nameof(SelectedDates));
            }
        }

        public BindingList<SelectedGap> DisplayedDates { get; set; } = new BindingList<SelectedGap>();
        //private ObservableCollection<SelectedGap> _displayedDates;
        //public ObservableCollection<SelectedGap> DisplayedDates
        //{
        //    get { return _displayedDates; }
        //    set
        //    {
        //        _displayedDates = value;
        //        OnPropertyChanged(nameof(DisplayedDates));
        //        if (DisplayedDates.Count==0)
        //        {
        //            IsGapVisible = false;
        //        }
        //        else
        //        {
        //            IsGapVisible = true;
        //        }
        //    }
        //}

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

        private int _clicksOnCalendar;
        public int ClicksOnCalendar
        {
            get { return _clicksOnCalendar; }
            set
            {
                _clicksOnCalendar = value;
                OnPropertyChanged(nameof(ClicksOnCalendar));

            }
        }

        private List<Vacation> _vacationTypes;
        public List<Vacation> VacationTypes
        {
            get { return _vacationTypes; }
            set
            {
                _vacationTypes = value;
                OnPropertyChanged(nameof(VacationTypes));
            }
        }

        
        private Thickness _recomendedMarginButton;
        public Thickness RecomendedMarginButton
        {
            get
            {
                return _recomendedMarginButton;
            }
            set
            {
                _recomendedMarginButton = value;
                OnPropertyChanged(nameof(RecomendedMarginButton));
            }
        }
        private Thickness _recomendedMarginBorderVacation;
        public Thickness RecomendedMarginBorderVacation
        {
            get
            {
                return _recomendedMarginBorderVacation;
            }
            set
            {
                _recomendedMarginBorderVacation = value;
                OnPropertyChanged(nameof(RecomendedMarginBorderVacation));
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

        private string _dayAddition;
        public string DayAddition
        {
            get
            {
                return _dayAddition;
            }
            set
            {
                _dayAddition = value;
                OnPropertyChanged(nameof(Person));
            }
        }

        private string _dayString;
        public string DayString
        {
            get
            {
                return _dayString;
            }
            set
            {
                _dayString = value;
                OnPropertyChanged(nameof(DayString));
            }
        }

        private int _countSelectedDays;
        public int CountSelectedDays
        {
            get => _countSelectedDays;
            set
            {
                SetProperty(ref _countSelectedDays, value);
                OnPropertyChanged(nameof(CountSelectedDays));
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
        private SnackbarMessageQueue _messageQueueVacation;
        public SnackbarMessageQueue MessageQueueVacation
        {
            get => _messageQueueVacation;
            set
            {
                SetProperty(ref _messageQueueVacation, value);
                OnPropertyChanged(nameof(MessageQueueVacation));
            }
        }

        private SnackbarMessageQueue _messageQueueCalendar;
        public SnackbarMessageQueue MessageQueueCalendar
        {
            get => _messageQueueCalendar;
            set
            {
                SetProperty(ref _messageQueueCalendar, value);
                OnPropertyChanged(nameof(MessageQueueCalendar));
            }
        }

        private SnackbarMessageQueue _messageQueueSelectedGap;
        public SnackbarMessageQueue MessageQueueSelectedGap
        {
            get => _messageQueueSelectedGap;
            set
            {
                SetProperty(ref _messageQueueSelectedGap, value);
                OnPropertyChanged(nameof(MessageQueueSelectedGap));
            }
        }

        private SnackbarMessageQueue _messageQueuePLanedVacations;
        public SnackbarMessageQueue MessageQueuePLanedVacations
        {
            get => _messageQueuePLanedVacations;
            set
            {
                SetProperty(ref _messageQueuePLanedVacations, value);
                OnPropertyChanged(nameof(MessageQueuePLanedVacations));
            }
        }
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
        #endregion Commands

        #region Constructor
        public PersonalVacationPlanningViewModel(Person person)
        {
            _person = person;
            PersonName = _person.ToString();

            _initializeLazy = new Lazy<Task>(() => Initialize());
            _currentDate = DateTime.Now;
            LoadModel = new LoadModelCommand(this);
            SaveDataModel = new SaveDataModelCommand(this);
            StartLearning = new StartLearningCommand(this);
            AddToApprovalList = new AddToApprovalListCommand(this);

            _selectedDates = new ObservableCollection<Vacation>();
            _selectedVacation = new List<Vacation>();
            _vacationsToAproval = new ObservableCollection<Vacation>();

            //_weekends = new ObservableCollection<DateTime>();
            //_holidays = new ObservableCollection<DateTime>();
            _vacationTypes = new List<Vacation>();
            LoadModel.Execute(new object());
        }

        private void addToMonth(int month, DayControl ucDays)
        {
            switch (month)
            {
                case 1:
                    DaysJanuary.Add(ucDays);
                    break;
                case 2:
                    DaysFebruary.Add(ucDays);
                    break;
                case 3:
                    DaysMath.Add(ucDays);
                    break;
                case 4:
                    DaysApril.Add(ucDays);
                    break;
                case 5:
                    DaysMay.Add(ucDays);
                    break;
                case 6:
                    DaysJune.Add(ucDays);
                    break;
                case 7:
                    DaysJuly.Add(ucDays);
                    break;
                case 8:
                    DaysAugust.Add(ucDays);
                    break;
                case 9:
                    DaysSeptember.Add(ucDays);
                    break;
                case 10:
                    DaysOktober.Add(ucDays);
                    break;
                case 11:
                    DaysNovember.Add(ucDays);
                    break;
                case 12:
                    DaysDecember.Add(ucDays);
                    break;
            }
        }

        private List<DayControl> getMonth(int month)
        {
            List<DayControl> result = null;
            switch (month)
            {
                case 1:
                     result = DaysJanuary;
                    break;
                case 2:
                    result = DaysFebruary;
                    break;
                case 3:
                    result=DaysMath;
                    break;
                case 4:
                    result=DaysApril;
                    break;
                case 5:
                    result=DaysMay;
                    break;
                case 6:
                    result=DaysJune;
                    break;
                case 7:
                    result=DaysJuly;
                    break;
                case 8:
                    result=DaysAugust;
                    break;
                case 9:
                    result=DaysSeptember;
                    break;
                case 10:
                    result=DaysOktober;
                    break;
                case 11:
                   result= DaysNovember;
                    break;
                case 12:
                    result=DaysDecember;
                    break;
            }
            return result;
        }

        private void UcDays_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (e.OriginalSource is TextBlock)
            {
                ClicksOnCalendar++;
                TextBlock obj = e.OriginalSource as TextBlock;
               
                SelectedDay = Convert.ToInt32(obj.Text);
                SelectedMonth = Convert.ToInt32(obj.Tag);
                DateTime newDate = new DateTime();

                if (ClicksOnCalendar == 3)
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
                    DayAddition = getDayAddition(CountSelectedDays);
                    DisplayedDateString = DayAddition + ": " + FirstSelectedDate.ToString("d.MM.yyyy");
                    SelectedVacation.Clear();
                    _plannedItem = new Vacation(SelectedItem.Name, CountSelectedDays, SelectedItem.Color, FirstSelectedDate, FirstSelectedDate);
                    //SelectedItem.Count = CountSelectedDays;
                    //Vacation vacation = new Vacation(SelectedItem.Name,CountSelectedDays, SelectedItem.Color, FirstSelectedDate, FirstSelectedDate);
                }
                else
                {
                    SecondSelectedDate = new DateTime(CurrentDate.Year, SelectedMonth, SelectedDay);
                    if (SecondSelectedDate > FirstSelectedDate)
                    {
                        CountSelectedDays = SecondSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                        DayAddition = getDayAddition(CountSelectedDays);
                        DisplayedDateString = DayAddition + ": " + FirstSelectedDate.ToString("dd.MM.yyyy") + " - " + SecondSelectedDate.ToString("dd.MM.yyyy");
                        _plannedItem = new Vacation(SelectedItem.Name, CountSelectedDays, SelectedItem.Color, FirstSelectedDate, SecondSelectedDate);
                        PrevPlannedItem = PlannedItem;
                    }
                    else
                    {
                        CountSelectedDays = FirstSelectedDate.Subtract(SecondSelectedDate).Days + 1;
                        DayAddition = getDayAddition(CountSelectedDays);
                        DisplayedDateString = DayAddition + ": " + SecondSelectedDate.ToString("dd.MM.yyyy") + " - " + FirstSelectedDate.ToString("dd.MM.yyyy");
                        _plannedItem = new Vacation(SelectedItem.Name, CountSelectedDays, SelectedItem.Color, SecondSelectedDate, FirstSelectedDate);
                        PrevPlannedItem = PlannedItem;
                    }
                    SelectedVacation.Clear();

                    //SelectedItem.Count = CountSelectedDays;
                    //Vacation vacation = new Vacation(SelectedItem.Name, CountSelectedDays, SelectedItem.Color, FirstSelectedDate, SecondSelectedDate);
                }
                SelectedVacation.Add(SelectedItem);

                blockAndPaintButtons();
                
            }
        }

        private void clearColorAndBlocked()
        {
            foreach (List<DayControl> month in Year)
            {
                foreach (DayControl item in month)
                {
                    Grid parentItem = item.Content as Grid;
                    UIElementCollection buttons = parentItem.Children as UIElementCollection;
                    foreach (var elem in buttons)
                    {
                        Button button = elem as Button;
                        if (!VacationsToAproval.Contains(PrevPlannedItem) && PrevPlannedItem!=null)
                        {
                            button.Background = Brushes.Transparent;
                            button.IsEnabled = true;
                        }
                    }
                }
            }
        }

        private void blockAndPaintButtons()
        {
            Range<DateTime> range;
            if (PlannedItem.Date_end> PlannedItem.Date_Start)
            {
                range = PlannedItem.Date_Start.To(PlannedItem.Date_end);
            }
            else
            {
                range = PlannedItem.Date_end.To(PlannedItem.Date_Start);
            }

            foreach (DateTime date in range.Step(x => x.AddDays(1)))
            {
                foreach (List<DayControl> month in Year)
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
                            if (date.Day == buttonDay && date.Month == buttonMonth)
                            {
                                button.IsEnabled = false;
                                button.Background = PlannedItem.Color;
                            }
                        }
                    }
                }
            }
        }

        private void clearVacationData() {
            SelectedVacation.Clear();
            DisplayedDateString = "";
            ClicksOnCalendar = 0;
            clearColorAndBlocked();
        }

        #endregion Constructor

        #region Task Lazy
        public async Task Initialize()
        {
            await Task.Delay(100);
            CheckRang().Await();
            RenderCalendars().Await();
            SetWindowSize(SystemParameters.FullPrimaryScreenWidth).Await();
        }

        public async Task Load()
        {
            try
            {
                await _initializeLazy.Value;
            }
            catch (Exception)
            {
                _initializeLazy = new Lazy<Task>(() => Initialize());
                throw;
            }
        }
        #endregion Task Lazy

        private async Task CheckRang()
        {
            await Task.Delay(100);

            if (Person.Is_Supervisor)
            {
                IsSupervisor = true;
            }
            else if (Person.Is_HR)
            {
                IsHR = true;
            }
            else
            {
                IsEmployee = true;
            }
            //VacationTypes = await _метод получения данных
            //Holidays = await _метод получения данных
            //Weekends = await _метод получения данных

            loadVacationTypes();
        }

        private void loadVacationTypes()
        {
            var converter = new System.Windows.Media.BrushConverter();
            VacationTypes.Add(new Vacation("Основной", 28, (Brush)converter.ConvertFromString("#9B4F2D"), DateTime.Now, DateTime.Now));
            VacationTypes.Add(new Vacation("Ненормированность", 3, (Brush)converter.ConvertFromString("#2D6D9B"), DateTime.Now, DateTime.Now));
            VacationTypes.Add(new Vacation("Вредность", 0, (Brush)converter.ConvertFromString("#9B2D84"), DateTime.Now, DateTime.Now));
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

        private async Task SetWindowSize(double fullPrimaryScreenWidth)
        {
            await Task.Delay(100);

            RecomendedMarginButton = fullPrimaryScreenWidth > 1600 ? new Thickness(0, 0, 0, 0) : new Thickness(0, 0, 0, 0);
            RecomendedMarginBorderVacation = fullPrimaryScreenWidth > 1600 ? new Thickness(0, 0, 0, 0) : new Thickness(0, 0, 0, 0);
        }
        private async Task RenderCalendars()
        {
            await Task.Delay(100);

            Holidays.Add(DateTime.Today.AddDays(-7));
            //Weekends.Add(DateTime.Today.AddDays(1));

            Year = new List<List<DayControl>>();

            for (int j = 1; j <= 12; j++)
            {
                DateTime startOftheMonth1 = new DateTime(CurrentDate.Year, j, 1);
                int days = DateTime.DaysInMonth(CurrentDate.Year, j);
                int daysOfWeek = Convert.ToInt32(CurrentDate.DayOfWeek.ToString("d")) - 1;
                for (int i = 0; i < daysOfWeek; i++)
                {
                    DayBlankControl dayBlank = new DayBlankControl();
                    BlankDays.Add(dayBlank);
                }
                List<DayControl> Days = new List<DayControl>();
                for (int i = 1; i <= days; i++)
                {
                    DayControl ucDays = new DayControl();
                    DateTime date = new DateTime(CurrentDate.Year, j, i);
                    Day = date;
                    ucDays.days(date);
                    ucDays.PreviewMouseLeftButtonDown += UcDays_PreviewMouseLeftButtonDown;
                    addToMonth(j, ucDays);
                    Days.Add(ucDays);
                }
                Year.Add(Days);
            }
        }

        public string getDayAddition(int days)
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

    }
}
