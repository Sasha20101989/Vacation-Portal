using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Vacation_Portal.Commands.HorizontalCalendarCommands;
using Vacation_Portal.Extensions;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.Models.HorizontalCalendar;
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

        private ObservableCollection<HorizontalDay> _yearDays = new ObservableCollection<HorizontalDay>();
        public ObservableCollection<HorizontalDay> YearDays
        {
            get => _yearDays;
            set
            {
                _yearDays = value;
                OnPropertyChanged(nameof(YearDays));
            }
        }

        private Subordinate _selectedSubordinate;
        public Subordinate SelectedSubordinate
        {
            get => _selectedSubordinate;
            set
            {
                _selectedSubordinate = value;
                OnPropertyChanged(nameof(SelectedSubordinate));
            }
        }

        private ObservableCollection<SvApprovalStateViewModel> _personStates;
        public ObservableCollection<SvApprovalStateViewModel> PersonStates
        {
            get => _personStates;
            set
            {
                _personStates = value;
                OnPropertyChanged(nameof(PersonStates));
            }
        }
        private VacationListViewModel _vacationListViewModel = new VacationListViewModel();
        public VacationListViewModel VacationListViewModel
        {
            get => _vacationListViewModel;
            set
            {
                _vacationListViewModel = value;
                OnPropertyChanged(nameof(VacationListViewModel));
    }
}

public ICommand SelectedSubordinateCommand { get; }
        public HorizontalCalendarViewModel()
        {
            SelectedSubordinateCommand = new SelectedSubordinateCommand(this);
            PersonStates = App.API.PersonStates;
            Persons = App.API.Person.Subordinates;

            YearDays = new ObservableCollection<HorizontalDay>(Enumerable.Range(1, 365).Select(day => new HorizontalDay(new DateTime(DateTime.Now.Year, 1, 1).AddDays(day - 1), false, false, 0, 0)));

            List<DateTime> vacationDays = new List<DateTime>();

            foreach(var person in Persons)
            {
                vacationDays.AddRange(person.Subordinate_Vacations.SelectMany(v => v.DateRange));
            }

            foreach(var day in YearDays)
            {
                foreach(var vacationDay in vacationDays)
                {
                    if(vacationDay.Date == day.Date)
                    {
                        day.VacationsCount++;
                    }
                }
                day.HasVacation = day.VacationsCount > 0;
            }

            foreach(var person in Persons)
            {
                foreach(var vacation in person.Subordinate_Vacations)
                {
                    List<DateTime> days = Enumerable.Range(0, (vacation.Date_end - vacation.Date_Start).Days + 1)
                                                    .Select(offset => vacation.Date_Start.AddDays(offset))
                                                    .ToList();
                    foreach(var day in YearDays)
                    {
                        if(days.Contains(day.Date))
                        {
                            day.IntersectionsCount++;
                            day.HasOnApprovalStatus = vacation.Vacation_Status_Name == MyEnumExtensions.ToDescriptionString(Statuses.OnApproval);
                        }
                        day.HasIntersection = day.IntersectionsCount > 0;
                    }
                }
            }

        }

        public void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if(eventArgs.Parameter is bool parameter &&
                parameter == false)
            {
                return;
            }

            eventArgs.Cancel();
            Task.Delay(TimeSpan.FromSeconds(0.2))
                .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                    TaskScheduler.FromCurrentSynchronizationContext());
            //Task.Delay(TimeSpan.FromSeconds(0.1))
            //    .ContinueWith((t, _) => eventArgs.Session.UpdateContent(new SampleError()), null,
            //        TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
