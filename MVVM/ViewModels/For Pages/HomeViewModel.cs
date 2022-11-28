using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages
{
    public class HomeViewModel : ViewModelBase
    {
        private bool _isLogginIn;
        public bool IsLogginIn
        {
            get
            {
                return _isLogginIn;
            }
            set
            {
                _isLogginIn = value;
                OnPropertyChanged(nameof(IsLogginIn));
            }
        }

        private bool _isLoginSuccesed;
        public bool IsLoginSuccesed
        {
            get
            {
                return _isLoginSuccesed;
            }
            set
            {
                _isLoginSuccesed = value;
                OnPropertyChanged(nameof(IsLoginSuccesed));
            }
        }

        public HomeViewModel()
        {

        }
    }
}
