using System.Windows.Media;
using Vacation_Portal.MVVM.ViewModels.Base;

namespace Vacation_Portal.MVVM.Models
{
    public class VacationType : ViewModelBase
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public Brush Color { get; set; }
        public VacationType(string name, int count, Brush color)
        {
            Name = name;
            Count = count;
            Color = color;
        }
    }
}
