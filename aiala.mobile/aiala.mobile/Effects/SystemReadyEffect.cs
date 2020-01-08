using aiala.mobile.Actions;
using Redux;
using System;
using System.Threading.Tasks;
using xappido.Mobile;
using xappido.Mobile.Core.Actions;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.Redux;

namespace aiala.mobile.Effects
{
    public class SystemReadyEffect : Effect<SystemIsReadyAction>
    {
        private readonly IStore<ApplicationState> _appStore;
        private readonly IAppContext _appContext;

        public SystemReadyEffect(IStore<ApplicationState> appStore, IAppContext appContext, SystemIsReadyAction action) : base(action)
        {
            _appStore = appStore;
            _appContext = appContext;
        }

        public override async Task OnExecute(SystemIsReadyAction action)
        {
            // load user info
            _appStore.Dispatch(new LoadUserInfoAction());

            // update / load scheduler
            _appStore.Dispatch(new LoadScheduleAction(DateTime.Today, DateTime.Today.AddDays(3)));

            // load settings
            _appStore.Dispatch(new LoadSettingsAction());

            // load gallery
            _appStore.Dispatch(new LoadGalleryAction());

            await _appContext.StartUi();
        }
    }
}
