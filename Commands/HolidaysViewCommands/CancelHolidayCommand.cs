using System;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.HolidaysViewCommands
{
    public class CancelHolidayCommand : CommandBase
    {
        private readonly HolidaysViewModel _viewModel;
        public CancelHolidayCommand(HolidaysViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            if(parameter is HolidayViewModel)
            {
                HolidayViewModel holiday = parameter as HolidayViewModel;
                if(holiday.Date.Year == DateTime.Now.Year)
                {
                    App.API.DeleteHolidayAsync(_viewModel.SelectedCurrentYearHoliday);
                    App.API.Holidays.Remove(_viewModel.SelectedCurrentYearHoliday);
                    _viewModel.HolidaysCurrentYear.Remove(_viewModel.SelectedCurrentYearHoliday);
                    App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
                } else
                {
                    App.API.DeleteHolidayAsync(_viewModel.SelectedNextYearHoliday);
                    App.API.Holidays.Remove(_viewModel.SelectedNextYearHoliday);
                    _viewModel.HolidaysNextYear.Remove(_viewModel.SelectedNextYearHoliday);
                    App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
                }
            }
        }
    }
}
