using System;
using System.Collections.Generic;

namespace Vacation_Portal.MVVM.Models
{
    public struct DateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public IEnumerable<DateTime> Step(Func<DateTime, DateTime> step)
        {
            for(DateTime date = StartDate; date <= EndDate; date = step(date))
            {
                yield return date;
            }
        }
    }
}
