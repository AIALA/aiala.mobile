using Redux;

namespace aiala.mobile.Actions
{
    public class LoadSettingsFailedAction : IAction
    {
        public LoadSettingsFailedAction(string errorMessage)
        {
            Message = errorMessage;
        }

        public string Message { get; }
    }
}
