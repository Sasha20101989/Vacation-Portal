using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models {
    public class Vacation : ViewModelBase, ICloneable {
        private int _id;
        public int Id {
            get => _id;
            set {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Name { get; set; }
        public string User_Name { get; set; }
        public string User_Surname { get; set; }
        public string FullName => User_Name + " " + User_Surname;
        public int User_Id_SAP { get; set; }
        public int Vacation_Id { get; set; }

        private int _count;
        private string _vacation_Status_Name;

        public int Count {
            get => _count;
            set {
                _count = value;
                OnPropertyChanged(nameof(Count));
            }
        }
        public Brush Color { get; set; }
        public DateTime Date_Start { get; set; }
        public DateTime Date_end { get; set; }
        public IEnumerable<DateTime> DateRange => GetDateRange(Date_Start, Date_end);
        public string Vacation_Status_Name {
            get => _vacation_Status_Name;
            set {
                _vacation_Status_Name = value;
                OnPropertyChanged(nameof(Vacation_Status_Name));
            }
        }

        private int _vacation_Status_Id;

        public int Vacation_Status_Id {
            get => _vacation_Status_Id;
            set {
                _vacation_Status_Id = value;
                OnPropertyChanged(nameof(Vacation_Status_Name));
                Status status = App.API.AllStatuses.FirstOrDefault(s => s.Id == _vacation_Status_Id);
                if(status != null) {
                    Vacation_Status_Name = status.Status_Name;
                }
            }
        }

        public string Creator_Id { get; set; }
        public PackIconKind VacationStatusKind { get; set; }
        public Brush BadgeBackground { get; set; }
        public bool IsIntersectingVacation { get; set; }

        public bool UsedForMerging { get; set; }
        public override string ToString() {
            return $"{Count}: {Date_Start:dd.MM.yyyy} - {Date_end:dd.MM.yyyy}";
        }

        public override bool Equals(object obj) {
            return obj is Vacation vacation && Date_Start == vacation.Date_Start && Date_end == vacation.Date_end;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Date_Start, Date_end);
        }

        public IEnumerable<DateTime> GetDateRange(DateTime start, DateTime end) {
            for(DateTime date = start.Date; date <= end.Date; date = date.AddDays(1)) {
                yield return date;
            }
        }

        public Vacation(int id, string name, int user_Id_SAP, string userName, string userSurname, int vacation_Id, int count, Brush color, DateTime date_Start, DateTime date_end, int statusId, string creator_Id) {
            Id = id;
            Name = name;
            User_Id_SAP = user_Id_SAP;
            Vacation_Id = vacation_Id;
            Count = count;
            Color = color;
            Date_Start = date_Start;
            Date_end = date_end;
            Vacation_Status_Id = statusId;
            Creator_Id = creator_Id;
            User_Name = userName;
            User_Surname = userSurname;
        }

        public Vacation(int vacation_Id, DateTime date_Start, DateTime date_end, int vacation_Status_Id) {
            Vacation_Id = vacation_Id;
            Date_Start = date_Start;
            Date_end = date_end;
            Vacation_Status_Id = vacation_Status_Id;
        }

        public bool IsInRange(DateTime date) {
            return date >= Date_Start && date <= Date_end;
        }

        public object Clone() {
            return MemberwiseClone();
        }
    }
}
