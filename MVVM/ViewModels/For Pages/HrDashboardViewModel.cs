using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages
{
    public class HrDashboardViewModel: ViewModelBase
    {
        private bool _isEnabled;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private PassedToHRViewModel _passedToHRViewModel = new PassedToHRViewModel();
        public PassedToHRViewModel PassedToHRViewModel
        {
            get
            {
                return _passedToHRViewModel;
            }
            set
            {
                _passedToHRViewModel = value;
                OnPropertyChanged(nameof(PassedToHRViewModel));
            }
        }

        private ObservableCollection<string> _departments = new ObservableCollection<string>();
        public ObservableCollection<string> Departments
        {
            get
            {
                return _departments;
            }
            set
            {
                _departments = value;
                OnPropertyChanged(nameof(Departments));
            }
        }

        private ObservableCollection<Subordinate> _employees = new ObservableCollection<Subordinate>();
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

        private ObservableCollection<EmployeeViewModel> _employeeViewModels = new ObservableCollection<EmployeeViewModel>();
        public ObservableCollection<EmployeeViewModel> EmployeeViewModels
        {
            get
            {
                return _employeeViewModels;
            }
            set
            {
                _employeeViewModels = value;
                OnPropertyChanged(nameof(EmployeeViewModels));
            }
        }

        public AnotherCommandImplementation SpendAllVacationsCommand { get; }

        public HrDashboardViewModel()
        {
            SpendAllVacationsCommand = new AnotherCommandImplementation(
               async _ => {
                   await SpendAllVacations();
               });
            App.API.PersonUpdated += onPersonUpdated;
        }

        private async void onPersonUpdated(Person obj)
        {
            await LoadFiltersAsync();
        }

        public async Task SpendAllVacations()
        {
            IsEnabled = false;
            foreach(EmployeeViewModel employeeViewModel in EmployeeViewModels)
            {
                await employeeViewModel.SpendVacations();
            }
            await LoadFiltersAsync();

        }

        public async Task PrepareEmployee()
        {
            await Task.Delay(100);
            PassedToHRViewModel.PrepareEmployee();
            Employees = PassedToHRViewModel.Employees;
        }

        public async Task LoadFiltersAsync()
        {
            await PrepareEmployee();

            var departments = new HashSet<string>();
            var employeeViewModelsList = Employees
                .Select(employee => new EmployeeViewModel(employee))
                .ToList();

            foreach(var employeeViewModel in employeeViewModelsList)
            {
                employeeViewModel.VacationsSpended += onVacationSpended;
                departments.Add(employeeViewModel.Employee.Department_Name);
            }

            Departments = new ObservableCollection<string>(departments.OrderBy(x => x));
            EmployeeViewModels = new ObservableCollection<EmployeeViewModel>(employeeViewModelsList);
            IsEnabled = EmployeeViewModels.Count > 0;
        }

        private void onVacationSpended(ObservableCollection<Vacation> vacations)
        {
            ObservableCollection<Subordinate> employees = PassedToHRViewModel.Employees;
            ObservableCollection<Subordinate> employeesForSpend = new ObservableCollection<Subordinate>();
            foreach(Subordinate employee in employees)
            {
                if(vacations[0].UserId == employee.Id_SAP)
                {
                    employeesForSpend.Add(employee);
                }
            }
            foreach(Subordinate employee in employeesForSpend)
            {
                PassedToHRViewModel.RemoveEmployee(employee);
            }
            Task.Run(async () => await LoadFiltersAsync());
        }
    }
}
