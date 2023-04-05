using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class TransferVacationCommand : AsyncComandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;

        public TransferVacationCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public async override Task ExecuteAsync(object parameter)
        {
            Vacation transferedVacation = (Vacation) parameter;
            ObservableCollection<VacationAllowanceViewModel> VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>();
            if(transferedVacation == null)
            {
                return;
            }

            if(App.SelectedMode == WindowMode.Personal)
            {
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    App.API.Person.User_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD)
            {
                VacationAllowances = new ObservableCollection<VacationAllowanceViewModel>(
                    _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances.Where(f => f.Vacation_Year == _viewModel.CurrentYear));
            }

            await _viewModel.TransferVacation(transferedVacation);
            int index = 0;
            for(int i = 0; i < VacationAllowances.Count; i++)
            {
                if(VacationAllowances[i].Vacation_Name == transferedVacation.Name)
                {
                    index = i;
                    break;
                }
            }

            if(App.SelectedMode == WindowMode.Personal)
            {
                App.API.Person.User_Vacation_Allowances[index].Vacation_Days_Quantity += transferedVacation.Count;
                await _viewModel.UpdateVacationAllowance(transferedVacation.UserId, transferedVacation.VacationId, transferedVacation.DateStart.Year, App.API.Person.User_Vacation_Allowances[index].Vacation_Days_Quantity);
                App.API.Person.User_Vacations.Remove(transferedVacation);
            } else if(App.SelectedMode == WindowMode.Subordinate || App.SelectedMode == WindowMode.HR_GOD)
            {
                _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances[index].Vacation_Days_Quantity += transferedVacation.Count;
                await _viewModel.UpdateVacationAllowance(transferedVacation.UserId, transferedVacation.VacationId, transferedVacation.DateStart.Year, _viewModel.SelectedSubordinate.Subordinate_Vacation_Allowances[index].Vacation_Days_Quantity);
                _viewModel.SelectedSubordinate.Subordinate_Vacations.Remove(transferedVacation);
                foreach(Subordinate subordinate in App.API.Person.Subordinates)
                {
                    if(subordinate.Id_SAP == transferedVacation.UserId)
                    {
                        subordinate.Subordinate_Vacations.Remove(transferedVacation);
                    }
                }
            }

            _viewModel.PlannedIndex = 0;
        }
    }
}
