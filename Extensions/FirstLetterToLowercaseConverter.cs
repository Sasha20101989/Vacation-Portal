using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Vacation_Portal.Extensions
{
    public class FirstLetterToLowercaseConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value is string stringValue && !string.IsNullOrEmpty(stringValue)) {
                return stringValue.First().ToString().ToLower() + stringValue.Substring(1);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
