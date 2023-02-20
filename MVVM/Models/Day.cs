using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class Day: ViewModelBase
    {
        public DateTime Date { get; set; }
        public string ToolTipText { get; set; }
        public Brush ColorIsAlreadyScheduledVacation { get; set; } = Brushes.Red;
        public bool ToolTipVisibility => string.IsNullOrEmpty(ToolTipText) ? false : true;
        public Brush ColorIsInSelectedVacation { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsOtherMonth { get; set; }

        private bool _isInSelectedVacation;
        public bool IsInSelectedVacation
        {
            get => _isInSelectedVacation;
            set
            {
                _isInSelectedVacation = value;
                OnPropertyChanged(nameof(IsInSelectedVacation));
            }
        }

        private bool _isAlreadyScheduledVacation;
        public bool IsAlreadyScheduledVacation
        {
            get
            {
                return _isAlreadyScheduledVacation;
            }
            set
            {
                _isAlreadyScheduledVacation = value;
                OnPropertyChanged(nameof(IsAlreadyScheduledVacation));
            }
        }
        public Day(DateTime date)
        {
            Date = date;
        }

    }
}
