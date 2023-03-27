using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;

namespace Vacation_Portal.Services.Providers.Interfaces
{
    public interface IHolidayRepository
    {
        ICommand LoadHolidays { get; }
        ICommand LoadHolidayTypes { get; }
        ObservableCollection<HolidayViewModel> Holidays { get; set; }
        ObservableCollection<Holiday> HolidayTypes { get; set; }
        Action<ObservableCollection<HolidayViewModel>> OnHolidaysChanged { get; set; }
        Action<ObservableCollection<Holiday>> OnHolidayTypesChanged { get; set; }
        Task<IEnumerable<HolidayViewModel>> GetHolidaysAsync(int yearCurrent, int yearNext);
        Task<IEnumerable<Holiday>> GetHolidayTypesAsync();
        Task AddHolidayAsync(HolidayViewModel holiday);
        Task DeleteHolidayAsync(HolidayViewModel holiday);
    }
}
