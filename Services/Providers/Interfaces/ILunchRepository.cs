using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.Services.Providers.Interfaces
{
    public interface ILunchRepository
    {
        Person Person { get; set; }
        List<HolidayViewModel> Holidays { get; set; }
        Action<List<HolidayViewModel>> OnHolidaysChanged { get; set; }
        Task<IEnumerable<PersonDTO>> LoginAsync(string account);
        Task LogoutAsync();
        Task<IEnumerable<HolidayViewModel>> GetHolidaysAsync(DateTime start, DateTime end);
        Task<IEnumerable<Settings>> GetSettingsAsync(string account);
        Task<IEnumerable<Access>> GetAccessAsync(string account);
        Task<IEnumerable<Holiday>> GetHolidayTypesAsync();
        Task<IEnumerable<HolidayViewModel>> GetHolidaysAsync();
        Task AddHolidayAsync(HolidayViewModel holiday);
        Task DeleteHolidayAsync(HolidayViewModel holiday);
        Task<IEnumerable<VacationAllowance>> GetVacationsAsync(int UserIdSAP, string year);
    }
}
