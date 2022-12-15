namespace Vacation_Portal.Services.Providers.Interfaces
{
    public interface IDependencyDetector
    {
        bool StartDependencyPlannedHoliday();
        bool StopDependencyPlannedHoliday();
        bool StartDependencyPlannedVacation();
        bool StopDependencyPlannedVacation();
        bool StartDependencyPerson();
        bool StopDependencyPerson();
    }
}
