using MaterialDesignThemes.Wpf;
using System.Windows.Media;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.ViewModels.For_Pages;

namespace Vacation_Portal.MVVM.ViewModels.ForPages {
    public class SettingsViewModel : ViewModelBase {
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();
        private readonly MainWindowViewModel _mainWindowViewModel;
        private Color? _primaryColor;
        public SettingsViewModel(MainWindowViewModel mainWindowViewModel) {
            _mainWindowViewModel = mainWindowViewModel;
            ITheme theme = _paletteHelper.GetTheme();
            _primaryColor = theme.PrimaryMid.Color;
            SelectedColor = _primaryColor;
        }

        private ColorScheme _activeScheme;
        public ColorScheme ActiveScheme {
            get => _activeScheme;
            set {
                if(_activeScheme != value) {
                    _activeScheme = value;
                    OnPropertyChanged();
                }
            }
        }

        private Color? _selectedColor;
        public Color? SelectedColor {
            get => _selectedColor;
            set {
                if(_selectedColor != value) {
                    _selectedColor = value;
                    OnPropertyChanged();

                    if(value is Color color) {
                        ChangeCustomColor(color);
                    }
                }
            }
        }

        private void ChangeCustomColor(object obj) {
            Color color = (Color) obj;

            if(ActiveScheme == ColorScheme.Primary) {

                _paletteHelper.ChangePrimaryColor(color);
                _primaryColor = color;
            }
        }
    }
}
