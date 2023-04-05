using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vacation_Portal.Commands.HRDashboardCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels
{
    public class EmployeeViewModel: ViewModelBase
    {
        public Subordinate Employee { get; set; }
        public ObservableCollection<Vacation> Vacations { get; set; }

        public ICommand SpendVacationsCommand { get; }
        public Action<ObservableCollection<Vacation>> VacationsSpended { get; set; }
        public EmployeeViewModel(Subordinate employee)
        {
            Employee = employee;
            Vacations = new ObservableCollection<Vacation>(employee.Subordinate_Vacations.Where(v => v.VacationStatusId == (int) Statuses.PassedToHR).OrderBy(x => x.DateStart));
            SpendVacationsCommand = new SpendVacationsCommand(this);
        }

        public async Task SpendVacations()
        {
            foreach(Vacation vacation in Vacations)
            {
                await App.VacationAPI.SpendVacation(vacation);
                vacation.VacationStatusId = (int) Statuses.Planned;
            }
            VacationsSpended?.Invoke(Vacations);
        }
    }
}
