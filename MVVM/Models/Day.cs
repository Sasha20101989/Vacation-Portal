﻿using System;
using System.Windows.Media;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models {
    public class Day : ViewModelBase {
        public DateTime Date { get; set; }
        public int Week { get; set; }
        public string ToolTipText { get; set; }
        public Brush ColorIsAlreadyScheduledVacation { get; set; } = Brushes.Red;
        public bool ToolTipVisibility => !string.IsNullOrEmpty(ToolTipText);
        public Brush ColorIsInSelectedVacation { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsOtherMonth { get; set; }
        public bool IsNotConflict { get; set; }

        private bool _isInSelectedVacation;
        public bool IsInSelectedVacation {
            get => _isInSelectedVacation;
            set {
                _isInSelectedVacation = value;
                OnPropertyChanged(nameof(IsInSelectedVacation));
            }
        }

        private bool _isAlreadyScheduledVacation;
        public bool IsAlreadyScheduledVacation {
            get => _isAlreadyScheduledVacation;
            set {
                _isAlreadyScheduledVacation = value;
                OnPropertyChanged(nameof(IsAlreadyScheduledVacation));
            }
        }

        public DayOfWeek DayOfWeek => Date.DayOfWeek;

        public Day(DateTime date) {
            Date = date;
        }

        public override bool Equals(object obj) {
            return obj is Day day &&
                   Date == day.Date;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Date);
        }
    }
}
