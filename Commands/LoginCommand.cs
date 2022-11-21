using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands
{
   public class LoginCommand:AsyncComandBase
    {
        private MainWindowViewModel _mainWindowViewModel;
        public LoginCommand(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public override async Task ExecuteAsync(object parameter)
        {
            await Task.Run(async () => await _mainWindowViewModel.GetUserAsync());
        }
    }
}
