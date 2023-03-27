using System.Collections.Generic;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Commands {
    public class LoadHolidayTypesCommand : CommandBase {
        public override async void Execute(object parameter) {
            await foreach(Holiday item in FetchHolidayTypesAsync()) {
                App.HolidayAPI.HolidayTypes.Add(item);
            }
            App.HolidayAPI.OnHolidayTypesChanged?.Invoke(App.HolidayAPI.HolidayTypes);
        }
        public async IAsyncEnumerable<Holiday> FetchHolidayTypesAsync() {
            IEnumerable<Holiday> holidayTypes = await App.HolidayAPI.GetHolidayTypesAsync();

            foreach(Holiday item in holidayTypes) {
                yield return item;
            }
        }
    }
}
