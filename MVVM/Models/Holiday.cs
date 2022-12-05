using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class Holiday : ViewModelBase
    {
        public int Id { get; set; }
        public string NameOfHoliday { get; set; }

        public Holiday(int id, string name)
        {
            Id = id;
            NameOfHoliday = name;
        }
    }
}
