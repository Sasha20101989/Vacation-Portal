using MiscUtil.Collections;
using MiscUtil.Collections.Extensions;
using System;

namespace Vacation_Portal.MVVM.ViewModels
{
    public class VacationViewModel
    {
        public string Name { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public int Count => GetCount();

        public VacationViewModel(string name, DateTime dateStart, DateTime dateEnd)
        {
            Name = name;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }

        public override bool Equals(object obj)
        {
            return obj is VacationViewModel model &&
                   Name == model.Name &&
                   DateStart == model.DateStart &&
                   DateEnd == model.DateEnd;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, DateStart, DateEnd);
        }

        public Range<DateTime> ReturnVacationRange()
        {
            Range<DateTime> range = DateEnd > DateStart ? DateStart.To(DateEnd) : DateEnd.To(DateStart);
            return range;
        }

        private int GetCount()
        {
            int count = 0;
            Range<DateTime> range = ReturnVacationRange();
            foreach(DateTime date in range.Step(x => x.AddDays(1)))
            {
                count++;
            }
            return count;
        }
    }
}
