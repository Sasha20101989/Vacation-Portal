using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels.For_Pages
{
   public class HorizontalCalendarViewModel : ViewModelBase
    {
        private ObservableCollection<Subordinate> _persons = new ObservableCollection<Subordinate>();
        public ObservableCollection<Subordinate> Persons
        {
            get => _persons;
            set
            {
                _persons = value;
                OnPropertyChanged(nameof(Persons));
            }
        }
        public ObservableCollection<DateTime> YearDays { get; } = new ObservableCollection<DateTime>(
            Enumerable.Range(1, 365).Select(day => new DateTime(DateTime.Now.Year, 1, 1).AddDays(day - 1))
            );
        public HorizontalCalendarViewModel()
        {
            Persons = App.API.Person.Subordinates;
        }
    }
}
