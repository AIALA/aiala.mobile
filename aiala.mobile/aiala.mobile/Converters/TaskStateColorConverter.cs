using aiala.mobile.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace aiala.mobile.Converters
{
    public class TaskStateColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = Application.Current.Resources["taskStateUnknown"];

            if (targetType == typeof(Color) && value is DayTask taskValue)
            {
                if(taskValue == null)
                    return Application.Current.Resources["taskStateUnknown"];

                if(taskValue.State == DayTaskState.Done)
                {
                    color = Application.Current.Resources["taskStateDone"];
                }
                else if(taskValue.State == DayTaskState.Undone)
                {
                    var isDelay = taskValue.IsOffsetExpired();

                    if(isDelay)
                        color = Application.Current.Resources["taskStateDelay"];
                    else
                        color = Application.Current.Resources["taskStateUndone"];
                }
                else
                {
                    color = Application.Current.Resources["taskStateUnknown"];
                }
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
