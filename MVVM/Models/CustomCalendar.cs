﻿using MiscUtil.Collections;
using MiscUtil.Collections.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.ViewModels.Calendar;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.Models
{
    public class CustomCalendar : ViewModelBase
    {
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

        public PersonalVacationPlanningViewModel ViewModel { get; set; }
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
        public CustomCalendar(DateTime currentDate, PersonalVacationPlanningViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public void Render(DateTime currentDate)
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
                    int days = DateTime.DaysInMonth(currentDate.Year, j);
                    string daysOfWeekString = new DateTime(currentDate.Year, j, 1).DayOfWeek.ToString("d");
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
                        DateTime date = new DateTime(currentDate.Year, j, i);

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
                            if(holiday.Date.Year == Convert.ToInt32(currentDate.Year) && holiday.Date.Month == j && holiday.Date.Day == i)
                            {
                                if(holiday.TypeOfHoliday == "Праздник")
                                {
                                    ucDays.Holiday(date);
                                    string d = date.DayOfWeek.ToString("d");
                                    if(d != "6" && d != "0")
                                    {
                                        DayOffCount++;
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

        public void UpdateColor()
        {
            App.Current.Dispatcher.Invoke((Action) delegate
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
            });
        }
        private void FillPlanedDays(Button button)
        {
            TextBlock textBlock = button.Content as TextBlock;
            for(int i = 0; i < ViewModel.VacationsToAproval.Count; i++)
            {
                Range<DateTime> range = ReturnRange(ViewModel.VacationsToAproval[i]);
                foreach(DateTime date in range.Step(x => x.AddDays(1)))
                {
                    if(date.Day == Convert.ToInt32(textBlock.Text) &&
                        date.Month == Convert.ToInt32(textBlock.Tag.ToString().Split(".")[0]) &&
                        date.Year == Convert.ToInt32(textBlock.Tag.ToString().Split(".")[1]))
                    {
                        button.Background = ViewModel.VacationsToAproval[i].Color;
                        //button.IsEnabled = false;
                    }
                }
            }
        }
        public Range<DateTime> ReturnRange(Vacation Item)
        {
            Range<DateTime> range = Item.Date_end > Item.Date_Start ? Item.Date_Start.To(Item.Date_end) : Item.Date_end.To(Item.Date_Start);
            //TODO: исправить
            return range;
        }
        public void BlockAndPaintButtons()
        {
            if(ViewModel.PlannedItem != null)
            {
                Range<DateTime> range = ReturnRange(ViewModel.PlannedItem);

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
                                        ViewModel.PlannedItem.Count--;
                                        CountSelectedDays--;
                                        DayAddition = GetDayAddition(CountSelectedDays);
                                        ViewModel.PlannedVacationString = DayAddition + ": " + SecondSelectedDate.ToString("dd.MM.yyyy") + " - " + FirstSelectedDate.ToString("dd.MM.yyyy");
                                    }

                                    button.Background = ViewModel.PlannedItem.Color;
                                }
                            }
                        }
                    }
                }
            }
        }
        public string GetDayAddition(int days)
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
        public void PaintButtons()
        {
            foreach(Vacation vacation in ViewModel.VacationsToAproval)
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
        public void ClearVacationData()
        {
            ViewModel.PlannedVacationString = "";
            ClicksOnCalendar = 0;
            UpdateColor();
        }
        public void UcDays_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(ViewModel.SelectedItemAllowance != null)
            {
                for(int i = 0; i < ViewModel.VacationAllowances.Count; i++)
                {
                    if(ViewModel.VacationAllowances[i].Vacation_Days_Quantity > 0)
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
                        UpdateColor();
                    }

                    if(ClicksOnCalendar == 1)
                    {
                        FirstSelectedDate = new DateTime(SelectedYear, SelectedMonth, SelectedDay);
                        CountSelectedDays = FirstSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                        if(CountSelectedDays <= ViewModel.SelectedItemAllowance.Vacation_Days_Quantity)
                        {
                            if(SelectedNameDay != "Праздник")
                            {
                                DayAddition = GetDayAddition(CountSelectedDays);
                                ViewModel.PlannedVacationString = DayAddition + ": " + FirstSelectedDate.ToString("d.MM.yyyy");
                                ViewModel.PlannedItem = new Vacation(ViewModel.SelectedItemAllowance.Vacation_Name, ViewModel.Person.Id_SAP, ViewModel.SelectedItemAllowance.Vacation_Id, CountSelectedDays, ViewModel.SelectedItemAllowance.Vacation_Color, FirstSelectedDate, FirstSelectedDate, null);
                            } else
                            {
                                ViewModel.ShowAlert("Этот день является праздичным, начните планирование отпуска с другого дня");
                            }
                        } else
                        {
                            ViewModel.ShowAlert("Выбранный промежуток больше доступного колличества дней");
                        }
                    } else
                    {
                        SecondSelectedDate = new DateTime(SelectedYear, SelectedMonth, SelectedDay);
                        if(SecondSelectedDate > FirstSelectedDate)
                        {
                            CountSelectedDays = SecondSelectedDate.Subtract(FirstSelectedDate).Days + 1;
                            if(CountSelectedDays <= ViewModel.SelectedItemAllowance.Vacation_Days_Quantity)
                            {
                                if(SelectedNameDay != "Праздник")
                                {

                                    DayAddition = GetDayAddition(CountSelectedDays);
                                    ViewModel.PlannedVacationString = DayAddition + ": " + FirstSelectedDate.ToString("dd.MM.yyyy") + " - " + SecondSelectedDate.ToString("dd.MM.yyyy");
                                    ViewModel.PlannedItem = new Vacation(ViewModel.SelectedItemAllowance.Vacation_Name, ViewModel.Person.Id_SAP, ViewModel.SelectedItemAllowance.Vacation_Id, CountSelectedDays, ViewModel.SelectedItemAllowance.Vacation_Color, FirstSelectedDate, SecondSelectedDate, null);
                                } else
                                {
                                    ViewModel.ShowAlert("Этот день является праздичным, закончите планирование отпуска другим днём");
                                }
                            } else
                            {
                                ViewModel.ShowAlert("Выбранный промежуток больше доступного колличества дней");
                            }
                        } else
                        {
                            CountSelectedDays = FirstSelectedDate.Subtract(SecondSelectedDate).Days + 1;
                            if(CountSelectedDays <= ViewModel.SelectedItemAllowance.Vacation_Days_Quantity)
                            {
                                if(SelectedNameDay != "Праздник")
                                {

                                    DayAddition = GetDayAddition(CountSelectedDays);
                                    ViewModel.PlannedVacationString = DayAddition + ": " + SecondSelectedDate.ToString("dd.MM.yyyy") + " - " + FirstSelectedDate.ToString("dd.MM.yyyy");
                                    ViewModel.PlannedItem = new Vacation(ViewModel.SelectedItemAllowance.Vacation_Name, ViewModel.Person.Id_SAP, ViewModel.SelectedItemAllowance.Vacation_Id, CountSelectedDays, ViewModel.SelectedItemAllowance.Vacation_Color, SecondSelectedDate, FirstSelectedDate, null);
                                } else
                                {
                                    ViewModel.ShowAlert("Этот день является праздичным, закончите планирование отпуска с другим днём");
                                }
                            } else
                            {
                                ViewModel.ShowAlert("Выбранный промежуток больше доступного колличества дней");
                            }
                        }
                    }
                    if(CountSelectedDays <= ViewModel.SelectedItemAllowance.Vacation_Days_Quantity)
                    {
                        if(ViewModel.PlannedItem != null)
                        {

                            Range<DateTime> range = ReturnRange(ViewModel.PlannedItem);
                            List<bool> isGoToNext = new List<bool>();

                            foreach(DateTime planedDate in range.Step(x => x.AddDays(1)))
                            {
                                for(int i = 0; i < ViewModel.VacationsToAproval.Count; i++)
                                {
                                    Vacation existingVacation = ViewModel.VacationsToAproval[i];

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
                                ViewModel.ShowAlert("Пересечение отпусков не допустимо");
                                BlockAndPaintButtons();
                                FirstSelectedDate = newDate;
                                SecondSelectedDate = newDate;
                                ClicksOnCalendar = 0;
                                CountSelectedDays = 0;
                                ViewModel.PlannedVacationString = "";
                                UpdateColor();
                            }
                        }
                    } else
                    {

                        ViewModel.ShowAlert("Выбранный промежуток больше доступного колличества дней");
                        BlockAndPaintButtons();
                        FirstSelectedDate = newDate;
                        SecondSelectedDate = newDate;
                        ClicksOnCalendar = 0;
                        CountSelectedDays = 0;
                        ViewModel.PlannedVacationString = "";
                        UpdateColor();
                        //PlannedItem = null;
                    }
                }
            } else
            {
                ViewModel.ShowAlert("Выберете тип отпуска");
            }
        }
    }
}
