using Redux;

namespace aiala.mobile.Actions
{
    public class LoadScheduleFailedAction : IAction
    {
        public LoadScheduleFailedAction(string errorMessage)
        {
            Message = errorMessage;
        }

        public string Message { get; }
    }
}
