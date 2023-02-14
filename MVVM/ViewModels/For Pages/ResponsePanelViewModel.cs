using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages
{
    public class ResponsePanelViewModel : ViewModelBase
    {
        private bool _isAcceptButtonEnabled = true;
        private bool _isDeclineButtonEnabled = true;
        private ICommand _acceptCommand;
        private ICommand _declineCommand;

        public ResponsePanelViewModel()
        {
            AcceptText = "Accept";
            DeclineText = "Decline";
            AcceptRow = 0;
            DeclineRow = 1;
            _acceptCommand = new RelayCommand(async () => await SetAcceptedStateAsync(), () => _isAcceptButtonEnabled);
            _declineCommand = new RelayCommand(async () => await SetDeclinedStateAsync(), () => _isDeclineButtonEnabled);
        }

        public ICommand AcceptCommand => _acceptCommand;
        public ICommand DeclineCommand => _declineCommand;

        private double _declineBorderOpacity = 1.0;
        public double DeclineBorderOpacity
        {
            get { return _declineBorderOpacity; }
            set { _declineBorderOpacity = value; OnPropertyChanged(nameof(DeclineBorderOpacity)); }
        }

        private double _declineRootOpacity = 1.0;
        public double DeclineRootOpacity
        {
            get { return _declineRootOpacity; }
            set { _declineRootOpacity = value; OnPropertyChanged(nameof(DeclineRootOpacity)); }
        }

        private double _acceptBorderOpacity = 1.0;
        public double AcceptBorderOpacity
        {
            get { return _acceptBorderOpacity; }
            set { _acceptBorderOpacity = value; OnPropertyChanged(nameof(AcceptBorderOpacity)); }
        }

        private double _accepRootOpacity = 1.0;
        public double AcceptRootOpacity
        {
            get { return _accepRootOpacity; }
            set { _accepRootOpacity = value; OnPropertyChanged(nameof(AcceptRootOpacity)); }
        }

        public bool IsAcceptedButtonEnabled
        {
            get { return _isAcceptButtonEnabled; }
            set
            {
                if(_isAcceptButtonEnabled != value)
                {
                    _isAcceptButtonEnabled = value;
                    OnPropertyChanged(nameof(IsAcceptedButtonEnabled));
                }
            }
        }

        private Brush _acceptBorderColor;
        public Brush AcceptBorderColor
        {
            get
            {
                return _acceptBorderColor;
            }
            set
            {
                _acceptBorderColor = value;
                OnPropertyChanged(nameof(AcceptBorderColor));
            }
        }

        private Brush _declineBorderColor;
        public Brush DeclineBorderColor
        {
            get
            {
                return _declineBorderColor;
            }
            set
            {
                _declineBorderColor = value;
                OnPropertyChanged(nameof(DeclineBorderColor));
            }
        }

        private ScaleTransform _acceptRenderTransform;
        public ScaleTransform AcceptRenderTransform
        {
            get
            {
                return _acceptRenderTransform;
            }
            set
            {
                _acceptRenderTransform = value;
                OnPropertyChanged(nameof(AcceptRenderTransform));
            }
        }
        public bool IsDeclinedButtonEnabled
        {
            get { return _isDeclineButtonEnabled; }
            set
            {
                if(_isDeclineButtonEnabled != value)
                {
                    _isDeclineButtonEnabled = value;
                    OnPropertyChanged(nameof(IsDeclinedButtonEnabled));
                }
            }
        }

        private string _acceptText;
        public string AcceptText
        {
            get
            {
                return _acceptText;
            }
            set
            {
                _acceptText = value;
                OnPropertyChanged(nameof(AcceptText));
            }
        }

        private string _declineText;
        public string DeclineText
        {
            get
            {
                return _declineText;
            }
            set
            {
                _declineText = value;
                OnPropertyChanged(nameof(DeclineText));
            }
        }

        private int _acceptRow;
        public int AcceptRow
        {
            get
            {
                return _acceptRow;
            }
            set
            {
                _acceptRow = value;
                OnPropertyChanged(nameof(AcceptRow));
            }
        }

        private int _declineRow;
        public int DeclineRow
        {
            get
            {
                return _declineRow;
            }
            set
            {
                _declineRow = value;
                OnPropertyChanged(nameof(DeclineRow));
            }
        }

        private async Task SetAcceptedStateAsync()
        {
            IsAcceptedButtonEnabled = false;
            VisualStateManager.GoToState(new ResponsePanel(), "Accepted", true);
            for(int i = 0; i < 10; i++)
            {
                await Task.Delay(50);
                DeclineBorderOpacity -= 0.1;
                DeclineRootOpacity -= 0.1;
            }
            await SubmitResponseAsync(InviteResponseKind.Accepted);
            IsAcceptedButtonEnabled = true;
        }

        private async Task SetDeclinedStateAsync()
        {
            IsDeclinedButtonEnabled = false;
            for(int i = 0; i < 10; i++)
            {
                await Task.Delay(40);
                AcceptBorderOpacity -= 0.1;
                AcceptRootOpacity -= 0.1;
            }
            await SubmitResponseAsync(InviteResponseKind.Declined);
            IsDeclinedButtonEnabled = true;
        }

        private async Task ReturnToDefaultAcceptButton()
        {
            for(int i = 0; i < 10; i++)
            {
                await Task.Delay(50);
                DeclineBorderOpacity += 0.1;
                DeclineRootOpacity += 0.1;
            }
            AcceptText = "Accept";
            AcceptBorderColor = Brushes.Transparent;
        }
        private async Task ReturnToDefaultDeclineButton()
        {
            for(int i = 0; i < 10; i++)
            {
                await Task.Delay(40);
                AcceptBorderOpacity += 0.1;
                AcceptRootOpacity += 0.1;
            }
            DeclineText = "Decline";
            DeclineBorderColor = Brushes.Transparent;
            DeclineRow = 1;
        }
        private async Task SubmitResponseAsync(InviteResponseKind response)
        {
            Brush brushAcceptedColor = Brushes.DarkSeaGreen;
            Brush brushDeclinedColor = Brushes.IndianRed;
            if(response == InviteResponseKind.Accepted)
            {
                AcceptText = "Accepted";
                AcceptBorderColor = brushAcceptedColor;

                await Task.Delay(3000);
                await ReturnToDefaultAcceptButton();

            } else if(response == InviteResponseKind.Declined)
            {
                DeclineText = "Declined";
                DeclineBorderColor = brushDeclinedColor;
                DeclineRow = 0;
                await Task.Delay(3000);
                await ReturnToDefaultDeclineButton();
            }
        }
    }
}
