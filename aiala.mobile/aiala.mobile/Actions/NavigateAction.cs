using aiala.mobile.Models;
using Redux;

namespace aiala.mobile.Actions
{
    public class NavigateAction : IAction
    {
        public NavigateAction(NavigationState navigationState)
        {
            NavigationState = navigationState;
        }

        public NavigationState NavigationState { get; }
    }
}
