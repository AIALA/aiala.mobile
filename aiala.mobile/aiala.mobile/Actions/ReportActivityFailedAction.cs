using Redux;

namespace aiala.mobile.Actions
{
    public class ReportActivityFailedAction : IAction
    {
        public string Message { get; }

        public ReportActivityFailedAction(string message)
        {
            Message = message;
        }
    }
}
