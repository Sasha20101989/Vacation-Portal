using System;

namespace Vacation_Portal.MVVM.ViewModels {
    public class HolidayViewModel {
        public int Id { get; set; }
        public string TypeOfHoliday { get; set; }
        public DateTime Date { get; set; }
        public int Year { get; set; }
        public HolidayViewModel(int id, string typeOfHoliday, DateTime date, int year) {
            Id = id;
            TypeOfHoliday = typeOfHoliday;
            Date = date;
            Year = Year;
        }

        public override bool Equals(object obj) {
            return obj is HolidayViewModel model &&
                   Date == model.Date;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Date);
        }
    }
}
