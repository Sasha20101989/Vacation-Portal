using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class Month : ViewModelBase
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int DaysInMonth { get; set; }

        private List<Day> days;
        public List<Day> Days
        {
            get { return days; }
            set
            {
                days = value;
                OnPropertyChanged(nameof(Days));
            }
        }

        private int startDayOfWeek;
        public int StartDayOfWeek
        {
            get { return startDayOfWeek; }
            set
            {
                startDayOfWeek = value;
                OnPropertyChanged(nameof(StartDayOfWeek));
            }
        }

        public Month(int number, string name, int daysInMonth, ObservableCollection<Vacation> intersectingVacations, ObservableCollection<Vacation> vacationsOnApproval)
        {
            intersectingVacations = new ObservableCollection<Vacation>(intersectingVacations.OrderBy(x => x.Date_Start));
            Number = number;
            Name = name;
            DaysInMonth = daysInMonth;

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, Number, 1);
            StartDayOfWeek = (int) firstDayOfMonth.DayOfWeek - 1;
            if(StartDayOfWeek == -1)
            {
                StartDayOfWeek = 6;
            }

            int daysToShowFromPrevMonth = StartDayOfWeek;

            int daysToShowFromNextMonth = (7 - ((daysToShowFromPrevMonth + DaysInMonth) % 7)) % 7;

            Days = new List<Day>();

            for(int i = 0; i < daysToShowFromPrevMonth; i++)
            {
                DateTime day = firstDayOfMonth.AddDays(-(daysToShowFromPrevMonth - i));
                Day newDay = new Day(day);
                foreach(Vacation vacationOnApproval in vacationsOnApproval)
                {
                    foreach(DateTime dateOnApproval in vacationOnApproval.DateRange.Step(x => x.AddDays(1)))
                    {
                        foreach(Vacation vacation in intersectingVacations)
                        {

                            foreach(DateTime date in vacation.DateRange.Step(x => x.AddDays(1)))
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
                    foreach(DateTime dateOnApproval in vacationOnApproval.DateRange.Step(x => x.AddDays(1)))
                    {
                        foreach(Vacation vacation in intersectingVacations)
                        {

                            foreach(DateTime date in vacation.DateRange.Step(x => x.AddDays(1)))
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
                    foreach(DateTime dateOnApproval in vacationOnApproval.DateRange.Step(x => x.AddDays(1)))
                    {
                        foreach(Vacation vacation in intersectingVacations)
                        {

                            foreach(DateTime date in vacation.DateRange.Step(x => x.AddDays(1)))
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
