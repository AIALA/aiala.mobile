using aiala.mobile.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace aiala.mobile.Converters
{
    public class TaskOverdueToVisiblityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(bool) && value is DayTask task)
            {
                if(parameter is string inverseParameter)
                {
                    if (inverseParameter.Equals("inverse", StringComparison.InvariantCultureIgnoreCase))
                        return !task.IsOverdue();
                }

                return task.IsOverdue();
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
