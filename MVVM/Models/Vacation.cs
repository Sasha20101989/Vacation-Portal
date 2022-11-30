﻿using System;
using System.Windows.Media;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class Vacation : ViewModelBase
    {
        public string Name { get; set; }
        //public int Count { get; set; }
        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged(nameof(Count));
            }
        }
        public Brush Color { get; set; }
        public DateTime Date_Start { get; set; }
        public DateTime Date_end { get; set; }

        public override string ToString()
        {
            return $"{Count}: {Date_Start:dd.MM.yyyy} - {Date_end:dd.MM.yyyy}";
        }

        public override bool Equals(object obj)
        {
            return obj is Vacation vacation && Date_Start == vacation.Date_Start && Date_end == vacation.Date_end;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Date_Start, Date_end);
        }

        public Vacation(string name, int count, Brush color, DateTime date_Start, DateTime date_end)
        {
            Name = name;
            _count = count;
            Color = color;
            Date_Start = date_Start;
            Date_end = date_end;
        }
    }
}
