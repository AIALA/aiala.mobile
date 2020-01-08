using aiala.mobile.Models;
using Redux;
using System.Collections.Generic;

namespace aiala.mobile.Actions
{
    public class LoadScheduleSuccessAction : IAction
    {
        public LoadScheduleSuccessAction(List<DaySchedule> result)
        {
            Result = result;
        }

        public List<DaySchedule> Result { get; }
    }
}
