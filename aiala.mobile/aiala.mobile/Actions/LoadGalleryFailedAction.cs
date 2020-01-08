using Redux;

namespace aiala.mobile.Actions
{
    public class LoadGalleryFailedAction : IAction
    {
        public LoadGalleryFailedAction(string errorMessage)
        {
            Message = errorMessage;
        }

        public string Message { get; }
    }
}
