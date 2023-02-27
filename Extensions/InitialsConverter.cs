using System;
using System.Globalization;
using System.Windows.Data;

namespace Vacation_Portal.Extensions {
    public class InitialsConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value is string displayName) {
                string[] nameParts = displayName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if(nameParts.Length >= 2) {
                    char firstNameInitial = nameParts[0][0];
                    char lastNameInitial = nameParts[1][0];
                    return string.Concat(firstNameInitial, lastNameInitial).ToString().ToUpper();
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
