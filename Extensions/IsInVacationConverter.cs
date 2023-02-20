using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Extensions
{
    public class IsInVacationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var vacation = values[3] as Vacation;
            var dayModel = values[1] as Day;

            var brush = dayModel.IsAlreadyScheduledVacation ? Brushes.LightSlateGray : Brushes.Transparent;

            if(vacation != null)
            {
                foreach(var date in vacation.DateRange.Step(x => x.AddDays(1)))
                {
                    if(date.Date == dayModel.Date && dayModel.IsAlreadyScheduledVacation)
                    {
                        brush = dayModel.Date.DayOfWeek == DayOfWeek.Saturday || dayModel.Date.DayOfWeek == DayOfWeek.Sunday
                                    ? Brushes.DarkSeaGreen
                                    : Brushes.IndianRed;
                    } else if(date.Date == dayModel.Date && !dayModel.IsAlreadyScheduledVacation)
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
