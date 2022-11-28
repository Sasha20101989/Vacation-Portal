using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.ViewModels.Calendar
{
    public class CalendarViewModel : ViewModelBase
    {
        public ObservableCollection<DayViewModel> Days { get; set; }
        public int ColumnOfWeek { get; set; }
        public int CountRows{ get; set; }

        public CalendarViewModel(ObservableCollection<DayViewModel> days, int columnOfWeek, int countRows)
        {
            Days = days;
            ColumnOfWeek = columnOfWeek;
            CountRows = countRows;
        }
    }
}
