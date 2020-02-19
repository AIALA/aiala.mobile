using System;
using System.Collections.Generic;
using System.Text;
using aiala.mobile.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace aiala.mobile.Activities.Emergencies
{
    public class EmergencyMoodActivity : EmergencyActivityBase
    {
        public EmergencyMoodActivity(EmergencyState mood, Guid emergencyId, DateTimeOffset timestamp) : base(emergencyId, timestamp)
        {
            Mood = mood;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public EmergencyState Mood { get; }
    }
}
