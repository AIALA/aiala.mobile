using System;
using System.Globalization;
using System.Reflection;
using Xamarin.Forms;

namespace aiala.mobile.Converters
{
    public class StringToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Uri uri)
            {
                return ImageSource.FromUri(uri);
            }
            if (value is string uriString && !string.IsNullOrEmpty(uriString) && Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute))
            {
                return ImageSource.FromUri(new Uri(uriString));
            }
            else
            {
                var currentAssembly = typeof(aiala.mobile.App).GetTypeInfo().Assembly;
                var currentAssemblyName = currentAssembly.GetName();

                var imageResource = $"{currentAssemblyName.Name}.Assets.empty.jpg";
                var fallback = ImageSource.FromResource(imageResource, currentAssembly);

                return fallback;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
