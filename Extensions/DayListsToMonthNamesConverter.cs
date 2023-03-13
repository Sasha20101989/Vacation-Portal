using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Extensions {
    public class DayListsToMonthNamesConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if(values.Length != 1 || !(values[0] is List<List<Day>> dayLists)) {
                return null;
            }

            var dateRange = dayLists.SelectMany(dayList => dayList.Select(day => day.Date));
            var monthNames = dateRange.Select(date => date.ToString("MMMM")).Distinct();

            return string.Join(" - ", monthNames);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotSupportedException();
        }
    }
}
