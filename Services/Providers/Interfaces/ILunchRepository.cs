using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Services.Providers.Interfaces
{
   public interface ILunchRepository
    {
        Person Person { get; set; }
        Task<Person> LoginAsync(string account);
        Task LogoutAsync();
        Task<IEnumerable<Department>> GetDepartmentsAsync();
        Task<IEnumerable<Settings>> GetSettingsAsync(string account);
        Task<IEnumerable<Person>> GetUser(string account);
    }
}
