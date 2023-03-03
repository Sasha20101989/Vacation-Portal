using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows;
using Vacation_Portal.MVVM.ViewModels.For_Pages;
using Vacation_Portal.MVVM.Models;

namespace Vacation_Portal.Extensions
{
    public class VacationStatusNameToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(value is Vacation vacation) 
            {
                if(vacation.Name == MyEnumExtensions.ToDescriptionString(VacationName.Experience) ||
                    vacation.Name == MyEnumExtensions.ToDescriptionString(VacationName.Irregularity)) {
                    if(vacation.Vacation_Status_Name == MyEnumExtensions.ToDescriptionString(Statuses.Approved)) {
                        return Visibility.Visible;
                    }
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
