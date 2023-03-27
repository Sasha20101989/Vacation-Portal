using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.Services.Providers.Interfaces
{
    public interface IVacationRepository
    {
        ObservableCollection<Vacation> Vacations { get; set; }
        Action<List<VacationViewModel>> OnVacationsChanged { get; set; }
        ObservableCollection<Vacation> ProcessedVacations { get; set; }
        ObservableCollection<Vacation> VacationsOnApproval { get; set; }
        Task<Vacation> AddVacationAsync(Vacation vacation);
        Task DeleteVacationAsync(Vacation vacation);
        Task<IEnumerable<Vacation>> LoadVacationsAsync(int UserIdSAP);
        Task<IEnumerable<VacationDTO>> GetConflictingVacationAsync(Vacation vacation);
        Task UpdateVacationStatusAsync(int vacationId, int statusId);
    }
}
