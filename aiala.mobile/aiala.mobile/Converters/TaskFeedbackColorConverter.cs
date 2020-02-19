using aiala.mobile.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace aiala.mobile.Converters
{
    public class TaskFeedbackColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = Application.Current.Resources["feedbackUnknown"];

            if (targetType == typeof(Color) && value is DayTaskFeedback feedbackValue)
            {
                switch(feedbackValue)
                {
                    case DayTaskFeedback.None:
                        color = Application.Current.Resources["feedbackUnknown"];
                        break;

                    case DayTaskFeedback.Good:
                        color = Application.Current.Resources["feedbackGreen"];
                        break;

                    case DayTaskFeedback.Medium:
                        color = Application.Current.Resources["feedbackYellow"];
                        break;

                    case DayTaskFeedback.Bad:
                        color = Application.Current.Resources["feedbackRed"];
                        break;
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
