using System.Collections.Generic;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.MVVM.Models
{
    public class Subordinate
    {
        public int Id_SAP { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Position { get; set; }
        public List<VacationViewModel> Subordinate_Vacations { get; set; } = new List<VacationViewModel>();
        public string FullName => ToString();
        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}";
        }
        public Subordinate(int idSAP, string name, string surname, string patronymic, List<VacationViewModel> subordinateVacations)
        {
            Id_SAP = idSAP;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Subordinate_Vacations = subordinateVacations;
        }
    }
}
