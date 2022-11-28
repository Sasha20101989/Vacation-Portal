using System;
using System.Collections.Generic;
using System.Text;

namespace Vacation_Portal.MVVM.ViewModels
{
   public class VacationViewModel
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public VacationViewModel(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }

        public override bool Equals(object obj)
        {
            return obj is VacationViewModel model &&
                   Name == model.Name &&
                   Date == model.Date;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Date);
        }
    }
}
