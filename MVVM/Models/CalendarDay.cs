using System;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models {
    public class CalendarDay : ViewModelBase {
        private DateTime _date;
        public DateTime Date {
            get => _date;
            set {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private bool _isSelected;
        public bool IsSelected {
            get => _isSelected;
            set {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public CalendarDay(DateTime date) {
            Date = date;
        }
    }
}
