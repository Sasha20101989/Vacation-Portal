using System;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages
{
    public class AdminPageHrViewModel : ViewModelBase
    {
        private DateTime _dateUnblockNextCalendar;
        public DateTime DateUnblockNextCalendar
        {
            get => _dateUnblockNextCalendar;
            set
            {
                _dateUnblockNextCalendar = value;
                App.API.DateUnblockNextCalendar = _dateUnblockNextCalendar;
                OnPropertyChanged(nameof(DateUnblockNextCalendar));

            }
        }
        private DateTime _dateUnblockPlanningCalendar;
        public DateTime DateUnblockPlanningCalendar
        {
            get => _dateUnblockPlanningCalendar;
            set
            {
                _dateUnblockPlanningCalendar = value;
                OnPropertyChanged(nameof(DateUnblockPlanningCalendar));

            }
        }
        public AdminPageHrViewModel()
        {
            _dateUnblockNextCalendar = DateTime.Parse("2022-12-14");
            _dateUnblockPlanningCalendar = DateTime.Parse("2022-12-14");
            App.API.DateUnblockNextCalendar = DateUnblockNextCalendar;
            App.API.DateUnblockPlanning = DateUnblockPlanningCalendar;
        }
    }
}
