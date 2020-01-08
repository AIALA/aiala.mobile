using Redux;

namespace aiala.mobile.Actions
{
    public class BackgroundProcessorStateAction : IAction
    {
        public BackgroundProcessorStateAction(bool isRunning)
        {
            IsRunning = isRunning;
        }

        public bool IsRunning { get; }
    }
}
