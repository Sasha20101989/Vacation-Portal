using System;
using System.Globalization;
using System.Windows.Data;

namespace Vacation_Portal.Extensions {
    public class CountToBoolConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value is int count && int.TryParse(parameter?.ToString(), out int threshold)) {
                return count >= threshold;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
