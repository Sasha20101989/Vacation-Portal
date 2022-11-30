using System.Collections.Generic;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.Models
{
    public class Year
    {
        public Year(List<DayControl> months)
        {
            Months = months;
        }

        public List<DayControl> Months { get; set; }
    }
}
