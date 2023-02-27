using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages {
    public class HomeViewModel : ViewModelBase {
        private bool _isLogginIn;
        public bool IsLogginIn {
            get => _isLogginIn;
            set {
                _isLogginIn = value;
                OnPropertyChanged(nameof(IsLogginIn));
            }
        }

        private bool _isLoginSuccesed;
        public bool IsLoginSuccesed {
            get => _isLoginSuccesed;
            set {
                _isLoginSuccesed = value;
                OnPropertyChanged(nameof(IsLoginSuccesed));
            }
        }

        public HomeViewModel() {

        }
    }
}
