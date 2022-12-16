using System.ComponentModel;

namespace Vacation_Portal
{
    public enum Roles
    {
        [Description("Worker")]
        Worker = 1,
        [Description("Supervisor")]
        Supervisor = 2,
        [Description("HR")]
        HR = 3,
        [Description("Accounting")]
        Accounting = 4,
    }
}
