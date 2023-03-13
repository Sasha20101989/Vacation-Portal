using System.Collections.ObjectModel;
using Vacation_Portal.MVVM.ViewModels.Calendar;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.ViewModels {
    public class YearViewModel {
        public ObservableCollection<ObservableCollection<DayControl>> Year { get; set; }
        public ObservableCollection<CalendarViewModel> FullYear { get; set; }

        public YearViewModel(ObservableCollection<ObservableCollection<DayControl>> year, ObservableCollection<CalendarViewModel> fullYear) {
            Year = year;
            FullYear = fullYear;
        }
    }
}
