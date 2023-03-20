using System;
using System.Collections.Generic;
using System.Text;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class SvApprovalState : ViewModelBase
    {
        public int Id { get; set; }
        public int VacationRecordId { get; set; }
        public int SupervisorId { get; set; }
        public int StatusId { get; set; }
        private Vacation _vacation;
        public Vacation Vacation
        {
            get => _vacation;
            set
            {
                _vacation = value;
                OnPropertyChanged(nameof(Vacation));
            }
        }

        public SvApprovalState(int id, int vacationRecordId, int supervisorId, int statusId, Vacation vacation)
        {
            Id = id;
            VacationRecordId = vacationRecordId;
            SupervisorId = supervisorId;
            StatusId = statusId;
            Vacation = vacation;
        }
    }
}
