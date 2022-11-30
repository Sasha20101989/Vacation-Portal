using System.Threading.Tasks;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands
{
    public class LoginCommand : AsyncComandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;
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
