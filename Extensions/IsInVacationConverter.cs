using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Extensions
{
    public class IsInVacationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Day dayModel = values[1] as Day;

            SolidColorBrush brush = dayModel.IsAlreadyScheduledVacation ? Brushes.Transparent : Brushes.Transparent;

            if(values[3] is Vacation vacation)
            {
                foreach(DateTime date in vacation.DateRange)
                {
                    if(date.Date == dayModel.Date && dayModel.IsAlreadyScheduledVacation)
                    {
                        brush = dayModel.Date.DayOfWeek == DayOfWeek.Saturday || dayModel.Date.DayOfWeek == DayOfWeek.Sunday
                                    ? Brushes.DarkSeaGreen
                                    : Brushes.IndianRed;
                    } else if(date.Date == dayModel.Date && !dayModel.IsAlreadyScheduledVacation)
                    {
                        brush = Brushes.DarkSeaGreen;
                    } else if(dayModel.IsNotConflict)
                    {
                        brush = Brushes.DarkSeaGreen;
                    }
                }
            }

            return brush;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
