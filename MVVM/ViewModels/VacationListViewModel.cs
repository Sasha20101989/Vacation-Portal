using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Commands.HorizontalCalendarCommands
{
    public class VacationListViewModel
    {
        public Subordinate Subordinate { get; internal set; }
        public ObservableCollection<Vacation> Vacations { get; set; }

        internal async Task<ObservableCollection<Vacation>> LoadVacationsAsync(Subordinate selectedSubordinate)
        {
            Subordinate = selectedSubordinate;
            ObservableCollection<Vacation> subordinateVacations = new ObservableCollection<Vacation>();
            IEnumerable<Vacation> vacations = await App.API.LoadVacationsAsync(Subordinate.Id_SAP);
            foreach(Vacation vacation in vacations)
            {
                subordinateVacations.Add(vacation);
            }
            Vacations = subordinateVacations;
            return subordinateVacations;
        }
    }
}