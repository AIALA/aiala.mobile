using System;

namespace aiala.mobile.Activities
{
    public class ActivityBase
    {
        public ActivityBase(DateTimeOffset timestamp)
        {
            Timestamp = timestamp;
        }

        public DateTimeOffset Timestamp { get; set; }

        public Guid? ActiveTaskId { get; set; }

        public Guid? ActiveStepId { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }
}
