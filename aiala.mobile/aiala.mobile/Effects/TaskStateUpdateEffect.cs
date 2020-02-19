using aiala.mobile.Actions;
using aiala.mobile.Models;
using Redux;
using System.Threading.Tasks;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.Redux;
using System.Linq;

namespace aiala.mobile.Effects
{
    public class TaskStateUpdateEffect : Effect<TaskStateUpdateAction>
    {
        private readonly IStore<ApplicationState> _store;

        public TaskStateUpdateEffect(IStore<ApplicationState> store, TaskStateUpdateAction action) : base(action)
        {
            _store = store;
        }

        public override Task OnExecute(TaskStateUpdateAction action)
        {
            var state = _store.GetState();

            var task = state.Selected?.Schedule?.Tasks?.FirstOrDefault(q => q.Id == action.TaskId);

            if (task == null)
                return Task.CompletedTask;

            task.State = action.State;
            task.Feedback = action.Feedback;

            // clone
            //var steps = task.Steps;
            //task = task.DeepClone();
            //task.Steps = steps;

            _store.Dispatch(new TaskUpdateSuccessAction(task));
            _store.Dispatch(new EvaluateTaskAction());

            return Task.CompletedTask;
        }
    }
}
