using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace aiala.mobile.Activities.Generic
{
    public class GenericActivity : ActivityBase
    {
        public GenericActivity(GenericActivityType activityType, DateTimeOffset timestamp) : base(timestamp)
        {
            ActivityType = activityType;
        }

        public GenericActivityType ActivityType { get; }

        public Dictionary<string, string> ActivityData { get; set; }
    }
}
