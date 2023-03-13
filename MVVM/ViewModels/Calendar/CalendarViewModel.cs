using System.Collections.ObjectModel;

namespace Vacation_Portal.MVVM.ViewModels.Calendar {
    public class CalendarViewModel {
        public ObservableCollection<DayViewModel> FullDays { get; set; }
        public int DaysOfWeek { get; set; }
        public int CountRows { get; set; }

        public string MonthName { get; set; }

        public CalendarViewModel(ObservableCollection<DayViewModel> fullDays, int daysOfWeek, int countRows, string monthName) {
            FullDays = fullDays;
            DaysOfWeek = daysOfWeek;
            CountRows = countRows;
            MonthName = monthName;
        }
    }
}
