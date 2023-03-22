using System;
using System.Collections.Generic;
using System.Text;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.DTOs
{
    public class SvApprovalStateDTO
    {
        public int Id { get; set; }
        public int Vacation_Record_Id { get; set; }
        public int Supervisor_Id { get; set; }
        public int Status_Id { get; set; }
    }
}
