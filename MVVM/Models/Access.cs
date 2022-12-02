using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.ViewModels.ForPages;
using Vacation_Portal.MVVM.Views;

namespace Vacation_Portal.MVVM.Models
{
    public class Access
    {
        public bool Is_Worker { get; set; }
        public bool Is_HR { get; set; }
        public bool Is_Accounting { get; set; }
        public bool Is_Supervisor { get; set; }
        public Access(bool is_Worker, bool is_HR, bool is_Accounting, bool is_Supervisor)
        {
            Is_Worker = is_Worker;
            Is_HR = is_HR;
            Is_Accounting = is_Accounting;
            Is_Supervisor = is_Supervisor;
        }
    }
}
