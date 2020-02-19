using Redux;
using System;

namespace aiala.mobile.Actions
{
    public class LoadScheduleAction : IAction
    {
        public LoadScheduleAction(DateTime dateFrom, DateTime dateTo)
        {
            DateFrom = dateFrom;
            DateTo = dateTo;
        }

        public DateTime DateFrom { get; }
        public DateTime DateTo { get; }
    }
}
