using aiala.mobile.Models;
using Redux;

namespace aiala.mobile.Actions
{
    public class LoadUserInfoSuccessAction : IAction
    {
        public LoadUserInfoSuccessAction(User result)
        {
            Result = result;
        }

        public User Result { get; }
    }
}
