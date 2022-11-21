using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.MVVM.Models;

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
                RaisePropertyChanged(nameof(IsLogginIn));
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
                RaisePropertyChanged(nameof(IsLoginSuccesed));
            }
        }

        public HomeViewModel()
        {

        }
    }
}
