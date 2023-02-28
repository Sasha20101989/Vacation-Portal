﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.Commands.BaseCommands;
using Vacation_Portal.Commands.ResponsePanelCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;
using Vacation_Portal.MVVM.Views.Controls;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages {
    public class ResponsePanelViewModel : ViewModelBase {
        private bool _isAcceptButtonEnabled = true;
        private bool _isDeclineButtonEnabled = true;

        public ICommand ReturnCommand { get; set; }
        public СustomizedCalendar Calendar { get; set; }
        public ResponsePanelViewModel() {
            VisibilityInfoBar = false;
            AcceptText = "Подтвердить";
            DeclineText = "Отклонить";
            AcceptRow = 0;
            DeclineRow = 1;
            AcceptCommand = new RelayCommand(async (parameter) => await SetAcceptedStateAsync(), (parameter) => _isAcceptButtonEnabled);
            DeclineCommand = new RelayCommand(async (parameter) => await SetDeclinedStateAsync(), (parameter) => _isDeclineButtonEnabled);

            ReturnCommand = new ReturnToApprovalListCommand(this);
            ProcessedVacations = App.API.ProcessedVacations;
            Calendar = new СustomizedCalendar();
            PersonsWithVacationsOnApproval = App.API.PersonsWithVacationsOnApproval;
        }
        private void UpdateWeeksAsync() {
            List<List<Day>> allWeeks = GetWeeksInYear(2023);
            //AllStatuses = App.API.GetStatuses();
            int weeksToShow = 5;
            int vacationWeeks = ISOWeek.GetWeekOfYear(VacationItem.Date_end) - ISOWeek.GetWeekOfYear(VacationItem.Date_Start) + 1;

            if(vacationWeeks >= weeksToShow) {
                weeksToShow = Math.Min(vacationWeeks, weeksToShow);
            }

            List<List<Day>> filteredWeeks = new List<List<Day>>();
            int currentWeekIndex = 0;

            while(currentWeekIndex < allWeeks.Count && filteredWeeks.Count < weeksToShow) {
                bool containsSelectedVacation = false;

                foreach(Day day in allWeeks[currentWeekIndex]) {
                    if(day.IsInSelectedVacation || day.IsAlreadyScheduledVacation) {
                        containsSelectedVacation = true;
                        break;
                    }
                }

                if(containsSelectedVacation) {
                    if(currentWeekIndex + weeksToShow > allWeeks.Count) {
                        filteredWeeks.AddRange(allWeeks.GetRange(currentWeekIndex, allWeeks.Count - currentWeekIndex));
                        currentWeekIndex = allWeeks.Count;
                    } else {
                        int weeksToAdd = (weeksToShow - vacationWeeks) / 2;
                        int vacationStartWeek = ISOWeek.GetWeekOfYear(VacationItem.Date_Start);
                        int vacationEndWeek = ISOWeek.GetWeekOfYear(VacationItem.Date_end);
                        int startIndex = Math.Max(vacationStartWeek - weeksToAdd, 1);
                        int endIndex = Math.Min(vacationEndWeek + weeksToAdd, 53 - weeksToShow + 1);

                        filteredWeeks.AddRange(allWeeks.GetRange(startIndex - 1, weeksToShow));
                        currentWeekIndex = endIndex;
                    }
                } else {
                    currentWeekIndex++;
                }
            }

            Weeks = filteredWeeks;

        }

        public List<List<Day>> GetWeeksInYear(int year) {
            DateTime startDate = new DateTime(year, 1, 1);
            while(startDate.DayOfWeek != DayOfWeek.Monday) {
                startDate = startDate.AddDays(1);
            }

            DateTime endDate = new DateTime(year, 12, 31);

            List<List<Day>> weeks = new List<List<Day>>();
            List<Day> currentWeek = new List<Day>();

            for(DateTime date = startDate; date <= endDate; date = date.AddDays(1)) {
                Day day = new Day(date);

                foreach(Vacation vacation in VacationsOnApproval) {
                    foreach(DateTime vacationDate in vacation.DateRange) {
                        if(vacationDate == date) {
                            day.IsInSelectedVacation = true;
                            day.ToolTipText ??= vacation.User_Surname;

                            break;
                        }
                    }
                }

                foreach(Vacation vacation in IntersectingVacations) {
                    if(vacation.DateRange.Contains(date)) {
                        day.IsAlreadyScheduledVacation = true;
                        day.ToolTipText = vacation.User_Surname;
                        break;
                    }
                }

                if(date.Date.DayOfWeek == DayOfWeek.Saturday || date.Date.DayOfWeek == DayOfWeek.Sunday) {
                    day.IsHoliday = true;
                    day.ToolTipText ??= "Выходной";
                }

                currentWeek.Add(day);

                if(date.DayOfWeek == DayOfWeek.Sunday) {
                    weeks.Add(currentWeek);
                    currentWeek = new List<Day>();
                }
            }

            if(currentWeek.Count > 0) {
                weeks.Add(currentWeek);
            }

            return weeks;
        }

        private void GetIntersectingVacations() {
            ObservableCollection<Vacation> intersectingVacations = new ObservableCollection<Vacation>();
            foreach(DateTime VacationItemDate in VacationItem.DateRange) {
                foreach(Subordinate subordinate in App.API.Person.Subordinates) {
                    foreach(Vacation vacation in subordinate.Subordinate_Vacations) {
                        foreach(DateTime dateVacation in vacation.DateRange) {
                            if(VacationItemDate == dateVacation && vacation.User_Id_SAP != VacationItem.User_Id_SAP) {
                                if(!intersectingVacations.Contains(vacation)) {
                                    intersectingVacations.Add(vacation);
                                }
                            }
                        }
                    }
                }
            }
            IntersectingVacations.Clear();
            IntersectingVacations = new ObservableCollection<Vacation>(intersectingVacations.OrderBy(x => x.Date_end));

        }
        private void GetVacationsOnApproval() {
            VacationsOnApproval.Clear();
            foreach(Vacation vacation in SelectedPersonWithVacationsOnApproval.Subordinate_Vacations) {
                if(vacation.Vacation_Status_Name == "On Approval") {
                    if(!VacationsOnApproval.Contains(vacation)) {
                        VacationsOnApproval.Add(vacation);
                    }
                }
            }
        }

        #region Button settings
        public ICommand AcceptCommand { get; }
        public ICommand DeclineCommand { get; }

        private double _declineBorderOpacity = 1.0;
        public double DeclineBorderOpacity {
            get => _declineBorderOpacity;
            set { _declineBorderOpacity = value; OnPropertyChanged(nameof(DeclineBorderOpacity)); }
        }

        private double _declineRootOpacity = 1.0;
        public double DeclineRootOpacity {
            get => _declineRootOpacity;
            set { _declineRootOpacity = value; OnPropertyChanged(nameof(DeclineRootOpacity)); }
        }

        private double _acceptBorderOpacity = 1.0;
        public double AcceptBorderOpacity {
            get => _acceptBorderOpacity;
            set { _acceptBorderOpacity = value; OnPropertyChanged(nameof(AcceptBorderOpacity)); }
        }

        private double _acceptRootOpacity = 1.0;
        public double AcceptRootOpacity {
            get => _acceptRootOpacity;
            set { _acceptRootOpacity = value; OnPropertyChanged(nameof(AcceptRootOpacity)); }
        }

        public bool IsAcceptedButtonEnabled {
            get => _isAcceptButtonEnabled;
            set {
                if(_isAcceptButtonEnabled != value) {
                    _isAcceptButtonEnabled = value;
                    OnPropertyChanged(nameof(IsAcceptedButtonEnabled));
                }
            }
        }

        private Brush _acceptBorderColor;
        public Brush AcceptBorderColor {
            get => _acceptBorderColor;
            set {
                _acceptBorderColor = value;
                OnPropertyChanged(nameof(AcceptBorderColor));
            }
        }

        private Brush _declineBorderColor;
        public Brush DeclineBorderColor {
            get => _declineBorderColor;
            set {
                _declineBorderColor = value;
                OnPropertyChanged(nameof(DeclineBorderColor));
            }
        }

        private ScaleTransform _acceptRenderTransform;
        public ScaleTransform AcceptRenderTransform {
            get => _acceptRenderTransform;
            set {
                _acceptRenderTransform = value;
                OnPropertyChanged(nameof(AcceptRenderTransform));
            }
        }
        public bool IsDeclinedButtonEnabled {
            get => _isDeclineButtonEnabled;
            set {
                if(_isDeclineButtonEnabled != value) {
                    _isDeclineButtonEnabled = value;
                    OnPropertyChanged(nameof(IsDeclinedButtonEnabled));
                }
            }
        }

        private string _acceptText;
        public string AcceptText {
            get => _acceptText;
            set {
                _acceptText = value;
                OnPropertyChanged(nameof(AcceptText));
            }
        }

        private string _declineText;
        public string DeclineText {
            get => _declineText;
            set {
                _declineText = value;
                OnPropertyChanged(nameof(DeclineText));
            }
        }

        private int _acceptRow;
        public int AcceptRow {
            get => _acceptRow;
            set {
                _acceptRow = value;
                OnPropertyChanged(nameof(AcceptRow));
            }
        }

        private int _declineRow;
        public int DeclineRow {
            get => _declineRow;
            set {
                _declineRow = value;
                OnPropertyChanged(nameof(DeclineRow));
            }
        }
        private Brush _badgeBackground;
        public Brush BadgeBackground {
            get => _badgeBackground;
            set {
                SetProperty(ref _badgeBackground, value);
                OnPropertyChanged(nameof(BadgeBackground));
            }
        }
        #endregion

        #region Collections
        public List<Status> AllStatuses = new List<Status>();
        private List<Month> _months = new List<Month>();
        public List<Month> Months {
            get => _months;
            set {
                _months = value;
                OnPropertyChanged(nameof(Months));
            }
        }

        private List<List<Day>> _weeks = new List<List<Day>>();
        public List<List<Day>> Weeks {
            get => _weeks;
            set {
                _weeks = value;
                OnPropertyChanged(nameof(Weeks));
            }
        }
        private ObservableCollection<Vacation> _processedVacations = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> ProcessedVacations {
            get => _processedVacations;
            set {
                _processedVacations = value;
                OnPropertyChanged(nameof(ProcessedVacations));
            }
        }

        private ObservableCollection<Vacation> _vacationsOnApproval = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> VacationsOnApproval {
            get => _vacationsOnApproval;
            set {
                _vacationsOnApproval = value;
                OnPropertyChanged(nameof(VacationsOnApproval));
            }
        }

        private ObservableCollection<Vacation> _intersectingVacations = new ObservableCollection<Vacation>();
        public ObservableCollection<Vacation> IntersectingVacations {
            get => _intersectingVacations;
            set {
                _intersectingVacations = value;
                OnPropertyChanged(nameof(IntersectingVacations));
            }
        }
        private ObservableCollection<Subordinate> _personsWithVacationsOnApproval = new ObservableCollection<Subordinate>();
        public ObservableCollection<Subordinate> PersonsWithVacationsOnApproval {
            get => _personsWithVacationsOnApproval;
            set {
                _personsWithVacationsOnApproval = value;
                OnPropertyChanged(nameof(PersonsWithVacationsOnApproval));
            }
        }
        #endregion

        #region Visibility settings
        private bool _visibilityInfoBar;
        public bool VisibilityInfoBar {
            get => _visibilityInfoBar;
            set {
                _visibilityInfoBar = value;
                OnPropertyChanged(nameof(VisibilityInfoBar));
            }
        }

        private bool _visibilityListVacations;
        public bool VisibilityListVacations {
            get => _visibilityListVacations;
            set {
                _visibilityListVacations = value;
                OnPropertyChanged(nameof(VisibilityListVacations));
            }
        }
        #endregion

        #region Selections
        private Vacation _vacationItem;
        public Vacation VacationItem {
            get => _vacationItem;
            set {
                SetProperty(ref _vacationItem, value);
                OnPropertyChanged(nameof(VacationItem));

                if(VacationItem == null) {
                    VisibilityInfoBar = false;
                } else {
                    GetIntersectingVacations();
                    UpdateWeeksAsync();
                    VisibilityInfoBar = true;
                }
            }
        }

        private int _vacationIndex;
        public int VacationIndex {
            get => _vacationIndex;
            set {
                SetProperty(ref _vacationIndex, value);
                OnPropertyChanged(nameof(VacationIndex));
            }
        }

        private Vacation _processedVacationItem;
        public Vacation ProcessedVacationItem {
            get => _processedVacationItem;
            set {
                SetProperty(ref _processedVacationItem, value);
                OnPropertyChanged(nameof(ProcessedVacationItem));
            }
        }

        private int _processedVacationIndex;
        public int ProcessedVacationIndex {
            get => _processedVacationIndex;
            set {
                SetProperty(ref _processedVacationIndex, value);
                OnPropertyChanged(nameof(ProcessedVacationIndex));
            }
        }

        private Subordinate _selectedPersonWithVacationsOnApproval;
        public Subordinate SelectedPersonWithVacationsOnApproval {
            get => _selectedPersonWithVacationsOnApproval;
            set {
                _selectedPersonWithVacationsOnApproval = value;
                OnPropertyChanged(nameof(SelectedPersonWithVacationsOnApproval));
                if(SelectedPersonWithVacationsOnApproval == null) {
                    VisibilityListVacations = false;
                } else {
                    GetVacationsOnApproval();
                    VisibilityListVacations = true;
                }
            }
        }

        private int _selectedIndexPersonWithVacationsOnApproval;
        public int SelectedIndexPersonWithVacationsOnApproval {
            get => _selectedIndexPersonWithVacationsOnApproval;
            set {
                _selectedIndexPersonWithVacationsOnApproval = value;
                OnPropertyChanged(nameof(SelectedIndexPersonWithVacationsOnApproval));
            }
        }

        #endregion

        #region Button tasks
        private async Task SetAcceptedStateAsync() {
            IsAcceptedButtonEnabled = false;
            VisualStateManager.GoToState(new ResponsePanel(), "Подтверждено", true);
            for(int i = 0; i < 10; i++) {
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
            if(VacationsOnApproval.Count > 0) {
                VacationIndex = 0;
            }
        }

        private async Task SetDeclinedStateAsync() {
            IsDeclinedButtonEnabled = false;
            for(int i = 0; i < 10; i++) {
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
            if(VacationsOnApproval.Count > 0) {
                VacationIndex = 0;
            }
        }

        private async Task ReturnToDefaultAcceptButton() {
            for(int i = 0; i < 10; i++) {
                await Task.Delay(50);
                DeclineBorderOpacity += 0.1;
                DeclineRootOpacity += 0.1;
            }
            AcceptText = "Подтвердить";
            AcceptBorderColor = Brushes.Transparent;
        }
        private async Task ReturnToDefaultDeclineButton() {
            for(int i = 0; i < 10; i++) {
                await Task.Delay(40);
                AcceptBorderOpacity += 0.1;
                AcceptRootOpacity += 0.1;
            }
            DeclineText = "Отклонить";
            DeclineBorderColor = Brushes.Transparent;
            DeclineRow = 1;
        }
        private async Task SubmitResponseAsync(InviteResponseKind response) {
            Brush brushAcceptedColor = Brushes.DarkSeaGreen;
            Brush brushDeclinedColor = Brushes.IndianRed;
            if(response == InviteResponseKind.Accepted) {
                AcceptText = "Подтверждено";
                AcceptBorderColor = brushAcceptedColor;

                await Task.Delay(1000);
                await ReturnToDefaultAcceptButton();

            } else if(response == InviteResponseKind.Declined) {
                DeclineText = "Отклонено";
                DeclineBorderColor = brushDeclinedColor;
                DeclineRow = 0;
                await Task.Delay(1000);
                await ReturnToDefaultDeclineButton();
            }
        }
        #endregion

    }
}
