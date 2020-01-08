using System;

namespace aiala.mobile.Activities.Tasks
{
    public class TaskOffsetRequestActivity : TaskActivityBase
    {
        public TaskOffsetRequestActivity(TimeSpan delayedUntil, Guid taskId, DateTimeOffset timestamp) : base(taskId, timestamp)
        {
            DelayedUntil = delayedUntil;
        }

        public TimeSpan DelayedUntil { get; }
    }
}
