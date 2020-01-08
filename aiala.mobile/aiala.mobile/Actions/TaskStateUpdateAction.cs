using aiala.mobile.Models;
using Redux;
using System;

namespace aiala.mobile.Actions
{
    public class TaskStateUpdateAction : IAction
    {
        public TaskStateUpdateAction(Guid taskId, DayTaskState state, DayTaskFeedback feedback)
        {
            TaskId = taskId;
            State = state;
            Feedback = feedback;
        }

        public Guid TaskId { get; }

        public DayTaskState State { get; }
        public DayTaskFeedback Feedback { get; }
    }
}
