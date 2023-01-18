using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.MVVM.Models
{
    public class Subordinate
    {
        public int Id_SAP { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Position { get; set; } = "test";
        public ObservableCollection<VacationViewModel> Subordinate_Vacations { get; set; } = new ObservableCollection<VacationViewModel>();
        public ObservableCollection<VacationAllowanceViewModel> Subordinate_Vacation_Allowances { get; set; } = new ObservableCollection<VacationAllowanceViewModel> ();
        public string FullName => ToString();

        public int integer { get; set; }
        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}";
        }
        public Subordinate(int idSAP, string name, string surname, string patronymic, ObservableCollection<VacationViewModel> subordinateVacations, ObservableCollection<VacationAllowanceViewModel> subordinateVacationAllowances)
        {
            integer++;
            Id_SAP = idSAP;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Subordinate_Vacations = subordinateVacations;
            Subordinate_Vacation_Allowances = subordinateVacationAllowances;
        }
    }
}
