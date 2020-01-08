using System;

namespace aiala.mobile.Models
{
    public static class TimeSpanExtensions
    {
        public static bool Between(this TimeSpan dueTime, TimeSpan before, TimeSpan after)
        {
            var isBetween = before <= dueTime && after >= dueTime;
            return isBetween;
        }

        public static bool Before(this TimeSpan time, TimeSpan dueTime)
        {
            var isBefore = time < dueTime;
            return isBefore;
        }

        public static bool After(this TimeSpan time, TimeSpan dueTime)
        {
            var isAfter = time > dueTime;
            return isAfter;
        }

        public static TimeSpan Minutes(this Int32 minutes)
        {
            return new System.TimeSpan(0, minutes, 0);
        }
    }
   
}
