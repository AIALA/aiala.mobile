using aiala.mobile.Models;
using Redux;
using System.Collections.ObjectModel;

namespace aiala.mobile.Actions
{
    public class CurrentStateUpdateAction : IAction
    {
        public DayTask CurrentTask { get; set; }

        public ObservableCollection<DayTask> UpcomingTasks { get; set; }

        public CurrentStateUpdateAction(DayTask currentTask, ObservableCollection<DayTask> upcomingTasks)
        {
            this.CurrentTask = currentTask;
            this.UpcomingTasks = upcomingTasks;
        }
    }
}
