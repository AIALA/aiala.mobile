using Redux;
using System;

namespace aiala.mobile.Actions
{
    public class TaskOffsetAction : IAction
    {
        public TaskOffsetAction(Guid taskId)
        {
            TaskId = taskId;
        }

        public Guid TaskId { get; }
    }
}
