using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace Vacation_Portal.Extensions
{
    class IntersectionsCountToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int count = (int) value;
            if(count <= 1)
            {
                return Brushes.Transparent;
            } else
            {
               return (Brushes) System.Windows.Application.Current.FindResource("MaterialDesignBody");
            }
        }



        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}