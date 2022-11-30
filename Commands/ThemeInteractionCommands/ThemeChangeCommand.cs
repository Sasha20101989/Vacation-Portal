using MaterialDesignThemes.Wpf;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.Commands.ThemeInteractionCommands
{
    public class ThemeChangeCommand : CommandBase
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public ThemeChangeCommand(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            mainWindowViewModel.IsCheckedTheme = theme.GetBaseTheme() == BaseTheme.Dark;
        }

        public override void Execute(object parameter)
        {
            bool isDarkTheme = _mainWindowViewModel.IsCheckedTheme ? _mainWindowViewModel.IsCheckedTheme = false : _mainWindowViewModel.IsCheckedTheme = true;
            ModifyTheme(isDarkTheme);
        }

        private static void ModifyTheme(bool isDarkTheme)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(isDarkTheme ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);
        }
    }
}

