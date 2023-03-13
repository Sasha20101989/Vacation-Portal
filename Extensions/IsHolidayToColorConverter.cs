using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Vacation_Portal.Extensions {
    public class IsHolidayToColorConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value is bool isHoliday && isHoliday
                ? Brushes.Red
                : (object) (Brush) System.Windows.Application.Current.FindResource("MaterialDesignBody");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
