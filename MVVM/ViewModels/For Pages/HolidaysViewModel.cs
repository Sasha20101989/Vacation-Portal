using MaterialDesignThemes.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Vacation_Portal.Commands.HolidaysViewCommands;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages
{
    public class HolidaysViewModel : ViewModelBase
    {

        #region Holidays props
        private Holiday _selectedItem;
        public Holiday SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnPropertyChanged(nameof(SelectedItem));
                ClearErrors(nameof(CurrentDate));

                if(CurrentDate == null)
                {
                    AddErrors("Выберете дату", nameof(CurrentDate));
                    ErrorMessage = "Выберете дату";
                }
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                SetProperty(ref _selectedIndex, value);
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }
        #endregion Holidays props

        #region Interaction
        private bool _isSaving;
        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                _isSaving = value;
                OnPropertyChanged(nameof(IsSaving));
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        private string _successMessage;
        public string SuccessMessage
        {
            get => _successMessage;
            set
            {
                _successMessage = value;
                OnPropertyChanged(nameof(SuccessMessage));
                OnPropertyChanged(nameof(HasSuccessMessage));
            }
        }

        public bool HasSuccessMessage => !string.IsNullOrEmpty(SuccessMessage);

        private readonly Dictionary<string, List<string>> _propertyNameToErrorsDictionary;
        public bool HasErrors => _propertyNameToErrorsDictionary.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void AddErrors(string errorMessage, string propertyName)
        {
            if(!_propertyNameToErrorsDictionary.ContainsKey(propertyName))
            {
                _propertyNameToErrorsDictionary.Add(propertyName, new List<string>());
            }
            _propertyNameToErrorsDictionary[propertyName].Add(errorMessage);
            OnErrorsChanged(propertyName);
        }
        public IEnumerable GetErrors(string propertyName)
        {
            return _propertyNameToErrorsDictionary.GetValueOrDefault(propertyName, new List<string>());
        }
        private void ClearErrors(string propertyName)
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            _propertyNameToErrorsDictionary.Remove(propertyName);
            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(propertyName)));
        }

        #endregion

        #region PlanedHolidays props
        private HolidayViewModel _selectedHoliday;
        public HolidayViewModel SelectedHoliday
        {
            get => _selectedHoliday;
            set
            {
                SetProperty(ref _selectedHoliday, value);
                OnPropertyChanged(nameof(SelectedHoliday));
            }
        }

        private int _selectedHolidayIndex;
        public int SelectedHolidayIndex
        {
            get => _selectedHolidayIndex;
            set
            {
                SetProperty(ref _selectedHolidayIndex, value);
                OnPropertyChanged(nameof(SelectedHolidayIndex));
            }
        }
        #endregion PlanedHolidays props

        private DateTime _currentDate;
        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
                ClearErrors(nameof(CurrentDate));
            }
        }

        private ObservableCollection<Holiday> _holidayTypes;
        public ObservableCollection<Holiday> HolidayTypes
        {
            get => _holidayTypes;
            set
            {
                _holidayTypes = value;
                OnPropertyChanged(nameof(HolidayTypes));

            }
        }

        private ObservableCollection<HolidayViewModel> _holidays;
        public ObservableCollection<HolidayViewModel> Holidays
        {
            get => _holidays;
            set
            {
                _holidays = value;
                OnPropertyChanged(nameof(Holidays));

            }
        }

        public SnackbarMessageQueue MessageQueue { get; set; }
        public ICommand Submit { get; }
        public ICommand Load { get; }
        public ICommand CancelHoliday { get; }

        public HolidaysViewModel()
        {
            _holidayTypes = new ObservableCollection<Holiday>();
            _holidays = new ObservableCollection<HolidayViewModel>();
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000));
            HolidayTypes.Add(new Holiday("Внеплановый"));
            HolidayTypes.Add(new Holiday("Праздник"));
            HolidayTypes.Add(new Holiday("Рабочий в выходной"));
            HolidayTypes.Add(new Holiday("Выходной"));

            _currentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            _propertyNameToErrorsDictionary = new Dictionary<string, List<string>>();

            Submit = new AddHolidayCommand(this);
            Load = new LoadHolidaysCommand(this);
            CancelHoliday = new CancelHolidayCommand(this);
        }
    }
}
