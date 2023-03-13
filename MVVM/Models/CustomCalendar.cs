using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.Commands;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.ViewModels.Calendar;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.Models {
    public class CustomCalendar : ViewModelBase {
        public ObservableCollection<ObservableCollection<DayControl>> Year { get; set; } = new ObservableCollection<ObservableCollection<DayControl>>();
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

        public ICommand DayClickCommand { get; set; }
        public CustomCalendar(int currentYear, PersonalVacationPlanningViewModel viewModel) {
            _viewModel = viewModel;
            CurrentYear = currentYear;
        }

        public async Task Render() {
            await Task.Delay(100);
            FullYear = new ObservableCollection<CalendarViewModel>();
            Year = new ObservableCollection<ObservableCollection<DayControl>>();
            App.Current.Dispatcher.Invoke((Action) delegate {
                for(int j = 1; j <= 12; j++) {
                    int days = DateTime.DaysInMonth(CurrentYear, j);
                    int daysOfWeek = (int) new DateTime(CurrentYear, j, 1).DayOfWeek - 1;
                    if(daysOfWeek < 0) {
                        daysOfWeek += 7;
                    }

                    int workDaysCount = 0;
                    int dayOffCount = 0;
                    int holidaysCount = 0;
                    int workingOnHolidayCount = 0;
                    int unscheduledCount = 0;

                    ObservableCollection<DayViewModel> fullDays = new ObservableCollection<DayViewModel>();
                    ObservableCollection<DayControl> daysOfMonth = new ObservableCollection<DayControl>();
                    string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(j);
                    for(int i = 1; i <= days; i++) {
                        DayControl ucDays = new DayControl();
                        DateTime date = new DateTime(CurrentYear, j, i);
                        ucDays.Day(date);
                        ucDays.DaysOff(date);
                        ucDays.PreviewMouseLeftButtonDown += HandleDayClick;

                        if(date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) {
                            dayOffCount++;
                            workDaysCount = days - dayOffCount;
                        }

                        foreach(HolidayViewModel holiday in App.API.Holidays.Where(h => h.Date.Year == CurrentYear && h.Date.Month == j && h.Date.Day == i)) {
                            switch(holiday.TypeOfHoliday) {
                                case "Праздник":
                                    ucDays.Holiday(date);
                                    if(date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday) {
                                        dayOffCount++;
                                    }
                                    holidaysCount++;
                                    workDaysCount = days - dayOffCount;
                                    break;
                                case "Внеплановый":
                                    dayOffCount++;
                                    unscheduledCount++;
                                    workDaysCount = days - dayOffCount;
                                    ucDays.DayOffNotInPlan(date);
                                    break;
                                case "Рабочий в выходной":
                                    workingOnHolidayCount++;
                                    ucDays.DayWork(date);
                                    dayOffCount--;
                                    workDaysCount = days - dayOffCount;
                                    break;
                            }
                        }
                        daysOfMonth.Add(ucDays);
                    }
                    fullDays.Add(new DayViewModel(daysOfMonth, workDaysCount, dayOffCount, holidaysCount, workingOnHolidayCount, unscheduledCount));
                    int countRows = 5;
                    if(daysOfMonth.Count == 31 && daysOfWeek >= 5) {
                        countRows = 6;
                    }
                    FullYear.Add(new CalendarViewModel(fullDays, daysOfWeek, countRows, monthName));
                    Year.Add(daysOfMonth);
                }
            });
        }

        public async Task UpdateColorAsync(ObservableCollection<Vacation> VacationsToApproval) {
            await Task.Run(() => {
                App.Current.Dispatcher.Invoke(() => {
                    foreach(DayControl month in Year.SelectMany(x => x)) {
                        Grid parentItem = month.Content as Grid;
                        IEnumerable<Button> buttons = parentItem.Children.OfType<Button>();
                        foreach(Button button in buttons) {
                            button.Background = Brushes.Transparent;
                            FillPlanedDays(button, VacationsToApproval);
                        }
                    }
                });
            });
        }
        private void FillPlanedDays(Button button, ObservableCollection<Vacation> VacationsToApproval) {
            TextBlock textBlock = button.Content as TextBlock;
            DateTime date = DateTime.Parse($"{textBlock.Tag}.{textBlock.Text}");
            Vacation vacation = VacationsToApproval.FirstOrDefault(x => x.DateRange.Contains(date));
            if(vacation != null) {
                button.Background = vacation.Color;
            }
        }

        public IEnumerable<DateTime> ReturnDateRange(DateTime start, DateTime end) {
            for(DateTime date = start.Date; date <= end.Date; date = date.AddDays(1)) {
                yield return date;
            }
        }

        public void PaintButtons() {
            if(_viewModel.PlannedItem != null) {
                IEnumerable<DateTime> plannedDateRange = _viewModel.PlannedItem.DateRange;
                foreach(ObservableCollection<DayControl> month in Year) {
                    foreach(DayControl item in month) {
                        Grid parentItem = item.Content as Grid;
                        IEnumerable<Button> buttons = parentItem.Children.OfType<Button>();
                        foreach(Button button in buttons) {
                            TextBlock buttonTextBlock = button.Content as TextBlock;
                            int buttonDay = int.Parse(buttonTextBlock.Text);
                            int buttonMonth = int.Parse(buttonTextBlock.Tag.ToString().Split(".")[0]);
                            int buttonYear = int.Parse(buttonTextBlock.Tag.ToString().Split(".")[1]);
                            string buttonNameOfDay = buttonTextBlock.ToolTip.ToString();
                            bool dateMatches = plannedDateRange.Any(date => date.Day == buttonDay && date.Month == buttonMonth && date.Year == buttonYear);
                            if(dateMatches) {
                                if(buttonNameOfDay == "Праздник") {
                                    _viewModel.PlannedItem.Count--;
                                    CountSelectedDays--;
                                    DayAddition = GetDayAddition(CountSelectedDays);
                                    _viewModel.PlannedVacationString = $"{DayAddition}: {SecondSelectedDate:dd.MM.yyyy} - {FirstSelectedDate:dd.MM.yyyy}";
                                }

                                button.Background = _viewModel.PlannedItem.Color;
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

        public async Task ClearVacationData(ObservableCollection<Vacation> VacationsToApproval) {
            _viewModel.PlannedVacationString = "";
            ClicksOnCalendar = 0;
            await UpdateColorAsync(VacationsToApproval);
        }
        public void HandleDayClick(object sender, MouseButtonEventArgs e) {
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
            ICommand command = new DayClickCommand(_viewModel, this);

            // выполнить команду
            if(command.CanExecute(null)) {
                command.Execute(null);
            }
        }
    }
}