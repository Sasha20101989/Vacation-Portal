using System;
using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;

namespace Vacation_Portal.Commands
{
    public class LoginCommand : AsyncComandBase
    {
        public override async Task ExecuteAsync(object parameter)
        {
            _ = await App.API.LoginAsync(Environment.UserName);
        }
    }
}
