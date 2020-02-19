using Redux;
using System.Threading.Tasks;
using xappido.Mobile;
using xappido.Mobile.Core.Actions;
using xappido.Mobile.Core.Redux;
using Xamarin.Forms;
using aiala.mobile.BackgroundServices;
using aiala.mobile.Actions;
using System;

namespace aiala.mobile.Effects
{

    public class LongRunningWatcherEffect : Effect<SystemIsReadyAction>
    {
        private readonly IStore<ApplicationState> _appStore;

        public LongRunningWatcherEffect(IStore<ApplicationState> appStore, IAppContext appContext, SystemIsReadyAction action) : base(action)
        {
            _appStore = appStore;

            
        }

       

        public override Task OnExecute(SystemIsReadyAction action)
        {
            // start long running task
            //var message = new StartTaskRunnerMessage();
            //MessagingCenter.Send(message, nameof(StartTaskRunnerMessage));

            return Task.CompletedTask;
        }
    }
}
