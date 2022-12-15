using System;
using System.Windows.Media;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class Vacation : ViewModelBase
    {
        public string Name { get; set; }
        public int User_Id_SAP { get; set; }
        public int Vacation_Id { get; set; }
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
        public string Status { get; set; }

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

        public Vacation(string name, int user_Id_SAP, int vacation_Id, int count, Brush color, DateTime date_Start, DateTime date_end, string status)
        {
            Name = name;
            User_Id_SAP = user_Id_SAP;
            Vacation_Id = vacation_Id;
            _count = count;
            Color = color;
            Date_Start = date_Start;
            Date_end = date_end;
            Status = status;
        }
    }
}
