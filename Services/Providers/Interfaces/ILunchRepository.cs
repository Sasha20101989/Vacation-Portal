using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
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
        ICommand LoadHolidays { get; }
        ICommand LoadHolidayTypes { get; }
        ICommand Login { get; }
        ObservableCollection<HolidayViewModel> Holidays { get; set; }
        ObservableCollection<Holiday> HolidayTypes { get; set; }
        //List<VacationViewModel> Vacations { get; set; }
        Action<ObservableCollection<HolidayViewModel>> OnHolidaysChanged { get; set; }
        Action<ObservableCollection<Holiday>> OnHolidayTypesChanged { get; set; }
        Action<List<VacationViewModel>> OnVacationsChanged { get; set; }
        Action<Person> OnLoginSuccess { get; set; }
        Action<Access> OnAccessChanged { get; set; }
        //Task<IEnumerable<SubordinateDTO>> GetSubordinateAsync(int Id_SAP);
        Task<Person> LoginAsync(string account);
        Task<IEnumerable<HolidayViewModel>> GetHolidaysAsync(int yearCurrent, int yearNext);
        Task<IEnumerable<Settings>> GetSettingsAsync(string account);
        Task<IEnumerable<Access>> GetAccessAsync(string account);
        Task<IEnumerable<Holiday>> GetHolidayTypesAsync();
        Task AddHolidayAsync(HolidayViewModel holiday);
        Task DeleteHolidayAsync(HolidayViewModel holiday);
        Task<IEnumerable<VacationAllowanceViewModel>> GetVacationAllowanceAsync(int UserIdSAP);
        Task UpdateVacationAllowanceAsync(int userIdSAP, int vacation_Id, int year, int count);
        Task AddVacationAsync(Vacation vacation);
        Task DeleteVacationAsync(Vacation vacation);
        Task<IEnumerable<VacationViewModel>> LoadVacationAsync(int UserIdSAP);
        Task<IEnumerable<VacationDTO>> GetConflictingVacationAsync(Vacation vacation);
    }
}
