using System.Collections.ObjectModel;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.ViewModels.Calendar {
    public class DayViewModel {
        public ObservableCollection<DayControl> DaysOfMonth { get; set; }
        public int WorkDaysCount { get; set; }
        public int DayOffCount { get; set; }
        public int HolidaysCount { get; set; }
        public int WorkingOnHolidayCount { get; set; }
        public int UnscheduledCount { get; set; }

        public DayViewModel(ObservableCollection<DayControl> daysOfMonth, int workDaysCount, int dayOffCount, int holidaysCount, int workingOnHolidayCount, int unscheduledCount) {
            DaysOfMonth = daysOfMonth;
            WorkDaysCount = workDaysCount;
            DayOffCount = dayOffCount;
            HolidaysCount = holidaysCount;
            WorkingOnHolidayCount = workingOnHolidayCount;
            UnscheduledCount = unscheduledCount;
        }
    }
}
