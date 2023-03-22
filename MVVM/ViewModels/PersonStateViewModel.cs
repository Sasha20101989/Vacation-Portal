using System;
using System.Collections.Generic;
using System.Text;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.ViewModels
{
   public class PersonStateViewModel: ViewModelBase
    {
        public Subordinate Subordinate { get; set; }
        public SvApprovalStateViewModel SvApprovalStateViewModel { get; set; }
        public PersonStateViewModel(Subordinate subordinate, SvApprovalStateViewModel svApprovalStateViewModel)
        {
            Subordinate = subordinate;
            SvApprovalStateViewModel = svApprovalStateViewModel;
        }

    }
}
