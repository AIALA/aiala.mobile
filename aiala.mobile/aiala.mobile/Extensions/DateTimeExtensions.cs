using System;
using System.Collections.Generic;
using System.Text;

namespace aiala.mobile.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToUtc(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, DateTimeKind.Utc);
        }
    }
}
