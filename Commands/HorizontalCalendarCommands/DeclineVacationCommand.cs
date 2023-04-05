using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.HorizontalCalendarCommands
{
    public class DeclineVacationCommand : AsyncComandBase
    {
        private readonly HorizontalCalendarViewModel _horizontalCalendarViewModel;

        public DeclineVacationCommand(HorizontalCalendarViewModel horizontalCalendarViewModel)
        {
            _horizontalCalendarViewModel = horizontalCalendarViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if(parameter is SvApprovalStateViewModel state)
            {
                await App.StateAPI.UpdateStateStatusAsync(state, (int) Statuses.NotAgreed);
                _horizontalCalendarViewModel.VacationListViewModel.LoadStatesAsync(_horizontalCalendarViewModel.SelectedSubordinate);
                _horizontalCalendarViewModel.SelectedSubordinate.UpdateStatesCount(false);
            }
        }
    }
}