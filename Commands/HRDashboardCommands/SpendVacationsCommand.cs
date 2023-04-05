using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.Commands.HRDashboardCommands
{
    public class SpendVacationsCommand : AsyncComandBase
    {
        private EmployeeViewModel _employeeViewModel;

        public SpendVacationsCommand(EmployeeViewModel employeeViewModel)
        {
            _employeeViewModel = employeeViewModel;
        }

        public async override Task ExecuteAsync(object parameter)
        {
           await _employeeViewModel.SpendVacations();
        }
    }
}
