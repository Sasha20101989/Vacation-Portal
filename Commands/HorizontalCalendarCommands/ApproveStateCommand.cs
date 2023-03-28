using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.HorizontalCalendarCommands
{
    public class ApproveStateCommand : AsyncComandBase
    {
        private readonly HorizontalCalendarViewModel _horizontalCalendarViewModel;

        public ApproveStateCommand(HorizontalCalendarViewModel horizontalCalendarViewModel)
        {
            _horizontalCalendarViewModel = horizontalCalendarViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            if(parameter is ObservableCollection<SvApprovalStateViewModel> states)
            {
                foreach(SvApprovalStateViewModel state in states)
                {
                    await App.VacationAPI.UpdateVacationStatusAsync(state.VacationRecordId, state.StatusId);
                }
                App.StateAPI.PersonStates = await App.StateAPI.GetStateVacationsOnApproval(App.API.Person.Id_SAP);
                foreach(Subordinate subordinate in App.API.Person.Subordinates)
                {
                    subordinate.UpdateStatesCount();
                }

            }
           
        }
    }
}
