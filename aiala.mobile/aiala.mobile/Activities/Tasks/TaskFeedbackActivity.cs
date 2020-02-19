using System;
using aiala.mobile.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace aiala.mobile.Activities.Tasks
{
    public class TaskFeedbackActivity : TaskActivityBase
    {
        public TaskFeedbackActivity(DayTaskFeedback feedback, Guid taskId, DateTimeOffset timestamp) : base(taskId, timestamp)
        {
            Feedback = feedback;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public DayTaskFeedback Feedback { get; }
    }
}
