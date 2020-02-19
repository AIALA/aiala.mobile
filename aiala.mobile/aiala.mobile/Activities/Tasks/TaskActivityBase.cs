using System;
using System.Collections.Generic;
using System.Text;

namespace aiala.mobile.Activities.Tasks
{
    public class TaskActivityBase : ActivityBase
    {
        public TaskActivityBase(Guid taskId, DateTimeOffset timestamp) : base(timestamp)
        {
            TaskId = taskId;
            ActiveTaskId = taskId;
        }

        public Guid TaskId { get; }
    }
}
