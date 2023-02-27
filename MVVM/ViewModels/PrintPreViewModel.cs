using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels {
    public class PrintPreViewModel : ViewModelBase {
        public PrintPreViewModel(string fullName, string position, string virtualDepartment, int sapId, Vacation vacationToCompensate) {
            FullName = fullName;
            Position = position;
            VirtualDepartment = virtualDepartment;
            SapId = sapId;
            VacationToCompensate = vacationToCompensate;
        }

        public PrintPreViewModel(string fullName, string position, string virtualDepartment, int sapId, IEnumerable<Vacation> allAvailableToShiftVacations) {
            FullName = fullName;
            Position = position;
            VirtualDepartment = virtualDepartment;
            SapId = sapId;
            AllAvailableToShiftVacations = new ObservableCollection<Vacation>(allAvailableToShiftVacations);
            SelectedToShiftVacations = new ObservableCollection<Vacation>(allAvailableToShiftVacations.Take(2));
            AvailableToSelectVacations = new ObservableCollection<Vacation>(allAvailableToShiftVacations.TakeLast(2));

        }

        public string FullName { get; private set; }
        public string Position { get; private set; }
        public string VirtualDepartment { get; private set; }
        public int SapId { get; private set; }
        public Vacation VacationToCompensate { get; private set; }
        private ObservableCollection<Vacation> AllAvailableToShiftVacations { get; }
        public ObservableCollection<Vacation> AvailableToSelectVacations { get; set; }
        public ObservableCollection<Vacation> SelectedToShiftVacations { get; set; }
        public ObservableCollection<Vacation> NewVacations { get; set; }

        public int FreeDaysToSpend => SelectedToShiftVacations.Sum(v => v.Count);

    }
}