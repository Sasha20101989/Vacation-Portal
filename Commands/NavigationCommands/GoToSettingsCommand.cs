using MaterialDesignThemes.Wpf;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.ViewModels.ForPages;
using Vacation_Portal.MVVM.Views;

namespace UI.Commands.NavigationCommands {
    public class GoToSettingsCommand : CommandBase {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public GoToSettingsCommand(MainWindowViewModel mainWindowViewModel) {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public override void Execute(object parameter) {
            _mainWindowViewModel.SearchKeyword = string.Empty;
            _mainWindowViewModel.SelectedItem = new MenuItem(
               "Settings",
               typeof(SettingsView),
               selectedIcon: PackIconKind.Cog,
               unselectedIcon: PackIconKind.CogOutline,
               new SettingsViewModel(_mainWindowViewModel));
        }
    }
}
