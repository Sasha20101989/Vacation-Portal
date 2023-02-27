using System.Collections.ObjectModel;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.MVVM.Models
{
    public class Worker
    {
        public int Id_SAP { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Position { get; set; }
        public ObservableCollection<Vacation> Vacations { get; set; } = new ObservableCollection<Vacation>();
        public ObservableCollection<VacationAllowanceViewModel> Vacation_Allowances { get; set; } = new ObservableCollection<VacationAllowanceViewModel>();
        public string FullName => ToString();

        public override string ToString()
        {
            return $"{Surname} {Name} {Patronymic}";
        }
        public Worker(int idSAP, string name, string surname, string patronymic, string position, ObservableCollection<Vacation> vacations, ObservableCollection<VacationAllowanceViewModel> vacationAllowances)
        {
            Id_SAP = idSAP;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Position = position;
            Vacations = vacations;
            Vacation_Allowances = vacationAllowances;
        }
    }
}
