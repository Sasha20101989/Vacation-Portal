using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Vacation_Portal {
    public enum Statuses {
        [Description("Being Planned")]
        BeingPlanned = 1,
        [Description("On Approval")]
        OnApproval = 2,
        [Description("Approved")]
        Approved = 3,
        [Description("Passed to HR")]
        PassedToHR = 4,
        [Description("Committed")]
        Committed = 5,
        [Description("Deleted")]
        Deleted = 6,
        [Description("Planned")]
        Planned = 7
    }
}
