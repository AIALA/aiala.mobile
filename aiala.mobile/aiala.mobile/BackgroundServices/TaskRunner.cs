using aiala.mobile.Actions;
using aiala.mobile.Configuration;
using Redux;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace aiala.mobile.BackgroundServices
{
    public class TaskRunner
    {
        private readonly ActivityQueueWorker _taskStatePusher;
        private readonly IApplicationSettingsService _settingsService;
        private readonly IStore<ApplicationState> _appStore;

        public TaskRunner()
        {
            try
            {
                _taskStatePusher = xappido.Mobile.AppContainer.Instance().Resolve<ActivityQueueWorker>();
                _settingsService = xappido.Mobile.AppContainer.Instance().Resolve<IApplicationSettingsService>();
                _appStore = xappido.Mobile.AppContainer.Instance().Resolve<IStore<ApplicationState>>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task Run(CancellationToken token)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromMinutes(_settingsService.ScheduleRefreshMinutes));
                    if (Connectivity.NetworkAccess != NetworkAccess.None)
                    {
                        _appStore.Dispatch(new LoadScheduleAction(DateTime.Now.Date, DateTime.Now.Date.AddDays(3)));
                    }
                }
            }, token).ConfigureAwait(false);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            await Task.Run(async () =>
            {
                if (_taskStatePusher != null)
                {
                    await _taskStatePusher.Run(token);
                }
            }, token);
        }
    }
}
