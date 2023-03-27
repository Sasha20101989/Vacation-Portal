using System;
using System.Collections.Generic;
using System.Text;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Services.Providers.Interfaces
{
    public interface IAssetsRepository
    {
        List<Status> AllStatuses { get; set; }
        List<Status> GetStatuses();
    }
}
