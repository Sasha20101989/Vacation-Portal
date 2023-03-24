using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.HorizontalCalendarCommands
{
    public class ApproveVacationCommand : AsyncComandBase
    {
        private readonly HorizontalCalendarViewModel _horizontalCalendarViewModel;

        public ApproveVacationCommand(HorizontalCalendarViewModel horizontalCalendarViewModel)
        {
            _horizontalCalendarViewModel = horizontalCalendarViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if(parameter is SvApprovalStateViewModel state)
            {
                _horizontalCalendarViewModel.IsLoading = true;
                state.StatusId = (int) Statuses.Approved;
                state.Vacation.Vacation_Status_Id = state.StatusId;
                await App.API.UpdateStateStatusAsync(state);
                await _horizontalCalendarViewModel.VacationListViewModel.LoadStatesAsync(_horizontalCalendarViewModel.SelectedSubordinate);
                _horizontalCalendarViewModel.SelectedSubordinate.UpdateStatesCount();
            }
        }
    }
}
