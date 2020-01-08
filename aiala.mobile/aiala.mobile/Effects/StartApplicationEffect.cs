using aiala.mobile.Actions;
using Redux;
using System.Threading.Tasks;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.Redux;

namespace aiala.mobile.Effects
{
    public class StartApplicationEffect : Effect<LoadUserInfoSuccessAction>
    {
        private readonly IStore<ApplicationState> _store;
        private readonly INavigationService _navigationService;

        public StartApplicationEffect(IStore<ApplicationState> store, INavigationService navigationService, LoadUserInfoSuccessAction action) : base(action)
        {
            _store = store;
            _navigationService = navigationService;
        }

        public override async Task OnExecute(LoadUserInfoSuccessAction action)
        {
            // navigate for example to current task
        }
    }
}
