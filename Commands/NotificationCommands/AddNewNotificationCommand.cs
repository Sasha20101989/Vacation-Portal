using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.NotificationCommands
{
    public class AddNewNotificationCommand : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public AddNewNotificationCommand(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }
        public override void Execute(object parameter)
        {
            _mainWindowViewModel.MenuItems[0].AddNewNotification();
        }
    }
}
