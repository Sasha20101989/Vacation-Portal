using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.NavigationCommands
{
    public class GoToNextPageCommand : CommandBase
    {
        private MainWindowViewModel _mainWindowViewModel;
        public GoToNextPageCommand(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public override void Execute(object parameter)
        {
            if (_mainWindowViewModel.SelectedIndex < _mainWindowViewModel.MenuItems.Count - 1)
            {
                if (!string.IsNullOrWhiteSpace(_mainWindowViewModel.SearchKeyword))
                    _mainWindowViewModel.SearchKeyword = string.Empty;

                _mainWindowViewModel.SelectedIndex++;
            }
        }
    }
}
