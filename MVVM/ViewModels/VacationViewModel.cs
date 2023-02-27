using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Vacation_Portal.MVVM.ViewModels {
    public class VacationViewModel {
        public string Name { get; set; }
        public int User_Id_SAP { get; set; }
        public int Vacation_Id { get; set; }
        public Brush Color { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Vacation_Status_Name { get; set; }
        public string Creator_Id { get; set; }
        public int Count => GetCount();

        public VacationViewModel(string name, int user_Id_SAP, int vacation_Id, Brush color, DateTime dateStart, DateTime dateEnd, string statusName, string creator_Id) {
            Name = name;
            User_Id_SAP = user_Id_SAP;
            Vacation_Id = vacation_Id;
            Color = color;
            DateStart = dateStart;
            DateEnd = dateEnd;
            Vacation_Status_Name = statusName;
            Creator_Id = creator_Id;
        }

        public override bool Equals(object obj) {
            return obj is VacationViewModel model &&
                   User_Id_SAP == model.User_Id_SAP &&
                   Name == model.Name &&
                   DateStart == model.DateStart &&
                   DateEnd == model.DateEnd;
        }

        public override int GetHashCode() {
            return HashCode.Combine(User_Id_SAP, Name, DateStart, DateEnd);
        }

        public IEnumerable<DateTime> GetDateRange(DateTime start, DateTime end) {
            for(DateTime date = start.Date; date <= end.Date; date = date.AddDays(1)) {
                yield return date;
            }
        }

        private int GetCount() {
            int count = 0;
            IEnumerable<DateTime> range = GetDateRange(DateStart, DateEnd);
            foreach(DateTime date in range) {
                count++;
            }
            return count;
        }
    }
}
