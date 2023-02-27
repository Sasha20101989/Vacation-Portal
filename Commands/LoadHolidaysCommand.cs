using System;
using System.Collections.Generic;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.Commands {
    public class LoadHolidaysCommand : CommandBase {
        public DateTime CurrentDate { get; set; } = DateTime.Now;
        public override async void Execute(object parameter) {
            await foreach(HolidayViewModel item in FetchHolidaysAsync()) {
                App.API.Holidays.Add(item);
            }
            App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
        }
        public async IAsyncEnumerable<HolidayViewModel> FetchHolidaysAsync() {
            IEnumerable<HolidayViewModel> holidays = await App.API.GetHolidaysAsync(CurrentDate.Year, CurrentDate.Year + 1);

            foreach(HolidayViewModel item in holidays) {
                yield return item;
            }
        }
    }
}
