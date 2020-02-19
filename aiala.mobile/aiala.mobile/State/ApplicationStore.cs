using Redux;
using xappido.Mobile.State;

namespace aiala.mobile
{
    public class ApplicationStore : AppStore<ApplicationState>
    {
        public ApplicationStore(Reducer<ApplicationState> reducer, ApplicationState initialState = null, params Middleware<ApplicationState>[] middlewares)
            : base(reducer, initialState, middlewares)
        {
        }
    }
}