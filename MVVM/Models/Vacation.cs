using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Vacation_Portal.MVVM.Models
{
    public class Vacation
    {
        public string Name { get; set; }
        public string Count { get; set; }
        public Brush Color { get; set; }      
        public Vacation(string name, string count, Brush color)
        {
            Name = name;
            Count = count;
            Color = color;
        }
    }
}
