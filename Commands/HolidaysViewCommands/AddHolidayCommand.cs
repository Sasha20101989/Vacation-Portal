using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            } else if(_viewModel.SelectedItem.NameOfHoliday == "Внеплановый" && IsDayOff)
            {
                _viewModel.MessageQueue.Enqueue("Этот день уже является выходным");
            } else
            {
                if(_viewModel.CurrentDate.Year == DateTime.Now.Year)
                {
                    if(!_viewModel.HolidaysCurrentYear.Contains(new HolidayViewModel(_viewModel.SelectedItem.Id, _viewModel.SelectedItem.NameOfHoliday, _viewModel.CurrentDate, Convert.ToInt32(_viewModel.CurrentDate.Year))))
                    {
                        App.API.Holidays.Add(new HolidayViewModel(_viewModel.SelectedItem.Id, _viewModel.SelectedItem.NameOfHoliday, _viewModel.CurrentDate, Convert.ToInt32(_viewModel.CurrentDate.Year)));
                        _viewModel.HolidaysCurrentYear.Add(new HolidayViewModel(_viewModel.SelectedItem.Id, _viewModel.SelectedItem.NameOfHoliday, _viewModel.CurrentDate, Convert.ToInt32(_viewModel.CurrentDate.Year)));
                        _viewModel.HolidaysCurrentYear = new ObservableCollection<HolidayViewModel>(_viewModel.HolidaysCurrentYear.OrderBy(i => i.Date));
                        App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
                        App.API.AddHolidayAsync(new HolidayViewModel(_viewModel.SelectedItem.Id, _viewModel.SelectedItem.NameOfHoliday, _viewModel.CurrentDate, Convert.ToInt32(_viewModel.CurrentDate.Year)));
                    } else
                    {
                        foreach(HolidayViewModel item in _viewModel.HolidaysCurrentYear)
                        {
                            if(item.Date == _viewModel.CurrentDate)
                            {
                                _viewModel.SelectedCurrentYearHoliday = new HolidayViewModel(_viewModel.SelectedItem.Id, item.TypeOfHoliday, item.Date, Convert.ToInt32(item.Date.Year));
                            }
                        }
                        _viewModel.MessageQueue.Enqueue("Такой день уже есть в списке");
                    }
                } else if(_viewModel.CurrentDate.Year == _viewModel.NextDate.Year)
                {
                    if(!_viewModel.HolidaysNextYear.Contains(new HolidayViewModel(_viewModel.SelectedItem.Id, _viewModel.SelectedItem.NameOfHoliday, _viewModel.CurrentDate, Convert.ToInt32(_viewModel.CurrentDate.Year))))
                    {
                        App.API.Holidays.Add(new HolidayViewModel(_viewModel.SelectedItem.Id, _viewModel.SelectedItem.NameOfHoliday, _viewModel.CurrentDate, Convert.ToInt32(_viewModel.CurrentDate.Year)));
                        _viewModel.HolidaysNextYear.Add(new HolidayViewModel(_viewModel.SelectedItem.Id, _viewModel.SelectedItem.NameOfHoliday, _viewModel.CurrentDate, Convert.ToInt32(_viewModel.CurrentDate.Year)));
                        _viewModel.HolidaysNextYear = new ObservableCollection<HolidayViewModel>(_viewModel.HolidaysNextYear.OrderBy(i => i.Date));
                        App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
                        App.API.AddHolidayAsync(new HolidayViewModel(_viewModel.SelectedItem.Id, _viewModel.SelectedItem.NameOfHoliday, _viewModel.CurrentDate, Convert.ToInt32(_viewModel.CurrentDate.Year)));
                    } else
                    {
                        foreach(HolidayViewModel item in _viewModel.HolidaysNextYear)
                        {
                            if(item.Date == _viewModel.CurrentDate)
                            {
                                _viewModel.SelectedNextYearHoliday = new HolidayViewModel(_viewModel.SelectedItem.Id, item.TypeOfHoliday, item.Date, Convert.ToInt32(item.Date.Year));
                            }
                        }
                        _viewModel.MessageQueue.Enqueue("Такой день уже есть в списке");
                    }
                } else
                {
                    _viewModel.MessageQueue.Enqueue("Вы не можете запланировать на этот год");
                }
            }
            _viewModel.IsSaving = false;
        }
    }
}
