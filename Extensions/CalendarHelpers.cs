using System;
using System.Collections.Generic;
using System.Text;

namespace Vacation_Portal.Extensions
{
   public class CalendarHelpers
    {
        public DateTime JanuaryStart
        {
            get {
                return new DateTime(DateTime.Now.Year, 1, 1);
            }
        }
        public DateTime JanuaryEnd
        {
            get { 
                return new DateTime(DateTime.Now.Year, 1, DateTime.DaysInMonth(DateTime.Now.Year, 1));
            }
        }

        public DateTime FebruaryStart
        {
            get
            {
                return new DateTime(DateTime.Now.Year, 1, 1);
            }
        }
        public DateTime FebruaryEnd
        {
            get
            {
                return new DateTime(DateTime.Now.Year, 1, DateTime.DaysInMonth(DateTime.Now.Year, 1));
            }
        }
    }
}
