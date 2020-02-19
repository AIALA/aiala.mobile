using System;
using Xamarin.Forms.Xaml;

namespace aiala.mobile
{
    public static class DateTimeHelper
    {
        public static void Init(DateTime? dateTime = null)
        {
            _dateTime = dateTime;
        }

        private static DateTime? _dateTime;

        public static DateTime Now { get => _dateTime ?? DateTime.Now; }

        public static DateTime Today { get => _dateTime?.Date ?? DateTime.Today; }
    }
}
