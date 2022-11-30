using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class Holiday : ViewModelBase
    {
        public string NameOfHoliday { get; set; }

        public Holiday(string name)
        {
            NameOfHoliday = name;
        }

    }
}
