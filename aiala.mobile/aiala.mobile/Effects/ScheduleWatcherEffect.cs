using aiala.mobile.Actions;
using aiala.mobile.Models;
using Redux;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using xappido.Mobile;
using xappido.Mobile.Core.Actions;
using xappido.Mobile.Core.Redux;

namespace aiala.mobile.Effects
{
    public class ScheduleWatcherEffect : Effect<SystemIsReadyAction>
    {
        private readonly IStore<ApplicationState> _appStore;

        public ScheduleWatcherEffect(IStore<ApplicationState> appStore, IAppContext appContext, SystemIsReadyAction action) : base(action)
        {
            _appStore = appStore;
        }

        public override Task OnExecute(SystemIsReadyAction action)
        {
            _appStore
                .DistinctUntilChanged(state => new { state.Schedule, state.TaskFilter })
                .Subscribe(UpdateScheduler);

            return Task.CompletedTask;
        }

        private void UpdateScheduler(ApplicationState state)
        {
            CurrentScheduleUpdateAction scheduleUpdate;
            CurrentStateUpdateAction stateUpdate;

            var currentSchedule = state.Schedule.GetCurrentSchedule();
            scheduleUpdate = new CurrentScheduleUpdateAction(currentSchedule);

            if (currentSchedule != null)
            {
                var upcomingTasks = currentSchedule.GetFilteredTasks(state.TaskFilter);
                var currentTask = currentSchedule.GetCurrentTask();
                stateUpdate = new CurrentStateUpdateAction(currentTask, upcomingTasks);
            }
            else
            {
                stateUpdate = new CurrentStateUpdateAction(null, null);
            }

            _appStore.Dispatch(scheduleUpdate);
            _appStore.Dispatch(stateUpdate);
        }
    }
}
