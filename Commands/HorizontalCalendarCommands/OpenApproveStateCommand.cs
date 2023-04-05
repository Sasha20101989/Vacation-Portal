using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Views;

namespace Vacation_Portal.Commands.HorizontalCalendarCommands
{
    public class OpenApproveStateCommand : CommandBase
    {
        private readonly HorizontalCalendarViewModel _viewModel;
        private readonly StateToApproveViewModel _stateToApproveViewViewModel;
        private readonly StateToApproveView _view = new StateToApproveView();

        public OpenApproveStateCommand(HorizontalCalendarViewModel viewModel)
        {
            _viewModel = viewModel;
            _stateToApproveViewViewModel = new StateToApproveViewModel();
        }

        public override void Execute(object parameter)
        {
            _view.DataContext = _viewModel;
            _stateToApproveViewViewModel.GetStatesWithoutOnApproval(_viewModel.PersonStates);
            _viewModel.StateToApproveViewModel = _stateToApproveViewViewModel;
            Task<object> openView = DialogHost.Show(_view, "RootDialog", _viewModel.ExtendedClosingEventHandler);
        }
    }
}
