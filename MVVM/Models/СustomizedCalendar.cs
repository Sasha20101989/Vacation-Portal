using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class СustomizedCalendar : ViewModelBase
    {
        private ObservableCollection<CalendarDay> _days;
        public ObservableCollection<CalendarDay> Days
        {
            get { return _days; }
            set
            {
                _days = value;
                OnPropertyChanged(nameof(Days));
            }
        }

        private Vacation _selectedVacation;
        public Vacation SelectedVacation
        {
            get { return _selectedVacation; }
            set
            {
                _selectedVacation = value;
                UpdateCalendarDays();
                OnPropertyChanged(nameof(SelectedVacation));
            }
        }

        public СustomizedCalendar()
        {
            Days = new ObservableCollection<CalendarDay>();

            // Заполнение коллекции дней
            // ...

            // Инициализация выбранного отпуска
            SelectedVacation = null;
        }

        // Обновление свойства IsSelected для каждого дня в календаре
        private void UpdateCalendarDays()
        {
            //foreach(var day in Days)
            //{
            //    day.IsSelected = SelectedVacation != null && day.Date >= SelectedVacation.StartDate && day.Date <= SelectedVacation.EndDate;
            //}
        }
    }
}
