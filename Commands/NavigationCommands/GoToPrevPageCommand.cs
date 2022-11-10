using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace UI.Commands.NavigationCommands
{
    public class GoToPrevPageCommand : CommandBase
    {
        private MainWindowViewModel _mainWindowViewModel;

        public GoToPrevPageCommand(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }
        public override void Execute(object parameter)
        {
            if (_mainWindowViewModel.SelectedIndex > 0)
            {
                if (!string.IsNullOrWhiteSpace(_mainWindowViewModel.SearchKeyword))
                    _mainWindowViewModel.SearchKeyword = string.Empty;

                _mainWindowViewModel.SelectedIndex--;
            }
        }
    }
}
