using aiala.mobile.BackgroundServices;
using aiala.mobile.iOS.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

namespace aiala.mobile.iOS.Services
{
    public class TaskRunnerService
    {
        nint _taskId;
        CancellationTokenSource _cts;

        public bool IsStarted { get; set; }

        public async Task Start()
        {
            _cts = new CancellationTokenSource();
            _taskId = UIApplication.SharedApplication.BeginBackgroundTask("TaskRunner", OnExpiration);

            try
            {
                var message = new TaskRunnerStartedMessage();
                Device.BeginInvokeOnMainThread(
                    () => MessagingCenter.Send<TaskRunnerStartedMessage>(message, nameof(TaskRunnerStartedMessage))
                );

                var runner = new TaskRunner();
                await runner.Run(_cts.Token);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                if (_cts.IsCancellationRequested)
                {
                    var message = new TaskRunnerCancelledMessage();
                    Device.BeginInvokeOnMainThread(
                        () => MessagingCenter.Send(message, nameof(TaskRunnerCancelledMessage))
                    );
                }
            }

            UIApplication.SharedApplication.EndBackgroundTask(_taskId);
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        void OnExpiration()
        {
            _cts.Cancel();
        }
    }
}
