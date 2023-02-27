using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class Month : ViewModelBase
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int DaysInMonth { get; set; }

        private List<Day> _days;
        public List<Day> Days
        {
            get => _days;
            set
            {
                _days = value;
                OnPropertyChanged(nameof(Days));
            }
        }

        private int _startDayOfWeek;
        public int StartDayOfWeek
        {
            get => _startDayOfWeek;
            set
            {
                _startDayOfWeek = value;
                OnPropertyChanged(nameof(StartDayOfWeek));
            }
        }

        public Month(int number, string name, int daysInMonth, ObservableCollection<Vacation> intersectingVacations, ObservableCollection<Vacation> vacationsOnApproval, Vacation vacationItem)
        {
            Number = number;
            Name = name;
            DaysInMonth = daysInMonth;

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, Number, 1);
            StartDayOfWeek = (int) firstDayOfMonth.DayOfWeek - 1;
            if(StartDayOfWeek == -1)
            {
                StartDayOfWeek = 6;
            }
            int countVacationDays = vacationItem.Count;
            DateTime centerVacationDate = vacationItem.Date_end.AddDays(-countVacationDays / 2);
            int week = ISOWeek.GetWeekOfYear(centerVacationDate);

            int daysToShowFromPrevMonth = StartDayOfWeek;

            int daysToShowFromNextMonth = ((7 - ((daysToShowFromPrevMonth + DaysInMonth) % 7)) % 7) + (countVacationDays / 2);

            Days = new List<Day>();
            if(intersectingVacations.Count == 0)
            {
                vacationsOnApproval = new ObservableCollection<Vacation>(vacationsOnApproval.OrderBy(x => x.Date_Start));

                for(int i = 0; i < daysToShowFromPrevMonth; i++)
                {
                    DateTime day = firstDayOfMonth.AddDays(-(daysToShowFromPrevMonth - i));
                    Day newDay = new Day(day);
                    foreach(Vacation vacationOnApproval in vacationsOnApproval)
                    {
                        if(vacationItem == vacationOnApproval)
                        {
                            foreach(DateTime dateOnApproval in vacationOnApproval.DateRange)
                            {

                                foreach(DateTime date in vacationOnApproval.DateRange)
                                {

                                    if(date.Month == newDay.Date.Month + 1)
                                    {
                                        if(dateOnApproval == date)
                                        {
                                            if(date == newDay.Date)
                                            {
                                                newDay.IsNotConflict = true;
                                                newDay.ToolTipText = $"{vacationOnApproval.User_Name} {vacationOnApproval.User_Surname}";
                                            }
                                            if(!Days.Contains(newDay))
                                            {
                                                if(newDay.Date.DayOfWeek == DayOfWeek.Saturday || newDay.Date.DayOfWeek == DayOfWeek.Sunday)
                                                {
                                                    newDay.IsHoliday = true;
                                                    newDay.ToolTipText = "Выходной";
                                                }
                                                newDay.Week = ISOWeek.GetWeekOfYear(newDay.Date);
                                                newDay.IsOtherMonth = true;
                                                Days.Add(newDay);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                for(int i = 1; i <= DaysInMonth; i++)
                {
                    DateTime day = new DateTime(DateTime.Now.Year, number, i);
                    Day newDay = new Day(day);
                    foreach(Vacation vacationOnApproval in vacationsOnApproval)
                    {
                        if(vacationItem == vacationOnApproval)
                        {
                            foreach(DateTime dateOnApproval in vacationOnApproval.DateRange)
                            {

                                foreach(DateTime date in vacationOnApproval.DateRange)
                                {

                                    if(date.Month == newDay.Date.Month)
                                    {
                                        if(dateOnApproval == date)
                                        {
                                            if(date == newDay.Date)
                                            {
                                                newDay.IsNotConflict = true;
                                                newDay.ToolTipText = $"{vacationOnApproval.User_Name} {vacationOnApproval.User_Surname}";
                                            }
                                            if(!Days.Contains(newDay))
                                            {
                                                if(newDay.Date.DayOfWeek == DayOfWeek.Saturday || newDay.Date.DayOfWeek == DayOfWeek.Sunday)
                                                {
                                                    newDay.IsHoliday = true;
                                                    newDay.ToolTipText = "Выходной";
                                                }
                                                newDay.Week = ISOWeek.GetWeekOfYear(newDay.Date);
                                                Days.Add(newDay);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                DateTime nextMonth = firstDayOfMonth.AddMonths(1);
                for(int i = 1; i <= daysToShowFromNextMonth; i++)
                {
                    DateTime day = nextMonth.AddDays(i - 1);
                    Day newDay = new Day(day);
                    foreach(Vacation vacationOnApproval in vacationsOnApproval)
                    {
                        if(vacationItem == vacationOnApproval)
                        {
                            foreach(DateTime dateOnApproval in vacationOnApproval.DateRange)
                            {

                                foreach(DateTime date in vacationOnApproval.DateRange)
                                {

                                    if(date.Month == newDay.Date.Month - 1)
                                    {
                                        if(dateOnApproval == date)
                                        {
                                            if(date == newDay.Date)
                                            {
                                                newDay.IsNotConflict = true;
                                                newDay.ToolTipText = $"{vacationOnApproval.User_Name} {vacationOnApproval.User_Surname}";
                                            }
                                            if(!Days.Contains(newDay))
                                            {
                                                if(newDay.Date.DayOfWeek == DayOfWeek.Saturday || newDay.Date.DayOfWeek == DayOfWeek.Sunday)
                                                {

                                                    newDay.IsHoliday = true;
                                                    newDay.ToolTipText = "Выходной";
                                                }
                                                newDay.Week = ISOWeek.GetWeekOfYear(newDay.Date);
                                                newDay.IsOtherMonth = true;
                                                Days.Add(newDay);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            } else
            {
                intersectingVacations = new ObservableCollection<Vacation>(intersectingVacations.OrderBy(x => x.Date_Start));

                for(int i = 0; i < daysToShowFromPrevMonth; i++)
                {
                    DateTime day = firstDayOfMonth.AddDays(-(daysToShowFromPrevMonth - i));
                    Day newDay = new Day(day);
                    foreach(Vacation vacationOnApproval in vacationsOnApproval)
                    {
                        foreach(DateTime dateOnApproval in vacationOnApproval.DateRange)
                        {
                            foreach(Vacation vacation in intersectingVacations)
                            {

                                foreach(DateTime date in vacation.DateRange)
                                {

                                    if(date.Month == newDay.Date.Month + 1)
                                    {
                                        if(dateOnApproval == date)
                                        {
                                            if(date == newDay.Date)
                                            {
                                                newDay.IsAlreadyScheduledVacation = true;
                                                newDay.ToolTipText = $"{vacation.User_Name} {vacation.User_Surname}";
                                            }
                                            if(!Days.Contains(newDay))
                                            {
                                                if(newDay.Date.DayOfWeek == DayOfWeek.Saturday || newDay.Date.DayOfWeek == DayOfWeek.Sunday)
                                                {
                                                    newDay.IsHoliday = true;
                                                    newDay.ToolTipText = "Выходной";
                                                }
                                                newDay.Week = ISOWeek.GetWeekOfYear(newDay.Date);
                                                newDay.IsOtherMonth = true;
                                                Days.Add(newDay);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                for(int i = 1; i <= DaysInMonth; i++)
                {
                    DateTime day = new DateTime(DateTime.Now.Year, number, i);
                    Day newDay = new Day(day);
                    foreach(Vacation vacationOnApproval in vacationsOnApproval)
                    {
                        foreach(DateTime dateOnApproval in vacationOnApproval.DateRange)
                        {
                            foreach(Vacation vacation in intersectingVacations)
                            {

                                foreach(DateTime date in vacation.DateRange)
                                {

                                    if(date.Month == newDay.Date.Month)
                                    {
                                        if(dateOnApproval == date)
                                        {
                                            if(date == newDay.Date)
                                            {
                                                newDay.IsAlreadyScheduledVacation = true;
                                                newDay.ToolTipText = $"{vacation.User_Name} {vacation.User_Surname}";
                                            }
                                            if(!Days.Contains(newDay))
                                            {
                                                if(newDay.Date.DayOfWeek == DayOfWeek.Saturday || newDay.Date.DayOfWeek == DayOfWeek.Sunday)
                                                {
                                                    newDay.IsHoliday = true;
                                                    newDay.ToolTipText = "Выходной";
                                                }
                                                newDay.Week = ISOWeek.GetWeekOfYear(newDay.Date);
                                                newDay.IsOtherMonth = true;
                                                Days.Add(newDay);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                DateTime nextMonth = firstDayOfMonth.AddMonths(1);
                for(int i = 1; i <= daysToShowFromNextMonth; i++)
                {
                    DateTime day = nextMonth.AddDays(i - 1);
                    Day newDay = new Day(day);
                    foreach(Vacation vacationOnApproval in vacationsOnApproval)
                    {
                        foreach(DateTime dateOnApproval in vacationOnApproval.DateRange)
                        {
                            foreach(Vacation vacation in intersectingVacations)
                            {

                                foreach(DateTime date in vacation.DateRange)
                                {

                                    if(date.Month == newDay.Date.Month - 1)
                                    {
                                        if(dateOnApproval == date)
                                        {
                                            if(date == newDay.Date)
                                            {
                                                newDay.IsAlreadyScheduledVacation = true;
                                                newDay.ToolTipText = $"{vacation.User_Name} {vacation.User_Surname}";
                                            }
                                            if(!Days.Contains(newDay))
                                            {
                                                if(newDay.Date.DayOfWeek == DayOfWeek.Saturday || newDay.Date.DayOfWeek == DayOfWeek.Sunday)
                                                {
                                                    newDay.IsHoliday = true;
                                                    newDay.ToolTipText = "Выходной";
                                                }
                                                newDay.Week = ISOWeek.GetWeekOfYear(newDay.Date);
                                                newDay.IsOtherMonth = true;
                                                Days.Add(newDay);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
