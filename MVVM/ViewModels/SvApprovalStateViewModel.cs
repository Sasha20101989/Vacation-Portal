using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels
{
    public class SvApprovalStateViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public int VacationRecordId { get; set; }
        public int SupervisorId { get; set; }

        private int _statusId;
        public int StatusId
        {
            get => _statusId;
            set
            {
                _statusId = value;
                OnPropertyChanged(nameof(StatusName));
                Status status = App.API.AllStatuses.FirstOrDefault(s => s.Id == _statusId);
                if(status != null)
                {
                    StatusName = status.Status_Name;
                }
            }
        }
        private string _statusName;
        public string StatusName
        {
            get => _statusName;
            set
            {
                _statusName = value;
                OnPropertyChanged(nameof(StatusName));
            }
        }
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

        public SvApprovalStateViewModel(int id, int vacationRecordId, int supervisorId, int statusId, Vacation vacation)
        {
            Id = id;
            VacationRecordId = vacationRecordId;
            SupervisorId = supervisorId;
            StatusId = statusId;
            Vacation = vacation;
        }
    }
}
