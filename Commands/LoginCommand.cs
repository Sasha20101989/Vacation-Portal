using System;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands
{
    public class LoginCommand : AsyncComandBase
    {
        public override async Task ExecuteAsync(object parameter)
        {
            App.Current.Dispatcher.Invoke((Action) delegate
            {
                App.SplashScreen.status.Text = "Ищу вас в базе данных...";
            });
            //_ = await App.API.LoginAsyncNew(Environment.UserName);
            _ = await App.API.LoginAsync(Environment.UserName);
        }
    }
}
