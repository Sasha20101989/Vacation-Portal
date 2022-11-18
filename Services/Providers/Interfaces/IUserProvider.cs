using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.DbContext;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Services.Providers.Interfaces
{
    public interface IUserProvider
    {
        Task<IEnumerable<Person>> GetUser(string account);
    }
}
