using aiala.mobile.Models;
using Redux;

namespace aiala.mobile.Actions
{
    public class TaskFilterAction : IAction
    {
        public TaskFilterAction(TaskFilter filter)
        {
            TaskFilter = filter;
        }

        public TaskFilter TaskFilter { get; set; }
    }
}
