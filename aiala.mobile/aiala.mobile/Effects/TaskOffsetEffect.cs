using aiala.mobile.Actions;
using aiala.mobile.Models;
using Redux;
using System.Threading.Tasks;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.Redux;
using Newtonsoft.Json;
using System.Linq;
using aiala.mobile.Activities;
using System;
using aiala.mobile.Activities.Tasks;

namespace aiala.mobile.Effects
{
    public class TaskOffsetEffect : Effect<TaskOffsetAction>
    {
        private readonly IStore<ApplicationState> _store;

        public TaskOffsetEffect(IStore<ApplicationState> store, TaskOffsetAction action) : base(action)
        {
            _store = store;
        }

        public override Task OnExecute(TaskOffsetAction action)
        {
            var state = _store.GetState();

            var task = state.Selected?.Schedule?.Tasks?.FirstOrDefault(q => q.Id == action.TaskId);

            if (task == null)
                return Task.CompletedTask;

            task.AddDelay(15.Minutes());

            var delayedUntil =  task.End.Add(task.ExpirationOffset);

            var currentStep = task.Steps.CurrentStep();

            _store.Dispatch(new ReportActivityAction(ActivityPriority.High, new TaskOffsetRequestActivity(delayedUntil, task.Id, DateTimeOffset.UtcNow)));
            _store.Dispatch(new TaskUpdateSuccessAction(task));

            return Task.CompletedTask;
        }
    }
}
