using System;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages {
    public class AdminPageHrViewModel : ViewModelBase {
        private DateTime _dateUnblockNextCalendar;
        public DateTime DateUnblockNextCalendar {
            get => _dateUnblockNextCalendar;
            set {
                _dateUnblockNextCalendar = value;
                App.API.DateUnblockNextCalendar = _dateUnblockNextCalendar;
                OnPropertyChanged(nameof(DateUnblockNextCalendar));

            }
        }
        private DateTime _dateUnblockPlanningCalendar;
        public DateTime DateUnblockPlanningCalendar {
            get => _dateUnblockPlanningCalendar;
            set {
                _dateUnblockPlanningCalendar = value;
                OnPropertyChanged(nameof(DateUnblockPlanningCalendar));

            }
        }
        public AdminPageHrViewModel() {
            DateUnblockNextCalendar = App.API.DateUnblockNextCalendar;
            DateUnblockPlanningCalendar = App.API.DateUnblockPlanning;
        }
    }
}
