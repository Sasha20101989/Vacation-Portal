using System;
using System.Collections.Generic;
using System.Text;

namespace Vacation_Portal.MVVM.Models.HorizontalCalendar
{
   public class HorizontalDay
    {
        public DateTime Date { get; set; }
        public bool HasIntersection { get; set; }
        public bool HasVacation { get; set; }
        public int IntersectionsCount { get; set; }
        public int VacationsCount { get; set; }

        public HorizontalDay(DateTime dateTime, bool hasIntersection, bool hasVacation, int intersectionsCount, int vacationsCount)
        {
            Date = dateTime;
            HasIntersection = hasIntersection;
            HasVacation = hasVacation;
            IntersectionsCount = intersectionsCount;
            VacationsCount = vacationsCount;
        }
    }
}
