using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace Vacation_Portal.Extensions {
    public class MyCalendarConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            DateTime date = (DateTime) values[0];
            _ = values[1].GetType();
            BindingList<DateTime> dates = values[1] as BindingList<DateTime>;
            return dates.Contains(date);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
