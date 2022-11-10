﻿using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace UI.Commands.NotificationCommands
{
    public class DismissAllNotificationsCommand : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public DismissAllNotificationsCommand(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }
        public override void Execute(object parameter)
        {
            _mainWindowViewModel.MenuItems[0].DismissAllNotifications();
        }
    }
}
