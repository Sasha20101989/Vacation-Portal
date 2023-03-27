using System;
using System.Collections.Generic;
using System.Text;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Services.Providers.Interfaces
{
   public interface ICalendarRepository
    {
        DateTime DateUnblockNextCalendar { get; set; }
        DateTime DateUnblockPlanning { get; set; }
        bool IsCalendarUnblocked { get; set; }
        bool IsCalendarPlannedOpen { get; }
        bool CheckDateUnblockedCalendarAsync();
        bool CheckNextCalendarPlanningUnlock();
        IEnumerable<CalendarSettings> GetSettingsForCalendar();
    }
}
