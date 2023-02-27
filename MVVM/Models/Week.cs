using System.Collections.Generic;
using System.Linq;

namespace Vacation_Portal.MVVM.Models {
    public class Week {
        public List<Day> Days { get; set; } = new List<Day>();
        public int WeekNumber => Days.FirstOrDefault()?.Date.DayOfYear / 7 + 1 ?? 0;

        public Week() {
            Days = new List<Day>();
        }

        public void AddDay(Day day) {
            if(Days.Count == 0 || day.Date >= Days[0].Date && day.Date <= Days[Days.Count - 1].Date) {
                Days.Add(day);
            }
        }

        public bool ContainsDay(Day day) {
            return Days.Any(d => d.Date == day.Date);
        }
    }
}
