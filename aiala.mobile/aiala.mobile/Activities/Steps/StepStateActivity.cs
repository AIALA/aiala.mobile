using System;
using aiala.mobile.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace aiala.mobile.Activities.Steps
{
    public class StepStateActivity : StepActivityBase
    {
        public StepStateActivity(DayStepState state, Guid stepId, DateTimeOffset timestamp) : base(stepId, timestamp)
        {
            State = state;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public DayStepState State { get; }
    }
}
