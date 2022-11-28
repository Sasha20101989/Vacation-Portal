using Vacation_Portal.Commands.BaseCommands;
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
            App.API.Holidays.Remove(_viewModel.SelectedHoliday);
            _viewModel.Holidays.Remove(_viewModel.SelectedHoliday);
            App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
        }
    }
}
