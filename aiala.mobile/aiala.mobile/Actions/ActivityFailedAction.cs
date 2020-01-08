using Redux;

namespace aiala.mobile.Actions
{
    public class ActivityFailedAction : IAction
    {
        public ActivityFailedAction(string errorMessage)
        {
            Message = errorMessage;
        }

        public string Message { get; }
    }
}
