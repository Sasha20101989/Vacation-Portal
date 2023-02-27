using System;

namespace Vacation_Portal.MVVM.Models {
    public class CalendarSettings {
        public string Setting_Name { get; set; }
        public DateTime Setting_Date { get; set; }

        public CalendarSettings(string settingName, DateTime settingDate) {
            Setting_Name = settingName;
            Setting_Date = settingDate;
        }
    }
}
