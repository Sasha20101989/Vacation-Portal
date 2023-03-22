using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.Commands.HorizontalCalendarCommands
{
    public class SelectedSubordinateCommand : CommandBase
    {
        private readonly HorizontalCalendarViewModel _viewModel;
        private readonly VacationListViewModel _vacationListViewModel;
        private readonly VacationsToApprovalPerPersonView _view = new VacationsToApprovalPerPersonView();
        public SelectedSubordinateCommand(HorizontalCalendarViewModel viewModel)
        {
            _viewModel = viewModel;
            _vacationListViewModel = new VacationListViewModel();
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override async void Execute(object parameter)
        {

            var isSubordinate = parameter is Subordinate;
            if(isSubordinate)
            {
                var selectedSubordinate = parameter as Subordinate;
                var countVacations = selectedSubordinate.Subordinate_Vacations.Count;
                if(countVacations > 0)
                {
                    _view.DataContext = _viewModel;
                    await _vacationListViewModel.LoadVacationsAsync(selectedSubordinate);
                    Task<object> openCheck = DialogHost.Show(_view, "RootDialog", _viewModel.ExtendedClosingEventHandler);
                }
            }
        }
    }
}
