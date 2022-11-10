using System;
using System.Collections.Generic;
using System.Text;

namespace Vacation_Portal.MVVM.Models
{
   public class SelectedGap
    {
        public string Count_Days { get; set; }
        public DateTime Date_Start { get; set; }
        public DateTime Date_end { get; set; }

        public override string ToString()
        {
            return $"{Count_Days}: {Date_Start.ToString("dd.MM.yyyy")} - {Date_end.ToString("dd.MM.yyyy")}";
        }
        public SelectedGap(string count_Days, DateTime date_Start, DateTime date_end)
        {
            Count_Days = count_Days;
            Date_Start = date_Start;
            Date_end = date_end;
        }
    }
}
