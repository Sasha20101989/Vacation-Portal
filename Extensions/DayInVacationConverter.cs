using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Extensions
{
    public class DayInVacationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)

        {

            //if(values.Length < 2 || !(values[1] is DateTime))
            //{
            //    return false;
            //}

                

            //var date = (DateTime) values[1];

            //var vacations = (ObservableCollection<Vacation>) values[0];

            return true;

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)

        {

            throw new NotImplementedException();

        }
    }
}
