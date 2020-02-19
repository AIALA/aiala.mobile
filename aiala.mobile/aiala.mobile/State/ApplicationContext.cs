using System.Collections.Generic;
using Redux;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Services;
using xappido.Mobile.Models;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.State;
using xappido.Mobile;

namespace aiala.mobile
{
    public class ApplicationContext : AppContext<ApplicationState, ViewModels.HomeViewModel>
    {
        public ApplicationContext(IStore<ApplicationState> applicationStore, IStore<SystemAppState> systemStore, INavigationService navigationService, IRefreshTimer refreshTimer, ISettingsService settingsService, ILoggerService loggerService)
            : base(applicationStore, systemStore, navigationService, refreshTimer, settingsService, loggerService)
        {
        }

        public override void OnPushNotificationOpened(object source, PushNotificationCategory category, IDictionary<string, object> Data)
        {
            // TODO do something...
            return;
        }

        public override void OnPushNotificationReceived(object source, IDictionary<string, object> data)
        {
            // TODO do something....
        }

        public override void OnRefreshTimerRaised()
        {
            var state = _appStore.GetState();

            if (!state.IsHttpRequestProcessing)
            {
                _loggerService.LogDebug($"Refreshing data through timer interval {_settingsService.RefreshTimeInterval} seconds");

                // TODO
                //_store.Dispatch(new RefreshSelectedSwitchRequestAction(true));
            }
            else
            {
                _loggerService.LogDebug($"Refreshing data through timer interval skipped because another request is running");
            }
        }
    }
}
