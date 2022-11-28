using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using Vacation_Portal.Commands.PersonalVacationPlanningVIewModelCommands;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class Holiday: ViewModelBase
    {
        public string NameOfHoliday { get; set; }

        public Holiday(string name)
        {
            NameOfHoliday = name;
        }

    }
}
