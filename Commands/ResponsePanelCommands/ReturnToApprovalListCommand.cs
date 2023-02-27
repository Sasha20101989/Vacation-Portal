using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.ResponsePanelCommands
{
    public class ReturnToApprovalListCommand : CommandBase
    {
        private readonly ResponsePanelViewModel _responsePanelViewModel;

        public ReturnToApprovalListCommand(ResponsePanelViewModel responsePanelViewModel)
        {
            _responsePanelViewModel = responsePanelViewModel;
        }
        public override void Execute(object parameter)
        {
            Vacation vacationItem = (Vacation) parameter;
            _responsePanelViewModel.ProcessedVacations.Remove(vacationItem);
            _responsePanelViewModel.VacationsOnApproval.Add(vacationItem);
        }
    }
}
