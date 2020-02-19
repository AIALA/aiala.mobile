using aiala.mobile.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace aiala.mobile.Converters
{
    public class TaskFilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(bool) && value is TaskFilter filterValue)
            {
                if (parameter is TaskFilter parameterValue)
                    return (filterValue == parameterValue);
                else
                    return false;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
