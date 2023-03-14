using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Vacation_Portal.Extensions
{
    public class ObservableCollectionConverter<T> : IValueConverter

    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)

        {

            if(value is ObservableCollection<T> collection)

                return collection;

            if(value is IEnumerable enumerable)

                return new ObservableCollection<T>(enumerable.Cast<T>());

            return null;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)

        {

            throw new NotImplementedException();

        }

    }
}
