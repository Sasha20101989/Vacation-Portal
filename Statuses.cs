﻿using System.ComponentModel;

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
        [Description("Completed")]
        Completed = 5,
        [Description("Transfered")]
        Transfered = 6,
        [Description("Planned")]
        Planned = 7,
        [Description("Compensated")]
        Compensated = 8,
        [Description("On Compensation")]
        OnCompensation = 9,
        [Description("For Tranfser")]
        ForTranfser = 10,
        [Description("Not Agreed")]
        NotAgreed = 11,
        [Description("Being Transfered")]
        BeingTransfered = 12
    }
}
