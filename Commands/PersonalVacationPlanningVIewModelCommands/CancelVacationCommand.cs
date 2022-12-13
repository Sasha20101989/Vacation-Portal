using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class CancelVacationCommand : CommandBase
    {
        private readonly PersonalVacationPlanningViewModel _viewModel;

        public CancelVacationCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object parameter)
        {
            Vacation deletedItem = (Vacation) parameter;
            if(deletedItem != null)
            {
                int index = 0;
                for(int i = 0; i < _viewModel.VacationAllowances.Count; i++)
                {
                    if(_viewModel.VacationAllowances[i].Vacation_Name == deletedItem.Name)
                    {
                        index = i;
                        break;
                    }
                }
                _viewModel.VacationAllowances[index].Vacation_Days_Quantity += deletedItem.Count;
                await _viewModel.UpdateVacationAllowance(deletedItem.Vacation_Id, deletedItem.Date_Start.Year, _viewModel.VacationAllowances[index].Vacation_Days_Quantity);
                await _viewModel.DeleteVacation(deletedItem);
                _viewModel.VacationsToAproval.Remove(deletedItem);
                _viewModel.VacationsToAprovalFromDataBase.Remove(deletedItem);
                _viewModel.PlannedIndex = 0;
                _viewModel.Calendar.UpdateColor();
            }
        }
    }
}
