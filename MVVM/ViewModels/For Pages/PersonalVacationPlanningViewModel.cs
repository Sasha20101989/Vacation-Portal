using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
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
        private List<Vacation> _vacationTypess;
        public List<Vacation> VacationTypess
        {
            get { return _vacationTypess; }
            set
            {
                _vacationTypess = value;
                OnPropertyChanged(nameof(VacationTypess));
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
                clearCalendar();
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
            //_displayedDates = new ObservableCollection<SelectedGap>();
            //_weekends = new ObservableCollection<DateTime>();
            //_holidays = new ObservableCollection<DateTime>();
            _vacationTypes = new List<Vacation>();
            _vacationTypess = new List<Vacation>();
            LoadModel.Execute(new object());
            DisplayedDates.ListChanged += DisplayedDatesonListChanged;
            Calendars.ListChanged += Calendars_ListChanged;
            SelectedDatesChanged += PersonalVacationPlanningViewModel_SelectedDatesChanged;

            Year = new List<List<DayControl>>();
            //for (int i = 0; i < 12; i++)
            //{
            //    DateTime startOftheYear = new DateTime(DateTime.Now.Year, i+1, 1);
            //}
            //DateTime now = DateTime.Now;
            //DateTime startOftheMonth = new DateTime(now.Year, now.Month, 1);
            //int days = DateTime.DaysInMonth(now.Year, now.Month);
            //int daysOfWeek = Convert.ToInt32(startOftheMonth.DayOfWeek.ToString("d")) - 1;
            //ColumnOfWeek = daysOfWeek;
            //for (int i = 0; i < daysOfWeek; i++)
            //{
            //    DayBlankControl dayBlank = new DayBlankControl();
            //    BlankDays.Add(dayBlank);
            //}
            //for (int i = 1; i <= days; i++)
            //{
            //    DayControl ucDays = new DayControl();
            //    ucDays.days(i);
            //}
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
                for (int i = 1; i <= days; i++)
                {
                    DayControl ucDays = new DayControl();
                    DateTime date = new DateTime(CurrentDate.Year, j, i);
                    Day = date;
                    ucDays.days(date);
                    ucDays.PreviewMouseLeftButtonDown += UcDays_PreviewMouseLeftButtonDown;
                    switch (j)
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
            }
        }

        private void UcDays_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is TextBlock)
            {
                ClicksOnCalendar++;
                TextBlock obj = e.OriginalSource as TextBlock;
                int day = Convert.ToInt32(obj.Text);
                int month = Convert.ToInt32(obj.Tag);
                DateTime newDate = new DateTime();
                if (ClicksOnCalendar == 3)
                {
                    FirstSelectedDate = newDate;
                    SecondSelectedDate = newDate;
                    ClicksOnCalendar = 1;
                }

                if (ClicksOnCalendar == 1)
                {
                    FirstSelectedDate = new DateTime(CurrentDate.Year, month, day);
                    CountSelectedDays = FirstSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                    DayAddition = getDayAddition(CountSelectedDays);
                    DisplayedDateString = DayAddition +": " + FirstSelectedDate.ToString("d-MM-yyyy");
                }
                else
                {
                    SecondSelectedDate = new DateTime(CurrentDate.Year, month, day);
                    CountSelectedDays = SecondSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                    DayAddition = getDayAddition(CountSelectedDays);
                    if (SecondSelectedDate>FirstSelectedDate)
                    {
                        DisplayedDateString = DayAddition + ": " + FirstSelectedDate.ToString("dd.MM.yyyy") + " - " + SecondSelectedDate.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        DisplayedDateString = DayAddition + ": " + SecondSelectedDate.ToString("dd.MM.yyyy") + " - " + FirstSelectedDate.ToString("dd.MM.yyyy");
                    }
                }
                
            }
        }

        private void PersonalVacationPlanningViewModel_SelectedDatesChanged(Calendar obj)
        {

        }

        private void Calendars_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {

            }

            if (e.ListChangedType == ListChangedType.ItemAdded)
            {

            }
        }


        #endregion Constructor

        private void DisplayedDatesonListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemChanged)
            {
                DisplayedDateString = DisplayedDates[0].ToString();
            }

            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                string str = DisplayedDates[0].ToString();
                int position = str.LastIndexOf("-");
                DisplayedDateString = str.Substring(0, position - 1);
            }
        }

        #region Lerns

        #endregion Lerns

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

            VacationTypess.Add(new Vacation("Основной", 28, (Brush)converter.ConvertFromString("#9B4F2D"), DateTime.Now, DateTime.Now));
            VacationTypess.Add(new Vacation("Ненормированность", 3, (Brush)converter.ConvertFromString("#2D6D9B"), DateTime.Now, DateTime.Now));
            VacationTypess.Add(new Vacation("Вредность", 0, (Brush)converter.ConvertFromString("#9B2D84"), DateTime.Now, DateTime.Now));
            VacationTypess.Add(new Vacation("Стаж", 2, (Brush)converter.ConvertFromString("#9B8C2D"), DateTime.Now, DateTime.Now));
            VacationTypess.Add(new Vacation("Основной", 28, (Brush)converter.ConvertFromString("#9B4F2D"), DateTime.Now, DateTime.Now));
            VacationTypess.Add(new Vacation("Ненормированность", 3, (Brush)converter.ConvertFromString("#2D6D9B"), DateTime.Now, DateTime.Now));
            VacationTypess.Add(new Vacation("Вредность", 0, (Brush)converter.ConvertFromString("#9B2D84"), DateTime.Now, DateTime.Now));
            VacationTypess.Add(new Vacation("Стаж", 2, (Brush)converter.ConvertFromString("#9B8C2D"), DateTime.Now, DateTime.Now));
            VacationTypess.Add(new Vacation("Основной", 28, (Brush)converter.ConvertFromString("#9B4F2D"), DateTime.Now, DateTime.Now));
            VacationTypess.Add(new Vacation("Ненормированность", 3, (Brush)converter.ConvertFromString("#2D6D9B"), DateTime.Now, DateTime.Now));
            VacationTypess.Add(new Vacation("Вредность", 0, (Brush)converter.ConvertFromString("#9B2D84"), DateTime.Now, DateTime.Now));
            VacationTypess.Add(new Vacation("Стаж", 2, (Brush)converter.ConvertFromString("#9B8C2D"), DateTime.Now, DateTime.Now));
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

            //Calendars = new BindingList<Calendar>();

            for (int i = 0; i < 12; i++)
            {
                Calendar calendar = new Calendar
                {
                    DisplayDate = new DateTime(DateTime.Now.Year, i + 1, DateTime.DaysInMonth(DateTime.Now.Year, i + 1)),
                    Style = (Style)App.Current.FindResource("CalendarStyle1"),
                    CalendarDayButtonStyle = (Style)App.Current.FindResource("CalendarDayButtonStyle1"),
                    FontFamily = new FontFamily("Trebuchet MS"),
                    SelectionMode = (CalendarSelectionMode)SelectionMode.Multiple,
                    //Margin = new Thickness(2),
                    Name = $"CalendarMonth_{i + 1}",
                };

                calendar.DisplayDateStart = new DateTime(DateTime.Now.Year, i + 1, 1);
                calendar.DisplayDateEnd = new DateTime(DateTime.Now.Year, i + 1, DateTime.DaysInMonth(DateTime.Now.Year, i + 1));

                //calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;
                calendar.GotFocus += Calendar_GotFocus;

                Calendars.Add(calendar);
            }
            
        }

        private void Calendar_GotFocus(object sender, RoutedEventArgs e)
        {
            Button button = e.OriginalSource as Button;
            string dateString = button.DataContext.ToString();
            DateTime date = DateTime.Parse(dateString);

            Vacation newVacation = new Vacation(SelectedItem.Name, SelectedDates.Count, SelectedItem.Color, date, date);
            if (!SelectedDates.Contains(newVacation) && SelectedDates.Count != 2)
            {
                SelectedDates.Add(newVacation);
                CountSelectedDays = SelectedDates[0].Date_Start.Subtract(SelectedDates[0].Date_end).Days + 1;

                if (DisplayedDates.Count < 1)
                {
                    if (CountSelectedDays <= SelectedItem.Count)
                    {
                        DisplayedDates.Add(new SelectedGap(getDayAddition(SelectedDates.Count), SelectedDates[0].Date_Start, SelectedDates[0].Date_Start));
                    }
                }

                if (SelectedDates.Count == 2)
                {
                    List<DateTime> monthFirstSelectedDays = new List<DateTime>();
                    List<DateTime> monthSecondSelectedDays = new List<DateTime>();
                    SelectedDates.Sort((a, b) => { return a.Date_Start.CompareTo(b.Date_Start); });
                    CountSelectedDays = SelectedDates[1].Date_Start.Subtract(SelectedDates[0].Date_end).Days + 1;

                    DayAddition = getDayAddition(CountSelectedDays);

                    if (CountSelectedDays <= SelectedItem.Count)
                    {
                        DisplayedDates[0] = new SelectedGap(DayAddition, SelectedDates[0].Date_Start, SelectedDates[1].Date_end);

                        for (int i = 0; i <= CountSelectedDays - 1; i++)
                        {
                            if (SelectedDates[0].Date_Start.Month == SelectedDates[0].Date_Start.AddDays(i).Month)
                            {
                                monthFirstSelectedDays.Add(SelectedDates[0].Date_Start.AddDays(i));
                            }
                            else
                            {
                                monthSecondSelectedDays.Add(SelectedDates[0].Date_Start.AddDays(i));
                            }
                        }

                        foreach (Calendar calendarItem in Calendars)
                        {
                            int calendarMonth = calendarItem.DisplayDate.Month;

                            if (calendarMonth == monthFirstSelectedDays[0].Month)
                            {
                                //calendarItem.BlackoutDates.Add(new CalendarDateRange(firstMonth[0], firstMonth[firstMonth.Count - 1]));
                                calendarItem.SelectedDates.AddRange(monthFirstSelectedDays[0], monthFirstSelectedDays[monthFirstSelectedDays.Count - 1]);
                                calendarItem.SelectedDatesChanged += CalendarItem_SelectedDatesChanged;
                            }
                            if (monthSecondSelectedDays.Count > 0 && calendarMonth == monthSecondSelectedDays[0].Month)
                            {
                                //calendarItem.BlackoutDates.Add(new CalendarDateRange(secondMonth[0], secondMonth[secondMonth.Count - 1]));
                                calendarItem.SelectedDates.AddRange(monthSecondSelectedDays[0], monthSecondSelectedDays[monthSecondSelectedDays.Count - 1]);
                            }
                        }

                        SelectedDates.Clear();
                    }
                }
            }
        }
        public event Action<Calendar> SelectedDatesChanged;
        private void CalendarItem_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedDatesChanged?.Invoke((Calendar)sender);
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

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Calendar calendar = sender as Calendar;
            //0 1 2
            if (calendar.SelectedDate != null && SelectedDates.Count != 2)
            {
                ClicksOnCalendar++;
                //3
                if (ClicksOnCalendar == 3)
                {
                    DisplayedDates.Clear();
                    ClicksOnCalendar = 1;

                    foreach (Calendar calendarItem in Calendars)
                    {
                        if (calendarItem != calendar)
                        {
                            calendarItem.SelectedDates.Clear();
                        }

                    }
                }
                DateTime startDate = (DateTime)calendar.SelectedDate;
                Vacation newVacation = new Vacation(SelectedItem.Name, SelectedDates.Count, SelectedItem.Color, startDate, startDate);
                //1 2
                if (!SelectedDates.Contains(newVacation) && SelectedDates.Count != 2)
                {
                    SelectedDates.Add(new Vacation(SelectedItem.Name, SelectedDates.Count, SelectedItem.Color, startDate, startDate));

                    //DayString = SelectedDates[0].Date_Start.ToString("dd.MM.yyyy");

                    CountSelectedDays = SelectedDates[0].Date_Start.Subtract(SelectedDates[0].Date_end).Days + 1;
                    //1
                    if (DisplayedDates.Count < 1)
                    {
                        //1
                        if (CountSelectedDays <= SelectedItem.Count)
                        {
                            DisplayedDates.Add(new SelectedGap(getDayAddition(SelectedDates.Count), SelectedDates[0].Date_Start, SelectedDates[0].Date_Start));
                        }
                    }

                    //clearSelectedDates();
                    //2
                    if (SelectedDates.Count == 2)
                    {
                        AddDaysOnUI(calendar);
                    }
                }
            }
        }
        private void clearCalendar()
        {
            foreach (Calendar calendarItem in Calendars)
            {
                calendarItem.SelectedDates.Clear();
            }
            DisplayedDates.Clear();
            SelectedDates.Clear();
            DisplayedDateString = string.Empty;
        }
        private void clearSelectedDates()
        {
            if (SelectedDates.Count > 0)
            {
                foreach (Calendar calendarItem in Calendars)
                {
                    int calendarMonth = calendarItem.DisplayDate.Month;

                    if (calendarMonth != SelectedDates[0].Date_Start.Month)
                    {
                        calendarItem.SelectedDates.Clear();
                    }
                    if (SelectedDates.Count > 1)
                    {
                        if (calendarMonth != SelectedDates[1].Date_Start.Month && calendarMonth != SelectedDates[0].Date_Start.Month)
                        {
                            calendarItem.SelectedDates.Clear();
                        }
                    }
                }
            }
            //foreach (Calendar calendarItem in Calendars)
            //{
            //    int calendarMonth = calendarItem.DisplayDate.Month;

            //    if (calendarMonth != SelectedDates[0].Month)
            //    {
            //        calendarItem.SelectedDates.Clear();
            //    }
            //    if (SelectedDates.Count > 1)
            //    {
            //        if (calendarMonth != SelectedDates[1].Month && calendarMonth != SelectedDates[0].Month)
            //        {
            //            calendarItem.SelectedDates.Clear();
            //        }
            //    }
            //}
        }
        private void AddDaysOnUI(Calendar calendar)
        {
            List<DateTime> firstDay = new List<DateTime>();
            List<DateTime> secondDay = new List<DateTime>();
            SelectedDates.Sort((a, b) => { return a.Date_Start.CompareTo(b.Date_Start); });
            CountSelectedDays = SelectedDates[1].Date_Start.Subtract(SelectedDates[0].Date_end).Days + 1;

            DayAddition = getDayAddition(CountSelectedDays);

            if (CountSelectedDays <= SelectedItem.Count)
            {
                DisplayedDates[0] = new SelectedGap(DayAddition, SelectedDates[0].Date_Start, SelectedDates[1].Date_end);

                //ObservableCollection<DateTime> preDatesWeekend = new ObservableCollection<DateTime>();

                for (int i = 0; i <= CountSelectedDays - 1; i++)
                {
                    if (SelectedDates[0].Date_Start.Month == SelectedDates[0].Date_Start.AddDays(i).Month)
                    {
                        firstDay.Add(SelectedDates[0].Date_Start.AddDays(i));
                        //preDatesWeekend.Add(SelectedDates[0].AddDays(i));
                        //Weekends = preDatesWeekend;
                    }
                    else
                    {
                        secondDay.Add(SelectedDates[0].Date_Start.AddDays(i));
                        // preDatesWeekend.Add(SelectedDates[0].AddDays(i));
                        //Weekends = preDatesWeekend;
                    }
                }

                foreach (Calendar calendarItem in Calendars)
                {
                    int calendarMonth = calendarItem.DisplayDate.Month;

                    if (calendarMonth == firstDay[0].Month)
                    {
                        //calendarItem.BlackoutDates.Add(new CalendarDateRange(firstMonth[0], firstMonth[firstMonth.Count - 1]));
                        calendarItem.SelectedDates.AddRange(firstDay[0], firstDay[firstDay.Count - 1]);

                    }
                    if (secondDay.Count > 0 && calendarMonth == secondDay[0].Month)
                    {
                        //calendarItem.BlackoutDates.Add(new CalendarDateRange(secondMonth[0], secondMonth[secondMonth.Count - 1]));
                        calendarItem.SelectedDates.AddRange(secondDay[0], secondDay[secondDay.Count - 1]);
                    }
                }

                SelectedDates.Clear();
            }
            else
            {
                SelectedDates.Clear();
                DisplayedDates.Clear();
                ClicksOnCalendar = 1;
                DisplayedDateString = string.Empty;
                foreach (Calendar calendarItem in Calendars)
                {
                    if (calendarItem != calendar)
                    {
                        calendarItem.SelectedDates.Clear();
                    }

                }
            }

        }

    }
}
