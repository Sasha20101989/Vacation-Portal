using System.Collections.Generic;
using System.Threading.Tasks;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Services.Providers.Interfaces
{
    public interface IDepartmentProvider
    {
        IEnumerable<Department> GetDepartmentForUser(string account);
    }
}
