using System.Threading.Tasks;
using xappido.Mobile.Core.Redux;
using Xamarin.Forms;
using aiala.mobile.BackgroundServices;
using xappido.Mobile.Auth.Actions;

namespace aiala.mobile.Effects
{
    public class StopTaskRunnerEffect : Effect<LogoutSuccessAction>
    {
        public StopTaskRunnerEffect(LogoutSuccessAction action) : base(action)
        {
        }

        public override Task OnExecute(LogoutSuccessAction action)
        {
            // stop long running task
            var message = new StopTaskRunnerMessage();
            MessagingCenter.Send(message, nameof(StopTaskRunnerMessage));

            return Task.CompletedTask;
        }
    }
}
