using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.Commands.HorizontalCalendarCommands
{
    public class VacationListViewModel : ViewModelBase
    {
        public Subordinate Subordinate { get; internal set; }

        private ObservableCollection<SvApprovalStateViewModel> _states;
        public ObservableCollection<SvApprovalStateViewModel> States
        {
            get => _states;
            set
            {
                _states = value;
                OnPropertyChanged(nameof(States));
            }
        }
        private SvApprovalStateViewModel _selectedState;
        public SvApprovalStateViewModel SelectedState
        {
            get => _selectedState;
            set
            {
                _selectedState = value;
                OnPropertyChanged(nameof(SelectedState));
            }
        }

        public ObservableCollection<SvApprovalStateViewModel> LoadStatesAsync(Subordinate selectedSubordinate)
        {
            Subordinate = selectedSubordinate;
            States = new ObservableCollection<SvApprovalStateViewModel>(App.StateAPI.PersonStates.Where(s => s.Vacation.User_Id_SAP == Subordinate.Id_SAP).OrderBy(state => state.Vacation.Date_Start));
            return States;
        }
    }
}