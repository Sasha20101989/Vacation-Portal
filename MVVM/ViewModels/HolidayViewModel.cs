using System;
using System.Collections.Generic;
using System.Text;

namespace Vacation_Portal.MVVM.ViewModels
{
   public class HolidayViewModel
    {
        public string TypeOfHoliday { get; set; }
        public DateTime Date { get; set; }
        public HolidayViewModel(string typeOfHoliday, DateTime date)
        {
            TypeOfHoliday = typeOfHoliday;
            Date = date;
        }

        public override bool Equals(object obj)
        {
            return obj is HolidayViewModel model &&
                   Date == model.Date;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date);
        }
    }
}
