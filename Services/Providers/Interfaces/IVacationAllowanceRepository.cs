using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.Services.Providers.Interfaces
{
    public interface IVacationAllowanceRepository
    {
        ObservableCollection<VacationAllowanceViewModel> VacationAllowances { get; set; }
        Task<IEnumerable<VacationAllowanceViewModel>> GetVacationAllowanceAsync(int UserIdSAP);
        Task UpdateVacationAllowanceAsync(int userIdSAP, int vacation_Id, int year, int count);
    }
}
