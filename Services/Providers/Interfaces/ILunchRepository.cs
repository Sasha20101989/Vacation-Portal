using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.Services.Providers.Interfaces {
    public interface ILunchRepository {
        Person Person { get; set; }
        ObservableCollection<Subordinate> PersonsWithVacationsOnApproval { get; set; }
        List<Status> AllStatuses { get; set; }
        DateTime DateUnblockNextCalendar { get; set; }
        DateTime DateUnblockPlanning { get; set; }
        bool IsCalendarUnblocked { get; set; }
        bool CheckDateUnblockedCalendarAsync();
        bool IsCalendarPlannedOpen { get; }
        bool CheckNextCalendarPlanningUnlock();
        void GetPersonsWithVacationsOnApproval();
        ICommand LoadHolidays { get; }
        ICommand LoadHolidayTypes { get; }
        ICommand Login { get; }

        ObservableCollection<Vacation> ProcessedVacations { get; set; }

        ObservableCollection<Vacation> VacationsOnApproval { get; set; }

        ObservableCollection<HolidayViewModel> Holidays { get; set; }
        ObservableCollection<Holiday> HolidayTypes { get; set; }
        Action<ObservableCollection<HolidayViewModel>> OnHolidaysChanged { get; set; }
        Action<ObservableCollection<Holiday>> OnHolidayTypesChanged { get; set; }
        Action<List<VacationViewModel>> OnVacationsChanged { get; set; }
        Action<Person> OnLoginSuccess { get; set; }
        List<Status> GetStatuses();
        Task<Person> LoginAsync(string account);
        Task<IEnumerable<HolidayViewModel>> GetHolidaysAsync(int yearCurrent, int yearNext);
        Task<IEnumerable<Holiday>> GetHolidayTypesAsync();
        Task AddHolidayAsync(HolidayViewModel holiday);
        Task DeleteHolidayAsync(HolidayViewModel holiday);
        Task<IEnumerable<VacationAllowanceViewModel>> GetVacationAllowanceAsync(int UserIdSAP);
        Task UpdateVacationAllowanceAsync(int userIdSAP, int vacation_Id, int year, int count);
        Task<Vacation> AddVacationAsync(Vacation vacation);
        Task DeleteVacationAsync(Vacation vacation);
        Task<IEnumerable<Vacation>> LoadVacationsAsync(int UserIdSAP);
        Task<IEnumerable<Vacation>> LoadAllVacationsAsync();
        Task<IEnumerable<VacationDTO>> GetConflictingVacationAsync(Vacation vacation);
        Task UpdateVacationStatusAsync(Vacation vacation);
        IEnumerable<CalendarSettings> GetSettingsForCalendar();

    }
}
