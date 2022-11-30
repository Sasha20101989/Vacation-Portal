using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.HolidaysViewCommands
{
    public class AddHolidayCommand : CommandBase
    {
        private readonly HolidaysViewModel _viewModel;
        public AddHolidayCommand(HolidaysViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override void Execute(object parameter)
        {
            _viewModel.IsSaving = true;
            bool IsDayOff = _viewModel.CurrentDate.DayOfWeek.ToString("d") == "6" || _viewModel.CurrentDate.DayOfWeek.ToString("d") == "0";
            if(_viewModel.SelectedItem.NameOfHoliday == "Рабочий в выходной" && !IsDayOff)
            {
                _viewModel.MessageQueue.Enqueue("Этот день не является выходным");
            } else if(_viewModel.SelectedItem.NameOfHoliday == "Выходной" && IsDayOff)
            {
                _viewModel.MessageQueue.Enqueue("Этот день уже является выходным");
            } else
            {
                if(!_viewModel.Holidays.Contains(new HolidayViewModel(_viewModel.SelectedItem.NameOfHoliday, _viewModel.CurrentDate)))
                {
                    App.API.Holidays.Add(new HolidayViewModel(_viewModel.SelectedItem.NameOfHoliday, _viewModel.CurrentDate));
                    _viewModel.Holidays.Add(new HolidayViewModel(_viewModel.SelectedItem.NameOfHoliday, _viewModel.CurrentDate));
                    App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
                } else
                {
                    foreach(HolidayViewModel item in _viewModel.Holidays)
                    {
                        if(item.Date == _viewModel.CurrentDate)
                        {
                            _viewModel.SelectedHoliday = new HolidayViewModel(item.TypeOfHoliday, item.Date);
                        }
                    }
                    _viewModel.MessageQueue.Enqueue("Такой день уже есть в списке");
                }
            }
            _viewModel.IsSaving = false;
        }
    }
}
