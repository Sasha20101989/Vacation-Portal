using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using Vacation_Portal.MVVM.Models;
using Vacation_Portal.MVVM.Models.HorizontalCalendar;

namespace Vacation_Portal.Extensions
{
    public class DayInVacationConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length < 2 ||
                !(values[0] is HorizontalDay date) ||
                !(values[1] is ObservableCollection<Vacation> currentPersonVacations) ||
                !(values[2] is ObservableCollection<Subordinate> subordinates))
            {
                return Brushes.Transparent;
            }
            if(currentPersonVacations.Count == 0)
            {
                return Brushes.Transparent;
            }

            IEnumerable<DateTime> grayDaysList = currentPersonVacations.SelectMany(v => v.DateRange);

            int currentPersonid = 0;

            List<DateTime> grayDays = new List<DateTime>();

            foreach(var vacation in currentPersonVacations)
            {
                currentPersonid = vacation.User_Id_SAP;
            }
            
            if(currentPersonid != 0)
            {
                foreach(Subordinate subordinate in subordinates)
                {
                    if(subordinate.Id_SAP == currentPersonid)
                    {
                        continue;
                    }

                    foreach(DateTime grayDay in grayDaysList)
                    {
                        foreach(Vacation subordinateVacation in subordinate.Subordinate_Vacations)
                        {
                            if(subordinateVacation.DateRange.Any(d => d == date.Date && grayDay == date.Date))
                            {
                                return Brushes.Red;
                            }
                        }
                    }
                }
                if(grayDaysList.Any(d => d == date.Date))
                {
                    return Brushes.LightGray;
                }
            }

            return Brushes.Transparent;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
