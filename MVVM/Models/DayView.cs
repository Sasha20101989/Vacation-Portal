using System;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models {
    public class DayView : ViewModelBase {
        private DateTime date;

        public DateTime Date {
            get => date;
            set {
                if(date != value) {
                    date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }
    }
}
