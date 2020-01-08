using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace aiala.mobile.Converters
{
    public class MarkdownLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var formatted = new FormattedString();

            if (targetType == typeof(FormattedString) && value is string inputText)
            {
                var sections = ProcessString(inputText);

                foreach (var item in sections)
                {
                    formatted.Spans.Add(CreateSpan(item, parameter));
                }
            }
            return formatted;
        }

        private Span CreateSpan(StringSection section, object parameter)
        {
            var span = new Span()
            {
                Text = section.Text
            };

            if (!string.IsNullOrEmpty(section.Link))
            {
                if (parameter is Binding binding)
                {
                    var tapRecognizer = new TapGestureRecognizer()
                    {
                        CommandParameter = section.Link
                    };

                    tapRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, new Binding(binding.Path));

                    span.GestureRecognizers.Add(tapRecognizer);
                }
                else
                {
                    span.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        Command = _navigationCommand,
                        CommandParameter = section.Link
                    });
                }

                var color = (Color) Application.Current.Resources["primaryColor"];
                span.TextColor = color;
                span.TextDecorations = TextDecorations.Underline;

                // Underline coming soon from https://github.com/xamarin/Xamarin.Forms/pull/2221
                // Currently available in Nightly builds if you wanted to try, it does work :)
                // As of 2018-07-22. But not avail in 3.2.0-pre1.
                // span.TextDecorations = TextDecorations.Underline;
            }

            return span;
        }

        public IList<StringSection> ProcessString(string rawText)
        {
            const string spanPattern = @"\[(.*?)\]\(.*?\)";

            MatchCollection collection = Regex.Matches(rawText, spanPattern, RegexOptions.Singleline);

            var sections = new List<StringSection>();

            var lastIndex = 0;

            foreach (Match item in collection)
            {
                var foundText = item.Value;
                sections.Add(new StringSection() { Text = rawText.Substring(lastIndex, item.Index) });
                lastIndex += item.Index + item.Length;

                // Get HTML href 
                var html = new StringSection()
                {
                    Text = Regex.Match(item.Value, @"\[(.*?)\]").Value.Replace("[", string.Empty).Replace("]", string.Empty),
                    Link = Regex.Match(item.Value, @"\((.*?)\)").Value.Replace("(", string.Empty).Replace(")", string.Empty),
                };

                sections.Add(html);
            }

            sections.Add(new StringSection() { Text = rawText.Substring(lastIndex) });

            return sections;
        }

        public class StringSection
        {
            public string Text { get; set; }
            public string Link { get; set; }
        }

        private ICommand _navigationCommand = new Command<string>((url) =>
        {
            Device.OpenUri(new Uri(url));
        });

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
