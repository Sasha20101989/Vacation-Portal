using System.Collections.ObjectModel;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.ViewModels.Calendar {
    public class DayViewModel : ViewModelBase {
        public ObservableCollection<DayControl> Days { get; set; }
        public int WorkDaysCount { get; set; }
        public int DayOffCount { get; set; }
        public int HolidaysCount { get; set; }
        public int WorkingOnHolidayCount { get; set; }
        public int UnscheduledCount { get; set; }

        public DayViewModel(ObservableCollection<DayControl> days, int workDaysCount, int dayOffCount, int holidaysCount, int workingOnHolidayCount, int unscheduledCount) {
            Days = days;
            WorkDaysCount = workDaysCount;
            DayOffCount = dayOffCount;
            HolidaysCount = holidaysCount;
            WorkingOnHolidayCount = workingOnHolidayCount;
            UnscheduledCount = unscheduledCount;
        }
    }
}
