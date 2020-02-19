using aiala.mobile.Models;
using Redux;
using System;

namespace aiala.mobile.Actions
{
    public class TaskStepUpdateAction : IAction
    {
        public TaskStepUpdateAction(Guid taskId, Guid stepId, DayStepState state)
        {
            TaskId = taskId;
            StepId = stepId;
            State = state;
        }

        public Guid TaskId { get; }

        public Guid StepId { get; }

        public DayStepState State { get; }
    }
}
