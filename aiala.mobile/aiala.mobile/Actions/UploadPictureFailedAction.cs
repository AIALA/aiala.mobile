using Redux;

namespace aiala.mobile.Actions
{
    public class UploadPictureFailedAction : IAction
    {
        public UploadPictureFailedAction(string errorMessage)
        {
            Message = errorMessage;
        }

        public string Message { get; }
    }
}
