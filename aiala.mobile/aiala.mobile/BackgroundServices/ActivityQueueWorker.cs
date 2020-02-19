using Autofac;
using Flurl;
using aiala.mobile.Actions;
using aiala.mobile.Activities;
using aiala.mobile.Extensions;
using aiala.mobile.Models;
using Redux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Models;
using xappido.Mobile.Core.Services;

namespace aiala.mobile.BackgroundServices
{
    public class ActivityQueueWorker
    {
        private readonly IStore<ApplicationState> _appStore;
        private readonly ActivityHandlerService _activityHandlerService;

        public ActivityQueueWorker(IStore<ApplicationState> appStore, ActivityHandlerService activityHandlerService)
        {
            _appStore = appStore;
            _activityHandlerService = activityHandlerService;
        }

        public async Task Run(CancellationToken token)
        {
            var failedItems = new List<(string queue, ActivityBase activity)>();

            do
            {
                token.ThrowIfCancellationRequested();

                var currentQueue = ActivityQueues.High;
                ActivityBase item = null;

                try
                {
                    item = await App.Current.Properties.Dequeue<ActivityBase>(currentQueue);

                    if (item == null)
                    {
                        currentQueue = ActivityQueues.Low;
                        item = await App.Current.Properties.Dequeue<ActivityBase>(currentQueue);
                    }

                    if (item != null)
                    {
                        var result = await _activityHandlerService.HandleActivity(item);

                        if (!result)
                        {
                            failedItems.Add((currentQueue, item));
                        }
                    }
                }
                catch (Exception)
                {
                    failedItems.Add((currentQueue, item));
                }

                // If no items in queue, re-add failed items, then wait. Otherwhise keep working the queue.
                if (App.Current.Properties.GetQueueItemsCount(ActivityQueues.High) + App.Current.Properties.GetQueueItemsCount(ActivityQueues.Low) == 0)
                {
                    foreach(var (queue, activity) in failedItems)
                    {
                        await App.Current.Properties.Enqueue(item, queue);
                    }
                    failedItems = new List<(string queue, ActivityBase activity)>();

                    await Task.Delay(5 * 1000);
                }
            }
            while (true);
        }
    }
}
