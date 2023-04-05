using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Vacation_Portal.Extensions
{
    public class StringEqualityToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value as string;
            string[] targetValues = parameter?.ToString().Split(',');

            if(strValue == null || targetValues == null)
            {
                return Visibility.Collapsed;
            }

            
            return targetValues.Any(value =>
                   strValue.Equals(value.Trim(), StringComparison.InvariantCultureIgnoreCase))
                   ? Visibility.Visible
                   : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
