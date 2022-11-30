using System.Collections.ObjectModel;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels.Calendar
{
    public class CalendarViewModel : ViewModelBase
    {
        public ObservableCollection<DayViewModel> Days { get; set; }
        public int ColumnOfWeek { get; set; }
        public int CountRows { get; set; }

        public CalendarViewModel(ObservableCollection<DayViewModel> days, int columnOfWeek, int countRows)
        {
            Days = days;
            ColumnOfWeek = columnOfWeek;
            CountRows = countRows;
        }
    }
}
