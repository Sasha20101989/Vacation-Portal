using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.ResponsePanelCommands
{
    public class ReturnToApprovalListCommand : CommandBase
    {
        ResponsePanelViewModel _responsePanelViewModel;

        public ReturnToApprovalListCommand(ResponsePanelViewModel responsePanelViewModel)
        {
            _responsePanelViewModel = responsePanelViewModel;
        }
        public override void Execute(object parameter)
        {
            var vacationItem = (Vacation) parameter;
            _responsePanelViewModel.ProcessedVacations.Remove(vacationItem);
            _responsePanelViewModel.VacationsOnApproval.Add(vacationItem);
        }
    }
}
