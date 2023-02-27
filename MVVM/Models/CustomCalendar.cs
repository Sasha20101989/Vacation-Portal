using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.ViewModels.Calendar;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.Models {
    public class CustomCalendar : ViewModelBase {
        public List<ObservableCollection<DayControl>> Year { get; set; } = new List<ObservableCollection<DayControl>>();
        public ObservableCollection<CalendarViewModel> FullYear { get; set; } = new ObservableCollection<CalendarViewModel>();

        private readonly PersonalVacationPlanningViewModel _viewModel;
        public string DayAddition { get; set; }
        public int ClicksOnCalendar { get; set; }
        public int CountSelectedDays { get; set; }
        public DateTime FirstSelectedDate { get; set; }
        public DateTime SecondSelectedDate { get; set; }
        public bool CalendarClickable { get; set; }
        public int SelectedDay { get; set; }
        public int SelectedMonth { get; set; }
        public int SelectedYear { get; set; }
        public string SelectedNameDay { get; set; }

        public List<bool> WorkingDays = new List<bool>();

        public int CurrentYear { get; set; }
        public CustomCalendar(int currentYear, PersonalVacationPlanningViewModel viewModel) {
            _viewModel = viewModel;
            CurrentYear = currentYear;
        }

        public async Task Render() {
            await Task.Delay(100);
            App.Current.Dispatcher.Invoke((Action) delegate {
                FullYear.Clear();
                Year.Clear();

                for(int j = 1; j <= 12; j++) {
                    int days = DateTime.DaysInMonth(CurrentYear, j);
                    string daysOfWeekString = new DateTime(CurrentYear, j, 1).DayOfWeek.ToString("d");
                    int daysOfWeek = 6;
                    if(daysOfWeekString != "0") {
                        daysOfWeek = Convert.ToInt32(daysOfWeekString) - 1;
                    }

                    int WorkDaysCount = 0;
                    int DayOffCount = 0;
                    int HolidaysCount = 0;
                    int WorkingOnHolidayCount = 0;
                    int UnscheduledCount = 0;

                    ObservableCollection<DayViewModel> FullDays = new ObservableCollection<DayViewModel>();
                    ObservableCollection<DayControl> Days = new ObservableCollection<DayControl>();
                    for(int i = 1; i <= days; i++) {
                        DayControl ucDays = new DayControl();

                        DateTime date = new DateTime(CurrentYear, j, i);

                        if(date.DayOfWeek.ToString("d") == "6" || date.DayOfWeek.ToString("d") == "0") {
                            DayOffCount++;
                            WorkDaysCount = days - DayOffCount;
                        }

                        ucDays.Day(date);
                        ucDays.DaysOff(date);
                        ucDays.PreviewMouseLeftButtonDown += UcDays_PreviewMouseLeftButtonDown;

                        for(int k = 0; k < App.API.Holidays.Count; k++) {
                            HolidayViewModel holiday = App.API.Holidays[k];
                            if(holiday.Date.Year == Convert.ToInt32(CurrentYear) && holiday.Date.Month == j && holiday.Date.Day == i) {
                                if(holiday.TypeOfHoliday == "Праздник") {
                                    ucDays.Holiday(date);
                                    string d = date.DayOfWeek.ToString("d");
                                    if(d != "6" && d != "0") {
                                        DayOffCount++;
                                    }
                                    HolidaysCount++;
                                    WorkDaysCount = days - DayOffCount;
                                } else if(holiday.TypeOfHoliday == "Внеплановый") {
                                    DayOffCount++;
                                    UnscheduledCount++;
                                    WorkDaysCount = days - DayOffCount;
                                    ucDays.DayOffNotInPlan(date);
                                } else if(holiday.TypeOfHoliday == "Рабочий в выходной") {
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
                    if(Days.Count == 31 && daysOfWeek >= 5) {
                        countRows = 6;
                    }
                    FullYear.Add(new CalendarViewModel(FullDays, daysOfWeek, countRows));
                    Year.Add(Days);
                }
            });
        }

        public void UpdateColor(ObservableCollection<Vacation> VacationsToApproval) {
            App.Current.Dispatcher.Invoke((Action) delegate {
                foreach(ObservableCollection<DayControl> month in Year) {
                    foreach(DayControl item in month) {
                        Grid parentItem = item.Content as Grid;
                        UIElementCollection buttons = parentItem.Children as UIElementCollection;
                        foreach(object element in buttons) {
                            Button button = element as Button;
                            button.Background = Brushes.Transparent;
                            FillPlanedDays(button, VacationsToApproval);
                        }
                    }
                }
            });
        }
        private void FillPlanedDays(Button button, ObservableCollection<Vacation> VacationsToApproval) {
            TextBlock textBlock = button.Content as TextBlock;

            for(int i = 0; i < VacationsToApproval.Count; i++) {
                foreach(DateTime date in VacationsToApproval[i].DateRange) {
                    if(date.Day == Convert.ToInt32(textBlock.Text) &&
                        date.Month == Convert.ToInt32(textBlock.Tag.ToString().Split(".")[0]) &&
                        date.Year == Convert.ToInt32(textBlock.Tag.ToString().Split(".")[1])) {
                        button.Background = VacationsToApproval[i].Color;
                    }
                }
            }
        }
        public static IEnumerable<DateTime> ReturnDateRange(DateTime start, DateTime end) {
            for(DateTime date = start.Date; date <= end.Date; date = date.AddDays(1)) {
                yield return date;
            }
        }

        public void BlockAndPaintButtons() {
            if(_viewModel.PlannedItem != null) {
                foreach(DateTime date in _viewModel.PlannedItem.DateRange) {
                    foreach(ObservableCollection<DayControl> month in Year) {
                        foreach(DayControl item in month) {
                            Grid parentItem = item.Content as Grid;
                            UIElementCollection buttons = parentItem.Children as UIElementCollection;
                            foreach(object element in buttons) {
                                Button button = element as Button;
                                TextBlock buttonTextBlock = button.Content as TextBlock;
                                int buttonDay = Convert.ToInt32(buttonTextBlock.Text);
                                int buttonMonth = Convert.ToInt32(buttonTextBlock.Tag.ToString().Split(".")[0]);
                                int buttonYear = Convert.ToInt32(buttonTextBlock.Tag.ToString().Split(".")[1]);
                                string buttonNameOfDay = buttonTextBlock.ToolTip.ToString();
                                if(date.Day == buttonDay && date.Month == buttonMonth && date.Year == buttonYear) {
                                    if(buttonNameOfDay == "Праздник") {
                                        _viewModel.PlannedItem.Count--;
                                        CountSelectedDays--;
                                        DayAddition = GetDayAddition(CountSelectedDays);
                                        _viewModel.PlannedVacationString = DayAddition + ": " + SecondSelectedDate.ToString("dd.MM.yyyy") + " - " + FirstSelectedDate.ToString("dd.MM.yyyy");
                                    }

                                    button.Background = _viewModel.PlannedItem.Color;
                                }
                            }
                        }
                    }
                }
            }
        }
        public string GetDayAddition(int days) {
            if(days < 0) {
                days *= -1;
            }
            int preLastDigit = days % 100 / 10;

            if(preLastDigit == 1) {
                return days + " Дней";
            }

            switch(days % 10) {
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
        public async Task PaintButtons(ObservableCollection<Vacation> VacationsToApproval) {
            await Task.Delay(100);
            App.Current.Dispatcher.Invoke((Action) delegate {
                foreach(Vacation vacation in VacationsToApproval) {
                    foreach(DateTime date in vacation.DateRange) {
                        foreach(ObservableCollection<DayControl> month in Year) {
                            foreach(DayControl itemControl in month) {
                                Grid parentItem = itemControl.Content as Grid;
                                UIElementCollection buttons = parentItem.Children as UIElementCollection;
                                foreach(object element in buttons) {
                                    Button button = element as Button;
                                    TextBlock buttonTextBlock = button.Content as TextBlock;
                                    int buttonDay = Convert.ToInt32(buttonTextBlock.Text);
                                    int buttonMonth = Convert.ToInt32(buttonTextBlock.Tag.ToString().Split(".")[0]);
                                    int buttonYear = Convert.ToInt32(buttonTextBlock.Tag.ToString().Split(".")[1]);
                                    if(date.Day == buttonDay && date.Month == buttonMonth && date.Year == buttonYear) {
                                        button.Background = vacation.Color;
                                    }
                                }
                            }
                        }
                    }
                }
            });
        }
        public void ClearVacationData(ObservableCollection<Vacation> VacationsToApproval) {
            _viewModel.PlannedVacationString = "";
            ClicksOnCalendar = 0;
            UpdateColor(VacationsToApproval);
        }
        public void UcDays_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if(App.API.IsCalendarPlannedOpen || CurrentYear == DateTime.Now.Year) {
                if(_viewModel.SelectedItemAllowance != null) {
                    ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
                    ObservableCollection<Vacation> VacationsToApproval = new ObservableCollection<Vacation>();
                    if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate) || App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.HR_GOD)) {
                        if(_viewModel.SelectedSubordinate != null) {
                            VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                            _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances.Where(f => f.Vacation_Year == CurrentYear));
                            VacationsToApproval = new ObservableCollection<Vacation>(_viewModel.SelectedSubordinate.Subordinate_Vacations.Where(f => f.Date_Start.Year == CurrentYear));
                        } else {
                            _viewModel.ShowAlert("Сначала выберете подчинённого!");
                        }
                    } else if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Personal)) {
                        VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                            App.API.Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == CurrentYear));
                        VacationsToApproval = new ObservableCollection<Vacation>(App.API.Person.User_Vacations.Where(f => f.Date_Start.Year == CurrentYear));
                    }
                    for(int i = 0; i < VacationAllowances.Count; i++) {
                        if(VacationAllowances[i].Vacation_Days_Quantity > 0) {
                            CalendarClickable = true;
                            break;
                        } else {
                            CalendarClickable = false;
                        }
                    }
                    if(CalendarClickable) {
                        if(e.OriginalSource is Grid) {
                            Grid source = e.OriginalSource as Grid;
                            UIElementCollection elements = source.Children as UIElementCollection;
                            foreach(object element in elements) {
                                if(element is ContentPresenter) {
                                    ContentPresenter presenter = element as ContentPresenter;
                                    TextBlock obj = presenter.Content as TextBlock;
                                    SelectedDay = Convert.ToInt32(obj.Text);
                                    SelectedMonth = Convert.ToInt32(obj.Tag.ToString().Split(".")[0]);
                                    SelectedYear = Convert.ToInt32(obj.Tag.ToString().Split(".")[1]);
                                    SelectedNameDay = obj.ToolTip.ToString();
                                }
                            }
                        } else if(e.OriginalSource is TextBlock) {
                            TextBlock obj = e.OriginalSource as TextBlock;

                            SelectedDay = Convert.ToInt32(obj.Text);
                            SelectedMonth = Convert.ToInt32(obj.Tag.ToString().Split(".")[0]);
                            SelectedYear = Convert.ToInt32(obj.Tag.ToString().Split(".")[1]);
                            SelectedNameDay = obj.ToolTip.ToString();
                        }

                        ClicksOnCalendar++;
                        int countHolidays = 0;
                        int availableQuantity = 0;
                        DateTime newDate = new DateTime();

                        if(ClicksOnCalendar >= 3) {
                            BlockAndPaintButtons();
                            FirstSelectedDate = newDate;
                            SecondSelectedDate = newDate;
                            ClicksOnCalendar = 1;
                            UpdateColor(VacationsToApproval);
                        }

                        if(ClicksOnCalendar == 1) {
                            FirstSelectedDate = new DateTime(SelectedYear, SelectedMonth, SelectedDay);
                            CountSelectedDays = FirstSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                            if(CountSelectedDays <= _viewModel.SelectedItemAllowance.Vacation_Days_Quantity) {
                                if(SelectedNameDay != "Праздник") {
                                    DayAddition = GetDayAddition(CountSelectedDays);
                                    _viewModel.PlannedVacationString = DayAddition + ": " + FirstSelectedDate.ToString("d.MM.yyyy");
                                    if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Personal)) {
                                        _viewModel.PlannedItem = new Vacation(0, _viewModel.SelectedItemAllowance.Vacation_Name, App.API.Person.Id_SAP, App.API.Person.Name, App.API.Person.Surname, _viewModel.SelectedItemAllowance.Vacation_Id, CountSelectedDays, _viewModel.SelectedItemAllowance.Vacation_Color, FirstSelectedDate, FirstSelectedDate, MyEnumExtensions.ToDescriptionString(Statuses.BeingPlanned), Environment.UserName);
                                        Statuses status = Statuses.Deleted;
                                        int statusId = (int) Statuses.Deleted;
                                    } else if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate) || App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.HR_GOD)) {
                                        _viewModel.PlannedItem = new Vacation(0, _viewModel.SelectedItemAllowance.Vacation_Name, _viewModel.SelectedSubordinate.Id_SAP, _viewModel.SelectedSubordinate.Name, _viewModel.SelectedSubordinate.Surname, _viewModel.SelectedItemAllowance.Vacation_Id, CountSelectedDays, _viewModel.SelectedItemAllowance.Vacation_Color, FirstSelectedDate, FirstSelectedDate, MyEnumExtensions.ToDescriptionString(Statuses.BeingPlanned), Environment.UserName);
                                    }
                                } else {
                                    _viewModel.ShowAlert("Этот день является праздничным, начните планирование отпуска с другого дня");
                                }
                            } else {
                                _viewModel.ShowAlert("Выбранный промежуток больше доступного количества дней");
                            }
                        } else {
                            SecondSelectedDate = new DateTime(SelectedYear, SelectedMonth, SelectedDay);
                            IEnumerable<DateTime> dateRange = ReturnDateRange(FirstSelectedDate, SecondSelectedDate);

                            foreach(DateTime date in dateRange) {
                                for(int h = 0; h < App.API.Holidays.Count; h++) {
                                    if(date == App.API.Holidays[h].Date) {
                                        countHolidays++;
                                    }
                                }
                            }
                            if(SecondSelectedDate > FirstSelectedDate) {
                                CountSelectedDays = SecondSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                                availableQuantity = CountSelectedDays - countHolidays;
                                if(availableQuantity <= _viewModel.SelectedItemAllowance.Vacation_Days_Quantity) {
                                    if(SelectedNameDay != "Праздник") {
                                        DayAddition = GetDayAddition(CountSelectedDays);
                                        _viewModel.PlannedVacationString = DayAddition + ": " + FirstSelectedDate.ToString("dd.MM.yyyy") + " - " + SecondSelectedDate.ToString("dd.MM.yyyy");
                                        if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Personal)) {
                                            _viewModel.PlannedItem = new Vacation(0, _viewModel.SelectedItemAllowance.Vacation_Name, _viewModel.SelectedItemAllowance.User_Id_SAP, App.API.Person.Name, App.API.Person.Surname, _viewModel.SelectedItemAllowance.Vacation_Id, CountSelectedDays, _viewModel.SelectedItemAllowance.Vacation_Color, FirstSelectedDate, SecondSelectedDate, MyEnumExtensions.ToDescriptionString(Statuses.BeingPlanned), Environment.UserName);
                                        } else if(App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.Subordinate) || App.SelectedMode == MyEnumExtensions.ToDescriptionString(Modes.HR_GOD)) {
                                            _viewModel.PlannedItem = new Vacation(0, _viewModel.SelectedItemAllowance.Vacation_Name, _viewModel.SelectedSubordinate.Id_SAP, _viewModel.SelectedSubordinate.Name, _viewModel.SelectedSubordinate.Surname, _viewModel.SelectedItemAllowance.Vacation_Id, CountSelectedDays, _viewModel.SelectedItemAllowance.Vacation_Color, FirstSelectedDate, SecondSelectedDate, MyEnumExtensions.ToDescriptionString(Statuses.BeingPlanned), Environment.UserName);
                                        }
                                    } else {
                                        _viewModel.ShowAlert("Этот день является праздничным, закончите планирование отпуска другим днём");
                                    }
                                } else {
                                    _viewModel.ShowAlert("Выбранный промежуток больше доступного количества дней");
                                }
                            } else {
                                CountSelectedDays = FirstSelectedDate.Subtract(SecondSelectedDate).Days + 1;
                                availableQuantity = CountSelectedDays - countHolidays;
                                if(availableQuantity <= _viewModel.SelectedItemAllowance.Vacation_Days_Quantity) {
                                    if(SelectedNameDay != "Праздник") {

                                        DayAddition = GetDayAddition(CountSelectedDays);
                                        _viewModel.PlannedVacationString = DayAddition + ": " + SecondSelectedDate.ToString("dd.MM.yyyy") + " - " + FirstSelectedDate.ToString("dd.MM.yyyy");
                                        _viewModel.PlannedItem = new Vacation(0, _viewModel.SelectedItemAllowance.Vacation_Name, App.API.Person.Id_SAP, App.API.Person.Name, App.API.Person.Surname, _viewModel.SelectedItemAllowance.Vacation_Id, CountSelectedDays, _viewModel.SelectedItemAllowance.Vacation_Color, SecondSelectedDate, FirstSelectedDate, MyEnumExtensions.ToDescriptionString(Statuses.BeingPlanned), Environment.UserName);
                                    } else {
                                        _viewModel.ShowAlert("Этот день является праздничным, закончите планирование отпуска с другим днём");
                                    }
                                } else {
                                    _viewModel.ShowAlert("Выбранный промежуток больше доступного количества дней");
                                }
                            }
                        }
                        if(availableQuantity <= _viewModel.SelectedItemAllowance.Vacation_Days_Quantity) {
                            if(_viewModel.PlannedItem != null) {
                                List<bool> isGoToNext = new List<bool>();

                                foreach(DateTime planedDate in _viewModel.PlannedItem.DateRange) {
                                    for(int i = 0; i < VacationsToApproval.Count; i++) {
                                        Vacation existingVacation = VacationsToApproval[i];

                                        foreach(DateTime existingDate in existingVacation.DateRange) {
                                            if(existingDate == planedDate) {
                                                isGoToNext.Add(false);
                                            }
                                        }
                                        if(isGoToNext.Contains(false)) {
                                            break;
                                        }
                                    }
                                }

                                if(!isGoToNext.Contains(false)) {
                                    BlockAndPaintButtons();
                                } else {
                                    _viewModel.ShowAlert("Пересечение отпусков не допустимо");
                                    BlockAndPaintButtons();
                                    FirstSelectedDate = newDate;
                                    SecondSelectedDate = newDate;
                                    ClicksOnCalendar = 0;
                                    CountSelectedDays = 0;
                                    _viewModel.PlannedVacationString = "";
                                    UpdateColor(VacationsToApproval);
                                }
                            }
                        } else {

                            _viewModel.ShowAlert("Выбранный промежуток больше доступного количества дней");
                            BlockAndPaintButtons();
                            FirstSelectedDate = newDate;
                            SecondSelectedDate = newDate;
                            ClicksOnCalendar = 0;
                            CountSelectedDays = 0;
                            _viewModel.PlannedVacationString = "";
                            UpdateColor(VacationsToApproval);
                        }
                    }
                } else {
                    _viewModel.ShowAlert("Выберете тип отпуска");
                }
            } else {
                _viewModel.ShowAlert($"Планирование будет доступно c {App.API.DateUnblockPlanning:dd.MM.yyyy}");
            }
        }
    }
}