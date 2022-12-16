using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Vacation_Portal.Commands.BaseCommands;
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
        #endregion

        #region PlanedHolidays props
        private HolidayViewModel _selectedCurrentYearHoliday;
        public HolidayViewModel SelectedCurrentYearHoliday
        {
            get => _selectedCurrentYearHoliday;
            set
            {
                SetProperty(ref _selectedCurrentYearHoliday, value);
                OnPropertyChanged(nameof(SelectedCurrentYearHoliday));
            }
        }

        private int _selectedCurrentYearHolidayIndex;
        public int SelectedCurrentYearHolidayIndex
        {
            get => _selectedCurrentYearHolidayIndex;
            set
            {
                SetProperty(ref _selectedCurrentYearHolidayIndex, value);
                OnPropertyChanged(nameof(SelectedCurrentYearHolidayIndex));
            }
        }
        private HolidayViewModel _selectedNextYearHoliday;
        public HolidayViewModel SelectedNextYearHoliday
        {
            get => _selectedNextYearHoliday;
            set
            {
                SetProperty(ref _selectedNextYearHoliday, value);
                OnPropertyChanged(nameof(SelectedNextYearHoliday));
            }
        }

        private int _selectedNextYearHolidayIndex;
        public int SelectedNextYearHolidayIndex
        {
            get => _selectedNextYearHolidayIndex;
            set
            {
                SetProperty(ref _selectedNextYearHolidayIndex, value);
                OnPropertyChanged(nameof(SelectedNextYearHolidayIndex));
            }
        }
        #endregion PlanedHolidays props

        public int Id { get; set; }
        public string NameOfHoliday { get; set; }

        private DateTime _currentDate = DateTime.Now;
        public DateTime CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }

        private DateTime _nextDate = new DateTime(DateTime.Now.Year + 1, DateTime.Now.Month, DateTime.Now.Day);
        public DateTime NextDate
        {
            get => _nextDate;
            set
            {
                _nextDate = value;
                OnPropertyChanged(nameof(NextDate));
            }
        }

        private ObservableCollection<Holiday> _holidayTypes = new ObservableCollection<Holiday>();
        public ObservableCollection<Holiday> HolidayTypes
        {
            get => _holidayTypes;
            set
            {
                _holidayTypes = value;
                OnPropertyChanged(nameof(HolidayTypes));

            }
        }

        private ObservableCollection<HolidayViewModel> _holidaysCurrentYear = new ObservableCollection<HolidayViewModel>();
        public ObservableCollection<HolidayViewModel> HolidaysCurrentYear
        {
            get => _holidaysCurrentYear;
            set
            {
                _holidaysCurrentYear = value;
                OnPropertyChanged(nameof(HolidaysCurrentYear));

            }
        }

        private ObservableCollection<HolidayViewModel> _holidaysNextYear = new ObservableCollection<HolidayViewModel>();
        public ObservableCollection<HolidayViewModel> HolidaysNextYear
        {
            get => _holidaysNextYear;
            set
            {
                _holidaysNextYear = value;
                OnPropertyChanged(nameof(HolidaysNextYear));
            }
        }

        public SnackbarMessageQueue MessageQueue { get; set; }
        public ICommand Submit { get; }
        public ICommand Load { get; }
        public ICommand CancelHoliday { get; }
        public AnotherCommandImplementation LoadHolidayTypes { get; }
        public AnotherCommandImplementation LoadHolidays { get; }

        public HolidaysViewModel()
        {
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(2000));

            Submit = new AddHolidayCommand(this);
            CancelHoliday = new CancelHolidayCommand(this);

            LoadHolidayTypes = new AnotherCommandImplementation(
                async _ =>
                {
                    IEnumerable<Holiday> holidayTypes =  await App.API.GetHolidayTypesAsync();
                    OnHolidayTypesLoad(holidayTypes);
                });
            LoadHolidays = new AnotherCommandImplementation(
                async _ =>
                {
                    IsLoading = true;
                    IEnumerable<HolidayViewModel> holidays =  await App.API.GetHolidaysAsync(CurrentDate.Year, CurrentDate.Year + 1);
                    IsLoading = false;
                    OnHolidaysLoad(holidays);
                });
            LoadHolidays.Execute(new object());
            LoadHolidayTypes.Execute(new object());
        }

        private void OnHolidaysLoad(IEnumerable<HolidayViewModel> holidays)
        {

            foreach(HolidayViewModel holiday in holidays)
            {
                if(holiday.Date.Year == CurrentDate.Year)
                {
                    if(!HolidaysCurrentYear.Contains(new HolidayViewModel(holiday.Id, holiday.TypeOfHoliday, holiday.Date, Convert.ToInt32(holiday.Date.Year))))
                    {
                        App.API.Holidays.Add(new HolidayViewModel(holiday.Id, holiday.TypeOfHoliday, holiday.Date, Convert.ToInt32(holiday.Date.Year)));
                        HolidaysCurrentYear.Add(new HolidayViewModel(holiday.Id, holiday.TypeOfHoliday, holiday.Date, Convert.ToInt32(holiday.Date.Year)));
                        HolidaysCurrentYear = new ObservableCollection<HolidayViewModel>(HolidaysCurrentYear.OrderBy(i => i.Date));

                        App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
                    }
                } else
                {
                    if(!HolidaysNextYear.Contains(new HolidayViewModel(holiday.Id, holiday.TypeOfHoliday, holiday.Date, Convert.ToInt32(holiday.Date.Year))))
                    {
                        App.API.Holidays.Add(new HolidayViewModel(holiday.Id, holiday.TypeOfHoliday, holiday.Date, Convert.ToInt32(holiday.Date.Year)));
                        HolidaysNextYear.Add(new HolidayViewModel(holiday.Id, holiday.TypeOfHoliday, holiday.Date, Convert.ToInt32(holiday.Date.Year)));
                        HolidaysNextYear = new ObservableCollection<HolidayViewModel>(HolidaysNextYear.OrderBy(i => i.Date));
                        App.API.OnHolidaysChanged?.Invoke(App.API.Holidays);
                    }
                }
            }
        }

        private void OnHolidayTypesLoad(IEnumerable<Holiday> holidayTypes)
        {
            foreach(Holiday holidayType in holidayTypes)
            {
                HolidayTypes.Add(holidayType);
            }
        }
    }
}
