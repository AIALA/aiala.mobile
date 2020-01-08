using aiala.mobile.Models;
using Redux;

namespace aiala.mobile.Actions
{
    public class TaskUpdateSuccessAction : IAction
    {
        public TaskUpdateSuccessAction(DayTask task)
        {
            Task = task;
        }

        public DayTask Task { get; }
    }
}
