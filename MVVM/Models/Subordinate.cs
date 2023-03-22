using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.MVVM.Models {
    public class Subordinate {
        public int Id_SAP { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Position { get; set; }
        public string Department_Name { get; set; }
        public string Virtual_Department_Name { get; set; }

        public ObservableCollection<Vacation> Subordinate_Vacations { get; set; } = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> SubordinateVacationsWithOnApprovalStatus => 
            new ObservableCollection<Vacation>(Subordinate_Vacations
                .Where(v => v.Vacation_Status_Name == MyEnumExtensions.ToDescriptionString(Statuses.OnApproval)));

        public ObservableCollection<VacationAllowanceViewModel> Subordinate_Vacation_Allowances { get; set; } = new ObservableCollection<VacationAllowanceViewModel>();
        public string FullName => ToString();

        public override string ToString() {
            return $"{Surname} {Name} {Patronymic}";
        }
        public Subordinate(int idSAP, string name, string surname, string patronymic, string position, string departmentName, string virtualDepartmentName, ObservableCollection<Vacation> subordinateVacations, ObservableCollection<VacationAllowanceViewModel> subordinateVacationAllowances) {
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
    }
}
