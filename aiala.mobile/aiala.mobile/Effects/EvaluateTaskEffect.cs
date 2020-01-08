using aiala.mobile.Actions;
using aiala.mobile.Models;
using Redux;
using System.Threading.Tasks;
using xappido.Mobile;
using xappido.Mobile.Core.Redux;

namespace aiala.mobile.Effects
{
    public class EvaluateTaskEffect : Effect<EvaluateTaskAction>
    {
        private readonly IStore<ApplicationState> _appStore;

        public EvaluateTaskEffect(IStore<ApplicationState> appStore, IAppContext appContext, EvaluateTaskAction action) : base(action)
        {
            _appStore = appStore;
        }

        public override Task OnExecute(EvaluateTaskAction action)
        {
            var state = _appStore.GetState();
            var currentSchedule = state.Selected?.Schedule;// .Schedule.GetCurrentSchedule();

            if (currentSchedule != null)
            {
                var upcomingTasks = currentSchedule.GetFilteredTasks(state.TaskFilter);
                var currentTask = currentSchedule.GetCurrentTask();

                //var steps = currentTask.Steps;
                //currentTask = currentTask.DeepClone();
                //currentTask.Steps = steps;

                _appStore.Dispatch(new CurrentStateUpdateAction(currentTask, upcomingTasks));
            }
            else
            {
                _appStore.Dispatch(new CurrentStateUpdateAction(null, null));
            }

            return Task.CompletedTask;
        }
    }
}
