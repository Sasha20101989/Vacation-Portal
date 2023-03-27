using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.Services.Providers.Interfaces
{
    public interface IStateRepository
    {
        ObservableCollection<SvApprovalStateViewModel> PersonStates { get; set; }
        Task<ObservableCollection<SvApprovalStateViewModel>> GetStateVacationsOnApproval(int UserIdSAP);
        Task UpdateStateStatusAsync(SvApprovalStateViewModel state);
    }
}
