using aiala.mobile.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace aiala.mobile.Converters
{
    public class TaskStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(bool) && value is DayTaskState stateValue)
            {
                if (parameter is DayTaskState parameterValue)
                    return stateValue == parameterValue;
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
