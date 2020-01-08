using aiala.mobile.Models;
using Redux;

namespace aiala.mobile.Actions
{
    public class CurrentScheduleUpdateAction : IAction
    {
        public DaySchedule CurrentSchedule { get; set; }

        public CurrentScheduleUpdateAction(DaySchedule currentSchedule)
        {
            this.CurrentSchedule = currentSchedule;
        }
    }
}
