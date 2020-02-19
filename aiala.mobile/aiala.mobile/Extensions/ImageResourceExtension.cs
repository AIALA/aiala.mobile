using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace aiala.mobile.Extensions
{

    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            var currentAssembly = typeof(ImageResourceExtension).GetTypeInfo().Assembly;
            var currentAssemblyName = currentAssembly.GetName();

            if (Source.StartsWith(currentAssemblyName.Name) == false)
            {
                Source = $"{currentAssemblyName.Name}.{Source}";
            }

            Source = Source.Replace("/", ".");

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source, currentAssembly);

            return imageSource;
        }
    }
}
