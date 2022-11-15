using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands
{
    public class AddToApprovalListCommand : CommandBase
    {
        PersonalVacationPlanningViewModel _viewModel;
        public AddToApprovalListCommand(PersonalVacationPlanningViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            int remainder = _viewModel.SelectedItem.Count - _viewModel.CountSelectedDays;
            bool isMorePlanedDays = remainder >= 0;
            if (isMorePlanedDays)
            {
                _viewModel.SelectedItem.Count -= _viewModel.CountSelectedDays;
            }   
        }
    }
}
