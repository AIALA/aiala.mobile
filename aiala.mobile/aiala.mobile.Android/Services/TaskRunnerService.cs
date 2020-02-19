using Android.App;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using System.Threading;
using System.Threading.Tasks;
using aiala.mobile.BackgroundServices;

namespace aiala.mobile.Droid.Services
{
    [Service]
    public class TaskRunnerService : Service
    {
        public static bool IsStarted { get; set; }

        CancellationTokenSource _cts;

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            _cts = new CancellationTokenSource();

            Task.Run(() => {
                try
                {
                    var message = new TaskRunnerStartedMessage();
                    Device.BeginInvokeOnMainThread(
                        () => MessagingCenter.Send<TaskRunnerStartedMessage>(message, nameof(TaskRunnerStartedMessage))
                    );

                    var runner = new TaskRunner();
                    runner.Run(_cts.Token).Wait();
                }
                catch (Android.Accounts.OperationCanceledException)
                {
                }
                finally
                {
                    if (_cts.IsCancellationRequested)
                    {
                        var message = new TaskRunnerCancelledMessage();
                        Device.BeginInvokeOnMainThread(
                            () => MessagingCenter.Send<TaskRunnerCancelledMessage>(message, nameof(TaskRunnerCancelledMessage))
                        );
                    }
                }

            }, _cts.Token);

            return StartCommandResult.Sticky;
        }

        public override void OnDestroy()
        {
            if (_cts != null)
            {
                _cts.Token.ThrowIfCancellationRequested();

                _cts.Cancel();
            }
            base.OnDestroy();
        }
    }
}