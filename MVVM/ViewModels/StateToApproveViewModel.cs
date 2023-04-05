using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Vacation_Portal.Commands.HorizontalCalendarCommands;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels
{
    public class StateToApproveViewModel: ViewModelBase
    {
        private bool _isSave;
        public bool IsSave
        {
            get => _isSave;
            set
            {
                _isSave = value;
                OnPropertyChanged(nameof(IsSave));

            }
        }
        public ObservableCollection<SvApprovalStateViewModel> StatesWithoutOnApproval { get; set; }
        public ICommand ApproveStateCommand { get; }
        public StateToApproveViewModel()
        {
            StatesWithoutOnApproval = new ObservableCollection<SvApprovalStateViewModel>();
            ApproveStateCommand = new ApproveStateCommand(this);
        }
        public void GetStatesWithoutOnApproval(ObservableCollection<SvApprovalStateViewModel> personStates)
        {
            StatesWithoutOnApproval.Clear();
            IEnumerable<IGrouping<int, SvApprovalStateViewModel>> groupedStates = personStates.OrderBy(s => s.Vacation.UserId).GroupBy(s => s.Vacation.UserId);
            foreach(var group in groupedStates)
            {
                bool hasOnApprovalStatus = false;
                foreach(SvApprovalStateViewModel state in group)
                {
                    if(state.StatusId == (int) Statuses.OnApproval)
                    {
                        hasOnApprovalStatus = true;
                        break;
                    }
                }
                if(!hasOnApprovalStatus)
                {
                    foreach(SvApprovalStateViewModel state in group)
                    {

                        StatesWithoutOnApproval.Add(state);
                    }
                } else
                {
                    // Не сохраняем ни один статус текущей группы, так как у сотрудника есть статус "onApproval"
                }
            }
        }
    }
}
