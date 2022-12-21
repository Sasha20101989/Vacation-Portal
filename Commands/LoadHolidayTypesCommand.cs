using System.Collections.Generic;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Commands
{
    public class LoadHolidayTypesCommand : CommandBase
    {
        public override async void Execute(object parameter)
        {
            await foreach(Holiday item in FetchHolidayTypesAsync())
            {
                App.API.HolidayTypes.Add(item);
            }
            App.API.OnHolidayTypesChanged?.Invoke(App.API.HolidayTypes);
        }
        public async IAsyncEnumerable<Holiday> FetchHolidayTypesAsync()
        {
            IEnumerable<Holiday> holidayTypes = await App.API.GetHolidayTypesAsync();

            foreach(Holiday item in holidayTypes)
            {
                yield return item;
            }
        }
    }
}
