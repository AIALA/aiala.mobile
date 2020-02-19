using aiala.mobile.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace aiala.mobile.Converters
{
    public class NavigationStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(bool) && value is NavigationState stateValue)
            {
                if (parameter is NavigationState parameterValue)
                    return !(stateValue == parameterValue);
                else
                    return true;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
