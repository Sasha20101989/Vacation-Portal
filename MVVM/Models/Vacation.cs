using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands;
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
        public string Source { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string FullName => UserName + " " + UserSurname;
        public int UserId { get; set; }
        public int Year { get; set; }
        public int VacationId { get; set; }

        private int _count;
        public int Count {
            get => _count;
            set {
                _count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        public Brush Color { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public IEnumerable<DateTime> DateRange => GetDateRange(DateStart, DateEnd);

        private string _vacationStatusName;
        public string VacationStatusName {
            get => _vacationStatusName;
            set {
                _vacationStatusName = value;
                OnPropertyChanged(nameof(VacationStatusName));
            }
        }

        private int _vacationStatusId;

        public int VacationStatusId {
            get => _vacationStatusId;
            set {
                _vacationStatusId = value;
                OnPropertyChanged(nameof(VacationStatusName));
                Status status = App.AssetsAPI.AllStatuses.FirstOrDefault(s => s.Id == _vacationStatusId);
                if(status != null) {
                    VacationStatusName = status.Status_Name;
                }
            }
        }

        public string CreatorId { get; set; }
        public PackIconKind VacationStatusKind { get; set; }
        public Brush BadgeBackground { get; set; }
        public bool IsIntersectingVacation { get; set; }

        public bool UsedForMerging { get; set; }
        public override string ToString() {
            return $"{Count}: {DateStart:dd.MM.yyyy} - {DateEnd:dd.MM.yyyy}";
        }

        public override bool Equals(object obj) {
            return obj is Vacation vacation && 
                Id == vacation.Id &&
                DateStart == vacation.DateStart &&
                DateEnd == vacation.DateEnd;
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, DateStart, DateEnd);
        }

        public IEnumerable<DateTime> GetDateRange(DateTime start, DateTime end) {
            for(DateTime date = start.Date; date <= end.Date; date = date.AddDays(1)) {
                yield return date;
            }
        }
        public Vacation(string source, int id, string name, int userId, string userName, string userSurname, int vacationId, int count, Brush color, DateTime dateStart, DateTime dateEnd, int statusId, string creatorId) {
            Source = source;
            Id = id;
            Name = name;
            UserId = userId;
            VacationId = vacationId;
            Count = count;
            Color = color;
            DateStart = dateStart;
            DateEnd = dateEnd;
            VacationStatusId = statusId;
            CreatorId = creatorId;
            UserName = userName;
            UserSurname = userSurname;
        }

        public bool IsInRange(DateTime date) {
            return date >= DateStart && date <= DateEnd;
        }

        public object Clone() {
            return MemberwiseClone();
        }
    }
}
