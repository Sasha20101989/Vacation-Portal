using System;
using System.Globalization;
using System.Windows.Data;

namespace Vacation_Portal.Extensions
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && value is DateTime date)
            {
                return date.ToString("d MMMM yyyy", new CultureInfo("ru-RU"));
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && value is string date)
            {
                if(DateTime.TryParseExact(date, "dd MMMM yyyy", new CultureInfo("ru-RU"), DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }

            return null;
        }
    }
}
