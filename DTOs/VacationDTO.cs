using System;
using System.Collections.Generic;

namespace Vacation_Portal.DTOs {
    public class VacationDTO {
        public int Id { get; set; }
        public string Vacation_Name { get; set; }
        public string Source { get; set; }
        public int User_Id { get; set; }
        public int Type_Id { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public int Status_Id { get; set; }
        public string Creator_Id { get; set; }
        public int Count => GetCount();

        public IEnumerable<DateTime> GetDateRange(DateTime start, DateTime end) {
            for(DateTime date = start.Date; date <= end.Date; date = date.AddDays(1)) {
                yield return date;
            }
        }

        private int GetCount() {
            int count = 0;
            IEnumerable<DateTime> dateRange = GetDateRange(Start_Date, End_Date);
            foreach(DateTime date in dateRange) {
                count++;
            }
            return count;
        }
    }
}
