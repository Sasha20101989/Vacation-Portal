using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Vacation_Portal.DbContext;
using Vacation_Portal.DTOs;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.Services.Providers.Interfaces;

namespace Vacation_Portal.Services.Providers
{
    public class CalendarRepository: ICalendarRepository
    {
        private readonly SqlDbConnectionFactory _sqlDbConnectionFactory;
        public CalendarRepository(SqlDbConnectionFactory sqlDbConnectionFactory)
        {
            _sqlDbConnectionFactory = sqlDbConnectionFactory;
        }

        public DateTime DateUnblockNextCalendar { get; set; }
        public DateTime DateUnblockPlanning { get; set; }
        public bool IsCalendarUnblocked { get; set; }

        public bool IsCalendarPlannedOpen { get; set; } = true;

        public bool CheckDateUnblockedCalendarAsync()
        {
            IEnumerable<CalendarSettings> calendarSettings = GetSettingsForCalendar();
            foreach(CalendarSettings settings in calendarSettings)
            {
                if(settings.Setting_Name == "NextCalendarUnlock")
                {
                    DateUnblockNextCalendar = settings.Setting_Date;
                    IsCalendarUnblocked = DateUnblockNextCalendar <= DateTime.UtcNow;
                    return IsCalendarUnblocked;
                }
            }
            return false;
        }

        public bool CheckNextCalendarPlanningUnlock()
        {
            IEnumerable<CalendarSettings> calendarSettings = GetSettingsForCalendar();
            foreach(CalendarSettings settings in calendarSettings)
            {
                if(settings.Setting_Name == "NextCalendarPlanningUnlock")
                {
                    DateUnblockPlanning = settings.Setting_Date;
                    IsCalendarPlannedOpen = DateUnblockPlanning <= DateTime.UtcNow;
                    return IsCalendarPlannedOpen;
                }
            }
            return false;
        }

        public IEnumerable<CalendarSettings> GetSettingsForCalendar()
        {
            using IDbConnection database = _sqlDbConnectionFactory.Connect();
            try
            {
                IEnumerable<CalendarSettingsDTO> settingsDTOs = database.Query<CalendarSettingsDTO>("usp_Load_Settings_For_Calendar", commandType: CommandType.StoredProcedure);
                return settingsDTOs.Select(ToSettings);
            } catch(Exception)
            {
                return null;
            }
        }
        private CalendarSettings ToSettings(CalendarSettingsDTO dto)
        {
            return new CalendarSettings(dto.Static_Date_Name, dto.Static_Date);
        }
    }
}
