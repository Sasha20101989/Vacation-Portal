using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.Models.HorizontalCalendar;

namespace Vacation_Portal.Extensions
{
    public class FirstPersonIntersectionCountConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length < 2 ||
                !(values[0] is HorizontalDay date) ||
                !(values[1] is ObservableCollection<Vacation> currentPersonVacations) ||
                !(values[2] is ObservableCollection<Subordinate> subordinates))
            {
                return null;
            }
            if(currentPersonVacations.Count == 0)
            {
                return null;
            }
            int currentPersonid = 0;

            List<DateTime> grayDays = new List<DateTime>();

            foreach(var vacation in currentPersonVacations)
            {
                currentPersonid = vacation.UserId;
            }

            //if(subordinates.Count !=1)
            //{
            //    return null;
            //}

            //if(subordinates[0] != subordinates.First())
            //{
            //    return null;
            //}
            int count = date.IntersectionsCount;
            if(count == 0)
            {
                return null;
            }
            return date.IntersectionsCount.ToString();
        }



        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
