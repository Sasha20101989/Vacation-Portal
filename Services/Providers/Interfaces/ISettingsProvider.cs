using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Services.Providers.Interfaces
{
    public interface ISettingsProvider
    {
        Task<IEnumerable<Settings>> GetSettingsUI(string account);
    }
}
