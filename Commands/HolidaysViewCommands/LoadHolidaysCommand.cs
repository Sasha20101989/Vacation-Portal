using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.HolidaysViewCommands
{
    public class LoadHolidaysCommand : CommandBase
    {
        private HolidaysViewModel _viewModel;

        public LoadHolidaysCommand(HolidaysViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object parameter)
        {
            _viewModel.IsLoading = true;
            MessageBox.Show("Произошла бы загрузка, но нет связи с базой");
            _viewModel.IsLoading = false;
        }
    }
}
