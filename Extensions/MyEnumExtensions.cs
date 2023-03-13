using System;
using System.ComponentModel;

namespace Vacation_Portal.Extensions {
    public static class MyEnumExtensions {
        public static string ToDescriptionString<TEnum>(this TEnum val) where TEnum : struct, Enum {
            DescriptionAttribute[] attributes = (DescriptionAttribute[]) val
                .GetType()
                .GetField(val.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }
}
