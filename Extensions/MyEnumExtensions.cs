using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Vacation_Portal.Extensions
{
    public static class MyEnumExtensions
    {
        public static string ToDescriptionString(this MyColors val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val
               .GetType()
               .GetField(val.ToString())
               .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
