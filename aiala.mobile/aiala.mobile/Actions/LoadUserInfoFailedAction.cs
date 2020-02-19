using Redux;

namespace aiala.mobile.Actions
{
    public class LoadUserInfoFailedAction : IAction
    {
        public LoadUserInfoFailedAction(string errorMessage)
        {
            Message = errorMessage;
        }

        public string Message { get; }
    }
}
