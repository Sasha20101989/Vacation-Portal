using CommunityToolkit.Mvvm.Input;
using MaterialDesignThemes.Wpf;
using MiscUtil.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.Commands.ResponsePanelCommands;
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

       public ICommand ReturnCommand { get; set; }
        public СustomizedCalendar Calendar { get; set; }
        public ResponsePanelViewModel()
        {
            VisibilityInfoBar = false;
            AcceptText = "Подтвердить";
            DeclineText = "Отклонить";
            AcceptRow = 0;
            DeclineRow = 1;
            _acceptCommand = new RelayCommand(async () => await SetAcceptedStateAsync(), () => _isAcceptButtonEnabled);
            _declineCommand = new RelayCommand(async () => await SetDeclinedStateAsync(), () => _isDeclineButtonEnabled);

            ReturnCommand = new ReturnToApprovalListCommand(this);
            ProcessedVacations = App.API.ProcessedVacations;
            Calendar = new СustomizedCalendar();
            PersonsWithVacationsOnApproval = App.API.PersonsWithVacationsOnApproval;
        }
        private void updateMonths()
        {
            List<Month> months = new List<Month>();
            for(int i = 1; i <= 12; i++)
            {
                months.Add(new Month(i, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i), DateTime.DaysInMonth(2023, i), IntersectingVacations, VacationsOnApproval));
            }
            months.RemoveAll(month => month.Days.Count == 0);
            Months = months;
        }
        private void GetIntersectingVacations()
        {
            ObservableCollection<Vacation> intersectingVacations = new ObservableCollection<Vacation>();
            foreach(DateTime VacationItemDate in VacationItem.DateRange.Step(x => x.AddDays(1)))
            {
                foreach(Subordinate subordinate in App.API.Person.Subordinates)
                {
                    foreach(Vacation vacation in subordinate.Subordinate_Vacations)
                    {
                        foreach(DateTime dateVacation in vacation.DateRange.Step(x => x.AddDays(1)))
                        {
                            if(VacationItemDate == dateVacation && vacation.User_Name != VacationItem.User_Name)
                            {
                                if(!intersectingVacations.Contains(vacation))
                                {
                                    intersectingVacations.Add(vacation);
                                }
                            }
                        }
                    }
                }
            }
            IntersectingVacations.Clear();
            IntersectingVacations = intersectingVacations;
        }

        private void GetVacationsOnApproval()
        {
            VacationsOnApproval.Clear();
                foreach(Vacation vacation in SelectedPersonWithVacationsOnApproval.Subordinate_Vacations)
                {
                    if(vacation.Vacation_Status_Name == "On Approval")
                    {
                        if(!VacationsOnApproval.Contains(vacation))
                        {
                            VacationsOnApproval.Add(vacation);
                        }
                    }
                }
        }
        private List<Month> _months = new List<Month>();
        public List<Month> Months
        {
            get
            {
                return _months;
            }
            set
            {
                _months = value;
                OnPropertyChanged(nameof(Months));
            }
        }

        private ObservableCollection<Vacation> _processedVacations = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> ProcessedVacations
        {
            get
            {
                return _processedVacations;
            }
            set
            {
                _processedVacations = value;
                OnPropertyChanged(nameof(ProcessedVacations));
            }
        }

        private ObservableCollection<Vacation> _vacationsOnApproval = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsOnApproval
        {
            get
            {
                return _vacationsOnApproval;
            }
            set
            {
                _vacationsOnApproval = value;
                OnPropertyChanged(nameof(VacationsOnApproval));
            }
        }

        private ObservableCollection<Vacation> _intersectingVacations = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> IntersectingVacations
        {
            get
            {
                return _intersectingVacations;
            }
            set
            {
                _intersectingVacations = value;
                OnPropertyChanged(nameof(IntersectingVacations));
            }
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

        private bool _visibilityInfoBar;
        public bool VisibilityInfoBar
        {
            get
            {
                return _visibilityInfoBar;
            }
            set
            {
                _visibilityInfoBar = value;
                OnPropertyChanged(nameof(VisibilityInfoBar));
            }
        }

        private bool _visibilityListVacations;
        public bool VisibilityListVacations
        {
            get
            {
                return _visibilityListVacations;
            }
            set
            {
                _visibilityListVacations = value;
                OnPropertyChanged(nameof(VisibilityListVacations));
            }
        }

        private Vacation _vacationItem;
        public Vacation VacationItem
        {
            get => _vacationItem;
            set
            {
                SetProperty(ref _vacationItem, value);
                OnPropertyChanged(nameof(VacationItem));
                
                if(VacationItem == null)
                {
                    VisibilityInfoBar = false;
                } else
                {
                    GetIntersectingVacations();
                    updateMonths();
                    VisibilityInfoBar = true;
                }
            }
        }

        private Brush _badgeBackground;
        public Brush BadgeBackground
        {
            get => _badgeBackground;
            set
            {
                SetProperty(ref _badgeBackground, value);
                OnPropertyChanged(nameof(BadgeBackground));
            }
        }

        private int _vacationIndex;
        public int VacationIndex
        {
            get => _vacationIndex;
            set
            {
                SetProperty(ref _vacationIndex, value);
                OnPropertyChanged(nameof(VacationIndex));
            }
        }

        private Vacation _processedVacationItem;
        public Vacation ProcessedVacationItem
        {
            get => _processedVacationItem;
            set
            {
                SetProperty(ref _processedVacationItem, value);
                OnPropertyChanged(nameof(ProcessedVacationItem));
            }
        }

        private int _processedVacationIndex;
        public int ProcessedVacationIndex
        {
            get => _processedVacationIndex;
            set
            {
                SetProperty(ref _processedVacationIndex, value);
                OnPropertyChanged(nameof(ProcessedVacationIndex));
            }
        }

        private ObservableCollection<Subordinate> _personsWithVacationsOnApproval = new ObservableCollection<Subordinate>();
        public ObservableCollection<Subordinate> PersonsWithVacationsOnApproval
        {
            get
            {
                return _personsWithVacationsOnApproval;
            }
            set
            {
                _personsWithVacationsOnApproval = value;
                OnPropertyChanged(nameof(PersonsWithVacationsOnApproval));
            }
        }

        private Subordinate _selectedPersonWithVacationsOnApproval;
        public Subordinate SelectedPersonWithVacationsOnApproval
        {
            get
            {
                return _selectedPersonWithVacationsOnApproval;
            }
            set
            {
                _selectedPersonWithVacationsOnApproval = value;
                OnPropertyChanged(nameof(SelectedPersonWithVacationsOnApproval));
                if(SelectedPersonWithVacationsOnApproval == null)
                {
                    VisibilityListVacations = false;
                } else
                {
                    GetVacationsOnApproval();
                    VisibilityListVacations = true;
                }
                
            }
        }

        private int _selectedIndexPersonWithVacationsOnApproval;
        public int SelectedIndexPersonWithVacationsOnApproval
        {
            get
            {
                return _selectedIndexPersonWithVacationsOnApproval;
            }
            set
            {
                _selectedIndexPersonWithVacationsOnApproval = value;
                OnPropertyChanged(nameof(SelectedIndexPersonWithVacationsOnApproval));
            }
        }

        private async Task SetAcceptedStateAsync()
        {
            IsAcceptedButtonEnabled = false;
            VisualStateManager.GoToState(new ResponsePanel(), "Подтверждено", true);
            for(int i = 0; i < 10; i++)
            {
                await Task.Delay(50);
                DeclineBorderOpacity -= 0.1;
                DeclineRootOpacity -= 0.1;
            }
            await SubmitResponseAsync(InviteResponseKind.Accepted);
            IsAcceptedButtonEnabled = true;
            VacationItem.VacationStatusKind = PackIconKind.CheckBold;
            VacationItem.BadgeBackground = Brushes.DarkSeaGreen;
            ProcessedVacations.Add(VacationItem);
            VacationsOnApproval.Remove(VacationItem);
            IntersectingVacations.Clear();
            if(VacationsOnApproval.Count > 0)
            {
                VacationIndex = 0;
            }
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
            VacationItem.VacationStatusKind = PackIconKind.Close;
            VacationItem.BadgeBackground = Brushes.IndianRed;
            ProcessedVacations.Add(VacationItem);
            VacationsOnApproval.Remove(VacationItem);
            IntersectingVacations.Clear();
            if(VacationsOnApproval.Count > 0)
            {
                VacationIndex = 0;
            }
        }

        private async Task ReturnToDefaultAcceptButton()
        {
            for(int i = 0; i < 10; i++)
            {
                await Task.Delay(50);
                DeclineBorderOpacity += 0.1;
                DeclineRootOpacity += 0.1;
            }
            AcceptText = "Подтвердить";
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
            DeclineText = "Отклонить";
            DeclineBorderColor = Brushes.Transparent;
            DeclineRow = 1;
        }
        private async Task SubmitResponseAsync(InviteResponseKind response)
        {
            Brush brushAcceptedColor = Brushes.DarkSeaGreen;
            Brush brushDeclinedColor = Brushes.IndianRed;
            if(response == InviteResponseKind.Accepted)
            {
                AcceptText = "Подтверждено";
                AcceptBorderColor = brushAcceptedColor;

                await Task.Delay(1000);
                await ReturnToDefaultAcceptButton();

            } else if(response == InviteResponseKind.Declined)
            {
                DeclineText = "Отклонено";
                DeclineBorderColor = brushDeclinedColor;
                DeclineRow = 0;
                await Task.Delay(1000);
                await ReturnToDefaultDeclineButton();
            }
        }
    }
}
