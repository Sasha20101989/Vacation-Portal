using System;
using System.Collections.Generic;
using System.Text;

namespace Vacation_Portal.Services.Providers.Interfaces
{
    public interface IDependencyDetector
    {
        bool startDependencyPlannedHoliday();
        bool stopDependencyPlannedHoliday();
        bool startDependencyPlannedVacation();
        bool stopDependencyPlannedVacation();
        bool startDependencyPerson();
        bool stopDependencyPerson();
    }
}
