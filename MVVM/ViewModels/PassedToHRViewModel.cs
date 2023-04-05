using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels
{
    public class PassedToHRViewModel: ViewModelBase
    {
        private ObservableCollection<Subordinate> _employees;
        public ObservableCollection<Subordinate> Employees
        {
            get
            {
                return _employees;
            }
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }

        public void PrepareEmployee()
        {
            Employees = new ObservableCollection<Subordinate>();
            ObservableCollection<Subordinate> employees = App.API.Person.Subordinates; //TODO: преписать на await GET Subordinates

            if(employees == null)
            {
                return;
            }

            ObservableCollection<Subordinate> employeesList = new ObservableCollection<Subordinate>(employees
                .Where(employee => employee.Subordinate_Vacations
                  .Any(vacation => vacation.VacationStatusId == (int) Statuses.PassedToHR))
                .OrderBy(employee => employee.FullName));

            Employees = employeesList;
        }

        public void RemoveEmployee(Subordinate employee)
        {
            if(Employees.Count > 0)
            {
                Employees.Remove(employee);
            }
        }
    }
}
