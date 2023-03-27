using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class Subordinate : ViewModelBase
    {
        public int Id_SAP { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Position { get; set; }
        public string Department_Name { get; set; }
        public string Virtual_Department_Name { get; set; }

        private int _countStatesOnApproval;
        public int CountStatesOnApproval
        {
            get => _countStatesOnApproval;
            set
            {
                _countStatesOnApproval = value;
                OnPropertyChanged(nameof(CountStatesOnApproval));
            }
        }

        private int _countStatesDecline;
        public int CountStatesDecline
        {
            get => _countStatesDecline;
            set
            {
                _countStatesDecline = value;
                OnPropertyChanged(nameof(CountStatesDecline));
            }
        }

        private int _countStatesApproval;
        public int CountStatesApproval
        {
            get => _countStatesApproval;
            set
            {
                _countStatesApproval = value;
                OnPropertyChanged(nameof(CountStatesApproval));
            }
        }

        public ObservableCollection<Vacation> Subordinate_Vacations { get; set; } = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> SubordinateVacationsWithOnApprovalStatus => UpdateListWithVacationsONApproval();

        public ObservableCollection<VacationAllowanceViewModel> Subordinate_Vacation_Allowances { get; set; } = new ObservableCollection<VacationAllowanceViewModel>();
        public string FullName => ToString();
        public ObservableCollection<Vacation> UpdateListWithVacationsONApproval()
        {
            ObservableCollection<Vacation> subordinateVacationsWithOnApprovalStatus = new ObservableCollection<Vacation>();
            subordinateVacationsWithOnApprovalStatus =
            new ObservableCollection<Vacation>(Subordinate_Vacations
                .Where(v => v.Vacation_Status_Name == MyEnumExtensions.ToDescriptionString(Statuses.OnApproval)));
            return subordinateVacationsWithOnApprovalStatus;
        }
        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}";
        }
        public Subordinate(int idSAP, string name, string surname, string patronymic, string position, string departmentName, string virtualDepartmentName, ObservableCollection<Vacation> subordinateVacations, ObservableCollection<VacationAllowanceViewModel> subordinateVacationAllowances)
        {
            Id_SAP = idSAP;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Position = position;
            Subordinate_Vacations = subordinateVacations;
            Subordinate_Vacation_Allowances = subordinateVacationAllowances;
            Department_Name = departmentName;
            Virtual_Department_Name = virtualDepartmentName;
        }

        internal IEnumerable<DateTime> SelectMany(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        public void UpdateStatesCount()
        {
            CountStatesOnApproval = App.StateAPI.PersonStates.Count(s => s.StatusId == (int) Statuses.OnApproval && s.Vacation.User_Id_SAP == Id_SAP);
            CountStatesApproval = App.StateAPI.PersonStates.Count(s => s.StatusId == (int) Statuses.Approved && s.Vacation.User_Id_SAP == Id_SAP);
            CountStatesDecline = App.StateAPI.PersonStates.Count(s => s.StatusId == (int) Statuses.NotAgreed && s.Vacation.User_Id_SAP == Id_SAP);
        }
    }
}
