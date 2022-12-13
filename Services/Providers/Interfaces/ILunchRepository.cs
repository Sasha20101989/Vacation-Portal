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
        DateTime DateUnblockNextCalendar { get; set; }
        DateTime DateUnblockPlanning { get; set; }
        bool IsCalendarUnblocked { get; set; }
        bool IsCalendarPlannedOpen { get; set; }
        List<HolidayViewModel> Holidays { get; set; }
        //List<VacationViewModel> Vacations { get; set; }
        Action<List<HolidayViewModel>> OnHolidaysChanged { get; set; }
        Action<List<VacationViewModel>> OnVacationsChanged { get; set; }
        Action<Access> OnAccessChanged { get; set; }
        //Task<IEnumerable<SubordinateDTO>> GetSubordinateAsync(int Id_SAP);
        Task<IEnumerable<PersonDTO>> LoginAsync(string account);
        Task<IEnumerable<HolidayViewModel>> GetHolidaysAsync(int yearCurrent, int yearNext);
        Task<IEnumerable<Settings>> GetSettingsAsync(string account);
        Task<IEnumerable<Access>> GetAccessAsync(string account);
        Task<IEnumerable<Holiday>> GetHolidayTypesAsync();
        Task AddHolidayAsync(HolidayViewModel holiday);
        Task DeleteHolidayAsync(HolidayViewModel holiday);
        Task<IEnumerable<VacationAllowanceViewModel>> GetVacationAllowanceAsync(int UserIdSAP, int year);
        Task UpdateVacationAllowanceAsync(int vacation_Id, int year, int count);
        Task AddVacationAsync(Vacation vacation);
        Task DeleteVacationAsync(Vacation vacation);
        Task<IEnumerable<VacationViewModel>> LoadVacationAsync(int UserIdSAP, int year);
        Task<IEnumerable<VacationDTO>> GetConflictingVacationAsync(Vacation vacation);
    }
}
