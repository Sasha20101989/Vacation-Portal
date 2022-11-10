using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using MaterialDesignThemes.Wpf;
using Vacation_Portal.Commands.BaseCommands;
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

        private ObservableCollection<DateTime> _holidays;
        public ObservableCollection<DateTime> Holidays
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

        private ObservableCollection<DateTime> _weekends;
        public ObservableCollection<DateTime> Weekends
        {
            get
            {
                return _weekends;
            }
            set
            {
                _weekends = value;
                OnPropertyChanged(nameof(Weekends));
            }
        }

        public ObservableCollection<Calendar> Calendars { get; set; }

        private ObservableCollection<DateTime> _selectedDates;
        public ObservableCollection<DateTime> SelectedDates
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
                if (DisplayedDateString!="")
                {
                    IsGapVisible = true;
                }
                else
                {
                    IsGapVisible = false;
                }
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

        private string _personName = string.Empty;
        public string PersonName
        {
            get
            {
                return _personName;
            }
            set
            {
                _personName = value;
                OnPropertyChanged(nameof(PersonName));

            }
        }

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
                OnPropertyChanged(nameof(PersonName));
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

        private Vacation _selectedItem;
        public Vacation SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnPropertyChanged(nameof(SelectedItem));
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
        #endregion BorderLern

        #endregion Props

        #region Commands
        public AnotherCommandImplementation LoadModelCommand { get; set; }
        public AnotherCommandImplementation SaveCommand { get; set; }
        public AnotherCommandImplementation StartLearning { get; set; }
        public AnotherCommandImplementation AddToApprovalList { get; set; }
        #endregion Commands

        #region Constructor
        public PersonalVacationPlanningViewModel(Person person)
        {
            MessageQueueVacation = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
            MessageQueueCalendar = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(11000));
            MessageQueueSelectedGap = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(11000));

            _initializeLazy = new Lazy<Task>(() => Initialize(person));
            _isEnabled = true;

            LoadModelCommand = new AnotherCommandImplementation(
                async _ =>
                {
                    await Load(person);
                });

            SaveCommand = new AnotherCommandImplementation(_ =>
            {
                if (IsSaveComplete)
                {
                    IsSaveComplete = false;
                    return;
                }
                if (SaveProgress != 0)
                {
                    return;
                }
                var started = DateTime.Now;
                IsSaving = true;

                new DispatcherTimer(
                    TimeSpan.FromMilliseconds(50),
                    DispatcherPriority.Normal,
                    new EventHandler((o, e) =>
                    {
                        var totalDuration = started.AddSeconds(3).Ticks - started.Ticks;
                        var currentProgress = DateTime.Now.Ticks - started.Ticks;
                        var currentProgressPercent = 100.0 / totalDuration * currentProgress;

                        SaveProgress = currentProgressPercent;

                        if (SaveProgress >= 100)
                        {
                            IsSaveComplete = true;
                            IsSaving = false;
                            SaveProgress = 0;
                            if (o is DispatcherTimer timer)
                            {
                                timer.Stop();
                            }
                        }
                    }), Dispatcher.CurrentDispatcher);
            });

            StartLearning = new AnotherCommandImplementation(_ =>
            {
                LernVacation();
            });

            AddToApprovalList = new AnotherCommandImplementation(_ =>
            {
                foreach (var item in SelectedDates)
                {
                    Console.WriteLine(item);
                }
                clearSelectedDates();
            });

            _selectedDates = new ObservableCollection<DateTime>();
            //_displayedDates = new ObservableCollection<SelectedGap>();
            _weekends = new ObservableCollection<DateTime>();
            _holidays = new ObservableCollection<DateTime>();
            _vacationTypes = new List<Vacation>();
            LoadModelCommand.Execute(new object());
            DisplayedDates.ListChanged += DisplayedDatesonListChanged; ;
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
        private void LernVacation()
        {
            IsEnabled = false;
            var started = DateTime.Now;
            string message = "Выберете отпуск который хотите запланировать.";

            new DispatcherTimer(
            TimeSpan.FromMilliseconds(50),
            DispatcherPriority.Normal,
            new EventHandler((o, e) =>
            {
                var totalDuration = started.AddMilliseconds(5000).Ticks - started.Ticks;
                var currentProgress = DateTime.Now.Ticks - started.Ticks;
                var currentProgressPercent = 100.0 / totalDuration * currentProgress;

                LearningProgress = currentProgressPercent;
                var converter = new System.Windows.Media.BrushConverter();
                BorderColorVacation = (Brush)converter.ConvertFromString("#FF0000");
                Task.Factory.StartNew(() => MessageQueueVacation.Enqueue(message));

                if (LearningProgress >= 100)
                {
                    BorderColorVacation = Brushes.Transparent;
                    LearningProgress = 0;
                    MessageQueueVacation.Clear();

                    if (o is DispatcherTimer timer)
                    {
                        timer.Stop();
                    }
                    LernCalendar();
                }
            }), Dispatcher.CurrentDispatcher);
        }

        private void LernCalendar()
        {
            Task.Delay(1500);
            var started = DateTime.Now;
            string message = "Выберете кликом в календаре, первый и последний день промежутка, который хотите запланировать.";

            new DispatcherTimer(
            TimeSpan.FromMilliseconds(50),
            DispatcherPriority.Normal,
            new EventHandler((o, e) =>
            {
                var totalDuration = started.AddMilliseconds(11000).Ticks - started.Ticks;
                var currentProgress = DateTime.Now.Ticks - started.Ticks;
                var currentProgressPercent = 100.0 / totalDuration * currentProgress;

                LearningProgress = currentProgressPercent;
                var converter = new System.Windows.Media.BrushConverter();
                BorderColorCalendar = (Brush)converter.ConvertFromString("#FF0000");
                Task.Factory.StartNew(() => MessageQueueCalendar.Enqueue(message));

                if (LearningProgress >= 100)
                {
                    BorderColorCalendar = Brushes.Transparent;
                    LearningProgress = 0;
                    MessageQueueCalendar.Clear();

                    if (o is DispatcherTimer timer)
                    {
                        timer.Stop();
                    }
                    LearnViewPlanedDays();
                }
            }), Dispatcher.CurrentDispatcher);
        }

        private void LearnViewPlanedDays()
        {

            var started = DateTime.Now;
            string message = "Убедитесь в том что вид отпуска, даты и колличество совпадет с вашим выбором и подтвердите кнопкой.";

            new DispatcherTimer(
            TimeSpan.FromMilliseconds(50),
            DispatcherPriority.Normal,
            new EventHandler((o, e) =>
            {
                var totalDuration = started.AddMilliseconds(11000).Ticks - started.Ticks;
                var currentProgress = DateTime.Now.Ticks - started.Ticks;
                var currentProgressPercent = 100.0 / totalDuration * currentProgress;

                LearningProgress = currentProgressPercent;
                var converter = new System.Windows.Media.BrushConverter();
                BorderColorSelectedGap = (Brush)converter.ConvertFromString("#FF0000");
                Task.Factory.StartNew(() => MessageQueueSelectedGap.Enqueue(message));

                if (LearningProgress >= 100)
                {
                    BorderColorSelectedGap = Brushes.Transparent;
                    LearningProgress = 0;
                    MessageQueueSelectedGap.Clear();
                    IsEnabled = true;
                    if (o is DispatcherTimer timer)
                    {
                        timer.Stop();
                    }
                }
            }), Dispatcher.CurrentDispatcher);
        }

        private void LearnCalendar()
        {

        }
        #endregion Lerns

        #region Task Lazy
        public async Task Initialize(Person person)
        {
            await Task.Delay(100);
            CheckRang(person).Await();
            RenderCalendars().Await();
            SetWindowSize(SystemParameters.FullPrimaryScreenWidth).Await();
        }

        public async Task Load(Person person)
        {
            try
            {
                await _initializeLazy.Value;
            }
            catch (Exception)
            {
                _initializeLazy = new Lazy<Task>(() => Initialize(person));
                throw;
            }
        }
        #endregion Task Lazy

        private async Task CheckRang(Person person)
        {
            await Task.Delay(100);

            if (person.Is_Supervisor)
            {
                IsSupervisor = true;
            }
            else if (person.Is_HR)
            {
                IsHR = true;
            }
            else
            {
                IsEmployee = true;
                AddPersonNameInView(person.ToString()).Await();
            }
            //VacationTypes = await _метод получения данных
            //Holidays = await _метод получения данных
            //Weekends = await _метод получения данных

            loadVacationTypes(person);
        }

        private void loadVacationTypes(Person person)
        {
            var converter = new System.Windows.Media.BrushConverter();
            VacationTypes.Add(new Vacation("Основной", "28", (Brush)converter.ConvertFromString("#9B4F2D")));
            VacationTypes.Add(new Vacation("Ненормированность", "3", (Brush)converter.ConvertFromString("#2D6D9B")));
            VacationTypes.Add(new Vacation("Вредность", "0", (Brush)converter.ConvertFromString("#9B2D84")));
            VacationTypes.Add(new Vacation("Стаж", "2", (Brush)converter.ConvertFromString("#9B8C2D")));
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

            Calendars = new ObservableCollection<Calendar>();

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

                calendar.SelectedDatesChanged += Calendar_SelectedDatesChanged;

                Calendars.Add(calendar);
            }

        }
        private async Task AddPersonNameInView(string name)
        {
            await Task.Delay(100);
            PersonName = name;
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
            if (calendar.SelectedDate != null)
            {
                DateTime startDate = (DateTime)calendar.SelectedDate;
                if (!SelectedDates.Contains(startDate) && SelectedDates.Count != 2)
                {
                    SelectedDates.Add(startDate);
                    DayString = SelectedDates[0].ToString("dd.MM.yyyy");
                    if (DisplayedDates.Count < 1)
                    {
                        DisplayedDates.Add(new SelectedGap(getDayAddition(SelectedDates.Count), SelectedDates[0], SelectedDates[0]));
                    }

                    

                    if (SelectedDates.Count == 2)
                    {
                        AddDaysOnUI(calendar);
                    }
                }
            }
        }
        private void clearSelectedDates()
        {
            foreach (Calendar calendarItem in Calendars)
            {
                int calendarMonth = calendarItem.DisplayDate.Month;

                if (calendarMonth != SelectedDates[0].Month)
                {
                    calendarItem.SelectedDates.Clear();
                }
                if (SelectedDates.Count > 1)
                {
                    if (calendarMonth != SelectedDates[1].Month && calendarMonth != SelectedDates[0].Month)
                    {
                        calendarItem.SelectedDates.Clear();
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
            List<DateTime> firstMonth = new List<DateTime>();
            List<DateTime> secondMonth = new List<DateTime>();
            SelectedDates.Sort((a, b) => { return a.CompareTo(b); });
            int countDays = SelectedDates[1].Subtract(SelectedDates[0]).Days;

            DayAddition = getDayAddition(countDays + 1);
            DisplayedDates[DisplayedDates.Count - 1] = new SelectedGap(DayAddition,SelectedDates[0], SelectedDates[1]);

            ObservableCollection<DateTime> preDatesWeekend = new ObservableCollection<DateTime>();

            for (int i = 0; i <= countDays; i++)
            {
                if (SelectedDates[0].Month == SelectedDates[0].AddDays(i).Month)
                {
                    firstMonth.Add(SelectedDates[0].AddDays(i));
                    preDatesWeekend.Add(SelectedDates[0].AddDays(i));
                    Weekends = preDatesWeekend;
                }
                else
                {
                    secondMonth.Add(SelectedDates[0].AddDays(i));
                    preDatesWeekend.Add(SelectedDates[0].AddDays(i));
                    Weekends = preDatesWeekend;
                }
            }

            foreach (Calendar calendarItem in Calendars)
            {
                int calendarMonth = calendarItem.DisplayDate.Month;

                if (calendarMonth == firstMonth[0].Month)
                {
                    //calendarItem.BlackoutDates.Add(new CalendarDateRange(firstMonth[0], firstMonth[firstMonth.Count - 1]));
                    calendarItem.SelectedDates.AddRange(firstMonth[0], firstMonth[firstMonth.Count - 1]);

                }
                if (secondMonth.Count > 0 && calendarMonth == secondMonth[0].Month)
                {
                    //calendarItem.BlackoutDates.Add(new CalendarDateRange(secondMonth[0], secondMonth[secondMonth.Count - 1]));
                    calendarItem.SelectedDates.AddRange(secondMonth[0], secondMonth[secondMonth.Count - 1]);
                }
            }

            SelectedDates.Clear();
        }

    }
}
