using aiala.mobile.Activities;
using Redux;

namespace aiala.mobile.Actions
{
    public class ReportActivityAction : IAction
    {
        public ReportActivityAction(ActivityPriority priority, ActivityBase activity, bool dispatchInstantly = false)
        {
            Priority = priority;
            Activity = activity;
            DispatchInstantly = dispatchInstantly;
        }

        public ActivityPriority Priority { get; }

        public ActivityBase Activity { get; }

        public bool DispatchInstantly { get; }
    }
}
