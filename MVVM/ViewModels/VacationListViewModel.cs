using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.Commands.HorizontalCalendarCommands
{
    public class VacationListViewModel
    {
        public Subordinate Subordinate { get; internal set; }
        public ObservableCollection<SvApprovalStateViewModel> States { get; set; }

        public ObservableCollection<SvApprovalStateViewModel> LoadStates(Subordinate selectedSubordinate)
        {
            Subordinate = selectedSubordinate;

            ObservableCollection<SvApprovalStateViewModel> svApprovalStateViewModels =
                new ObservableCollection<SvApprovalStateViewModel>(App.API.PersonStates.Where(s => s.Vacation.User_Id_SAP == Subordinate.Id_SAP));

            States = svApprovalStateViewModels;
            return States;
        }
    }
}