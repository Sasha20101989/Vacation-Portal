using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.HorizontalCalendarCommands
{
    public class ApproveStateCommand : AsyncComandBase
    {
        private readonly StateToApproveViewModel _viewModel;

        public ApproveStateCommand(StateToApproveViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            _viewModel.IsSave = true;
            ObservableCollection<SvApprovalStateViewModel> statesWithoutOnApproval = parameter as ObservableCollection<SvApprovalStateViewModel>;
            IEnumerable<IGrouping<int, SvApprovalStateViewModel>> groupedStates = statesWithoutOnApproval.OrderBy(s => s.Vacation.UserId).GroupBy(s => s.Vacation.UserId);
            
            foreach(var group in groupedStates)
            {
                bool hasDeclineStatus = false;
                foreach(SvApprovalStateViewModel state in group)
                {
                    if(state.StatusId == (int) Statuses.NotAgreed)
                    {
                        hasDeclineStatus = true;
                        break;
                    }
                }
                if(!hasDeclineStatus)
                {
                    foreach(SvApprovalStateViewModel state in group)
                    {
                        await App.VacationAPI.UpdateVacationStatusAsync(state, (int)Statuses.PassedToHR);
                        await App.StateAPI.DeleteStateAsync(state.Id);
                        App.StateAPI.PersonStates.Remove(state);
                    }
                } else
                {
                    foreach(SvApprovalStateViewModel state in group)
                    {
                        await App.VacationAPI.UpdateVacationStatusAsync(state, state.StatusId);
                        await App.StateAPI.DeleteStateAsync(state.Id);
                        App.StateAPI.PersonStates.Remove(state);
                    }
                }
            }

            //App.StateAPI.PersonStates = await App.StateAPI.GetStateVacationsOnApproval(App.API.Person.Id_SAP);
            App.StateAPI.PersonStatesChanged.Invoke(App.StateAPI.PersonStates);
            foreach(Subordinate subordinate in App.API.Person.Subordinates)
            {
                subordinate.UpdateStatesCount(false);
            }
            

            Timer timer1 = new Timer(3000);
            timer1.Elapsed += Timer1_Elapsed;
            timer1.Enabled = true;


        }
        private void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(sender is Timer timer)
            {
                _viewModel.IsSave = false;
                timer.Enabled = false;
            }
        }
    }
}
