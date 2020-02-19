using System;
using System.Globalization;
using Xamarin.Forms;

namespace aiala.mobile.Converters
{
    public class ObjectToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value != null;

            if (parameter is string inverseParameter)
            {
                if (inverseParameter.Equals("inverse", StringComparison.InvariantCultureIgnoreCase))
                    return !result;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
