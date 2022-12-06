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

        public List<bool> WorkingDays = new List<bool>();
        public BindingList<DateTime> DayOffs { get; set; } = new BindingList<DateTime>();

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
        public List<ObservableCollection<DayControl>> Year { get; set; } = new List<ObservableCollection<DayControl>>();
        public ObservableCollection<CalendarViewModel> FullYear { get; set; } = new ObservableCollection<CalendarViewModel>();

        private ObservableCollection<DayControl> _daysJanuary = new ObservableCollection<DayControl>();
        public ObservableCollection<DayControl> DaysJanuary
        {
            get => _daysJanuary;
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
            get => _daysFebruary;
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
            get => _daysMarch;
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
            get => _daysApril;
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
            get => _daysMay;
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
            get => _daysJune;
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
            get => _daysJuly;
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
            get => _daysAugust;
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
            get => _daysSeptember;
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
            get => _daysOktober;
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
            get => _daysNovember;
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
            get => _daysDecember;
            set
            {
                _daysDecember = value;
                OnPropertyChanged(nameof(DaysDecember));

            }
        }
        public int ColumnOfWeekDecember { get; set; }
        public int CountRowsDecember { get; set; }
        #endregion

        private ObservableCollection<Vacation> _vacationsToAproval;
        public ObservableCollection<Vacation> VacationsToAproval
        {
            get => _vacationsToAproval;
            set
            {
                _vacationsToAproval = value;
                OnPropertyChanged(nameof(VacationsToAproval));

            }
        }
        private ObservableCollection<VacationAllowanceViewModel> _vacationAllowances;
        public ObservableCollection<VacationAllowanceViewModel> VacationAllowances
        {
            get => _vacationAllowances;
            set
            {
                _vacationAllowances = value;
                OnPropertyChanged(nameof(VacationAllowances));
            }
        }
        private ObservableCollection<Vacation> _vacationTypes;
        public ObservableCollection<Vacation> VacationTypes
        {
            get => _vacationTypes;
            set
            {
                _vacationTypes = value;
                OnPropertyChanged(nameof(VacationTypes));
            }
        }
        private ObservableCollection<string> _vacationUniqNames = new ObservableCollection<string>();
        public ObservableCollection<string> VacationUniqNames
        {
            get => _vacationUniqNames;
            set
            {
                _vacationUniqNames = value;
                OnPropertyChanged(nameof(VacationUniqNames));
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

        public string PersonName { get; set; }

        private bool _isHR = false;
        public bool IsHR
        {
            get => _isHR;
            set
            {
                _isHR = value;
                OnPropertyChanged(nameof(IsHR));

            }
        }

        private bool _isSupervisor = false;
        public bool IsSupervisor
        {
            get => _isSupervisor;
            set
            {
                _isSupervisor = value;
                OnPropertyChanged(nameof(IsSupervisor));

            }
        }

        private bool _isEmployee = false;
        public bool IsEmployee
        {
            get => _isEmployee;
            set
            {
                _isEmployee = value;
                OnPropertyChanged(nameof(IsEmployee));

            }
        }
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
        private DateTime FirstSelectedDate { get; set; }
        private DateTime SecondSelectedDate { get; set; }
        private string DayAddition { get; set; }
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

        public Vacation UpdatedVacation { get; set; }

        //private Vacation _selectedItem;
        //public Vacation SelectedItem
        //{
        //    get => _selectedItem;
        //    set
        //    {
        //        SetProperty(ref _selectedItem, value);
        //        OnPropertyChanged(nameof(SelectedItem));
        //        if(SelectedItem != null)
        //        {
        //            CalendarClickable = SelectedItem.Count > 0;
        //        }
        //        ClearVacationData();
        //    }
        //}

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
                ClearVacationData();
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _ = SetProperty(ref _selectedIndex, value);
                OnPropertyChanged(nameof(SelectedIndex));
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
        public ICommand LoadModel { get; set; }
        public ICommand SaveDataModel { get; set; }
        public ICommand StartLearning { get; set; }
        public ICommand AddToApprovalList { get; set; }
        public ICommand CancelVacation { get; set; }
        public ICommand CheckVacations { get; set; }
        public AnotherCommandImplementation MovePrevYearCommand { get; set; }
        public AnotherCommandImplementation MoveNextYearCommand { get; set; }
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
        #endregion Commands

        #region Constructor
        public PersonalVacationPlanningViewModel()
        {
            _vacationTypes = new ObservableCollection<Vacation>();
            _vacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
            _vacationsToAproval = new ObservableCollection<Vacation>();
            MessageQueueVacation = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
            MessageQueueCalendar = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
            MessageQueueSelectedGap = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
            MessageQueuePLanedVacations = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000));
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
            LoadModel.Execute(new object());      
            App.API.OnHolidaysChanged += OnHolidaysChanged;
        }
        #endregion Constructor

        #region Calendar Interaction
        private void OnHolidaysChanged(List<HolidayViewModel> obj)
        {
            Holidays = obj;
            RenderCalendar();
        }
        private void UcDays_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                        BlockAndPaintButtons();
                        FirstSelectedDate = newDate;
                        SecondSelectedDate = newDate;
                        ClicksOnCalendar = 1;
                        ClearColor();
                    }

                    if(ClicksOnCalendar == 1)
                    {
                        FirstSelectedDate = new DateTime(SelectedYear, SelectedMonth, SelectedDay);
                        CountSelectedDays = FirstSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                        if(CountSelectedDays <= SelectedItemAllowance.Vacation_Days_Quantity)
                        {
                            if(SelectedNameDay != "Праздник")
                            {
                                DayAddition = GetDayAddition(CountSelectedDays);
                                DisplayedDateString = DayAddition + ": " + FirstSelectedDate.ToString("d.MM.yyyy");
                                _plannedItem = new Vacation(SelectedItemAllowance.Vacation_Name, Person.Id_SAP,SelectedItemAllowance.Vacation_Id, CountSelectedDays, SelectedItemAllowance.Vacation_Color, FirstSelectedDate, FirstSelectedDate,null);
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

                                    DayAddition = GetDayAddition(CountSelectedDays);
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

                                    DayAddition = GetDayAddition(CountSelectedDays);
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

                            Range<DateTime> range = ReturnRange(PlannedItem);
                            List<bool> isGoToNext = new List<bool>();

                            foreach(DateTime planedDate in range.Step(x => x.AddDays(1)))
                            {
                                for(int i = 0; i < VacationsToAproval.Count; i++)
                                {
                                    Vacation existingVacation = VacationsToAproval[i];

                                    Range<DateTime> rangeExistingVacation = ReturnRange(existingVacation); ;

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
                                BlockAndPaintButtons();
                            } else
                            {
                                ShowAlert("Пересечение отпусков не допустимо");
                                BlockAndPaintButtons();
                                FirstSelectedDate = newDate;
                                SecondSelectedDate = newDate;
                                ClicksOnCalendar = 0;
                                CountSelectedDays = 0;
                                DisplayedDateString = "";
                                ClearColor();
                            }
                        }
                    } else
                    {

                        ShowAlert("Выбранный промежуток больше доступного колличества дней");
                        BlockAndPaintButtons();
                        FirstSelectedDate = newDate;
                        SecondSelectedDate = newDate;
                        ClicksOnCalendar = 0;
                        CountSelectedDays = 0;
                        DisplayedDateString = "";
                        ClearColor();
                        //PlannedItem = null;
                    }
                }
            } else
            {
                ShowAlert("Выберете тип отпуска");
            }
        }
        private void ClearColor()
        {
            foreach(ObservableCollection<DayControl> month in Year)
            {
                foreach(DayControl item in month)
                {
                    Grid parentItem = item.Content as Grid;
                    UIElementCollection buttons = parentItem.Children as UIElementCollection;
                    foreach(object elem in buttons)
                    {
                        Button button = elem as Button;
                        button.Background = Brushes.Transparent;
                        FillPlanedDays(button);
                    }
                }
            }
        }
        private void FillPlanedDays(Button button)
        {
            TextBlock textBlock = button.Content as TextBlock;
            for(int i = 0; i < VacationsToAproval.Count; i++)
            {
                Range<DateTime> range = ReturnRange(VacationsToAproval[i]);
                foreach(DateTime date in range.Step(x => x.AddDays(1)))
                {
                    if(date.Day == Convert.ToInt32(textBlock.Text) && 
                        date.Month == Convert.ToInt32(textBlock.Tag.ToString().Split(".")[0]) && 
                        date.Year == Convert.ToInt32(textBlock.Tag.ToString().Split(".")[1]))
                    {
                        button.Background = VacationsToAproval[i].Color;
                        //button.IsEnabled = false;
                    }
                }
            }
        }
        private void PaintButtons()
        {
            foreach(Vacation vacation in VacationsToAproval)
            {
                Range<DateTime> range = ReturnRange(vacation);
                foreach(DateTime date in range.Step(x => x.AddDays(1)))
                {
                    foreach(ObservableCollection<DayControl> month in Year)
                    {
                        foreach(DayControl itemContol in month)
                        {
                            Grid parentItem = itemContol.Content as Grid;
                            UIElementCollection buttons = parentItem.Children as UIElementCollection;
                            foreach(object elem in buttons)
                            {
                                Button button = elem as Button;
                                TextBlock buttonTextBlock = button.Content as TextBlock;
                                int buttonDay = Convert.ToInt32(buttonTextBlock.Text);
                                int buttonMonth = Convert.ToInt32(buttonTextBlock.Tag.ToString().Split(".")[0]);
                                int buttonYear = Convert.ToInt32(buttonTextBlock.Tag.ToString().Split(".")[1]);
                                if(date.Day == buttonDay && date.Month == buttonMonth && date.Year == buttonYear)
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
        private void BlockAndPaintButtons()
        {
            if(PlannedItem != null)
            {
                Range<DateTime> range = ReturnRange(PlannedItem);

                foreach(DateTime date in range.Step(x => x.AddDays(1)))
                {
                    foreach(ObservableCollection<DayControl> month in Year)
                    {
                        foreach(DayControl item in month)
                        {
                            Grid parentItem = item.Content as Grid;
                            UIElementCollection buttons = parentItem.Children as UIElementCollection;
                            foreach(object elem in buttons)
                            {
                                Button button = elem as Button;
                                TextBlock buttonTextBlock = button.Content as TextBlock;
                                int buttonDay = Convert.ToInt32(buttonTextBlock.Text);
                                int buttonMonth = Convert.ToInt32(buttonTextBlock.Tag.ToString().Split(".")[0]);
                                int buttonYear = Convert.ToInt32(buttonTextBlock.Tag.ToString().Split(".")[1]);
                                string buttonNameOfDay = buttonTextBlock.ToolTip.ToString();
                                if(date.Day == buttonDay && date.Month == buttonMonth && date.Year == buttonYear)
                                {
                                    //button.IsEnabled = false;
                                    if(buttonNameOfDay == "Праздник")
                                    {
                                        PlannedItem.Count--;
                                        CountSelectedDays--;
                                        DayAddition = GetDayAddition(CountSelectedDays);
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

        public void ClearVacationData()
        {
            DisplayedDateString = "";
            ClicksOnCalendar = 0;
            ClearColor();
        }
        #endregion Calendar Interaction

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

        private async Task LoadVacationAllowanceForYearAsync()
        {
            IsLoadingPage = true;
            VacationAllowances.Clear();
            ClearColor();
            IEnumerable<VacationAllowanceViewModel> vacations = await App.API.GetVacationAllowanceAsync(App.API.Person.Id_SAP, Convert.ToInt32(CurrentDate.Year));
            IEnumerable<HolidayViewModel> holidays = await Task.Run(async () => await App.API.GetHolidaysAsync(Convert.ToInt32(CurrentDate.Year)));
            foreach(VacationAllowanceViewModel item in vacations)
            {
                VacationAllowances.Add(item);
            }
            OnHolidaysLoad(holidays);

            //foreach(VacationViewModel item in App.API.Vacations)
            //{
            //    VacationsToAproval.Add(new Vacation(item.Name, item.User_Id_SAP, item.Vacation_Id, item.Count, item.Color, item.DateStart, item.DateEnd, item.Status));
            //}
            IsLoadingPage = false;
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
            RenderCalendar();
        }

        private void RenderCalendar()
        {
            
            App.Current.Dispatcher.Invoke((Action) delegate
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

                for(int j = 1; j <= 12; j++)
                {
                    int days = DateTime.DaysInMonth(CurrentDate.Year, j);
                    string daysOfWeekString = new DateTime(CurrentDate.Year, j, 1).DayOfWeek.ToString("d");
                    int daysOfWeek = 6;
                    if(daysOfWeekString != "0")
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
                    for(int i = 1; i <= days; i++)
                    {
                        DayControl ucDays = new DayControl();
                        DateTime date = new DateTime(CurrentDate.Year, j, i);

                        if(date.DayOfWeek.ToString("d") == "6" || date.DayOfWeek.ToString("d") == "0")
                        {
                            DayOffCount++;
                            WorkDaysCount = days - DayOffCount;
                        }

                        ucDays.Day(date);
                        ucDays.DaysOff(date);
                        ucDays.PreviewMouseLeftButtonDown += UcDays_PreviewMouseLeftButtonDown;

                        for(int k = 0; k < App.API.Holidays.Count; k++)
                        {
                            HolidayViewModel holiday = App.API.Holidays[k];
                            if(holiday.Date.Year == Convert.ToInt32(CurrentDate.Year) && holiday.Date.Month == j && holiday.Date.Day == i)
                            {
                                if(holiday.TypeOfHoliday == "Праздник")
                                {
                                    ucDays.Holiday(date);
                                    string d = date.DayOfWeek.ToString("d");
                                    if(d != "6" && d != "0")
                                    {
                                        DayOffCount++;//если не выходной
                                    }
                                    HolidaysCount++;
                                    WorkDaysCount = days - DayOffCount;
                                } else if(holiday.TypeOfHoliday == "Внеплановый")
                                {
                                    // DayOffCount++;
                                    DayOffCount++;
                                    UnscheduledCount++;
                                    WorkDaysCount = days - DayOffCount;
                                    ucDays.DayOffNotInPlan(date);
                                } else if(holiday.TypeOfHoliday == "Рабочий в выходной")
                                {
                                    WorkingOnHolidayCount++;
                                    ucDays.DayWork(date);
                                    DayOffCount--;
                                    WorkDaysCount = days - DayOffCount;
                                }
                            }
                        }
                        Days.Add(ucDays);
                    }
                    FullDays.Add(new DayViewModel(Days, WorkDaysCount, DayOffCount, HolidaysCount, WorkingOnHolidayCount, UnscheduledCount));
                    int countRows = 5;
                    if(Days.Count == 31 && daysOfWeek >= 5)
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
        private string GetDayAddition(int days)
        {
            if(days < 0)
            {
                days *= -1;
            }
            int preLastDigit = days % 100 / 10;

            if(preLastDigit == 1)
            {
                return days + " Дней";
            }

            switch(days % 10)
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
            Range<DateTime> range = Item.Date_end > Item.Date_Start ? Item.Date_Start.To(Item.Date_end) : Item.Date_end.To(Item.Date_Start);
            //TODO: исправить
            return range;
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
                Task.Run(async() => await UpdateVacationAllowance(VacationAllowances[index].Vacation_Id, deletedItem.Date_Start.Year, VacationAllowances[index].Vacation_Days_Quantity));
                VacationsToAproval.Remove(deletedItem);
                PlannedIndex = 0;
                ClearColor();
            }
        }
        public VacationAllowanceViewModel GetVacationAllowance(string name)
        {
            foreach(var item in VacationAllowances)
            {
                if(item.Vacation_Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        public async Task UpdateVacationAllowance(int vacation_Id, int year, int count)
        {
            await App.API.UpdateVacationAllowanceAsync(vacation_Id, year, count);
        }
        #endregion Utils
    }
}
