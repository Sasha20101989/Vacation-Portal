using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Vacation_Portal.Extensions
{
    public class MyCalendarConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)values[0];
            var d = values[1].GetType();
            var dates = values[1] as BindingList<DateTime>;
            return dates.Contains(date);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
