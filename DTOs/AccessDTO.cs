using System;
using System.Collections.Generic;
using System.Text;

namespace Vacation_Portal.DTOs
{
   public class AccessDTO
    {
        public bool Is_Worker { get; set; }
        public bool Is_HR { get; set; }
        public bool Is_Accounting { get; set; }
        public bool Is_Supervisor { get; set; }
    }
}
