using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Vacation_Portal.Extensions
{
    public class IsHolidayToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is bool isHoliday && isHoliday)
            {
                return Brushes.Red;
            } else
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
