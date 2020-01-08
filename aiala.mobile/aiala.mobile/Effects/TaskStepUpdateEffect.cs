using aiala.mobile.Actions;
using Redux;
using System.Threading.Tasks;
using xappido.Mobile.Core.Redux;
using System.Linq;

namespace aiala.mobile.Effects
{
    public class TaskStepUpdateEffect : Effect<TaskStepUpdateAction>
    {
        private readonly IStore<ApplicationState> _store;

        public TaskStepUpdateEffect(IStore<ApplicationState> store, TaskStepUpdateAction action) : base(action)
        {
            _store = store;
        }

        public override Task OnExecute(TaskStepUpdateAction action)
        {
            var state = _store.GetState();

            var task = state.Selected?.Schedule?.Tasks?.FirstOrDefault(q => q.Id == action.TaskId);

            if (task == null)
                return Task.CompletedTask;

            var step = task.Steps.FirstOrDefault(q => q.Id == action.StepId);

            if (step == null)
                return Task.CompletedTask;

            step.State = action.State;

            _store.Dispatch(new TaskUpdateSuccessAction(task));
            //_store.Dispatch(new StepUpdateSuccessAction(task));

            return Task.CompletedTask;
        }
    }
}
