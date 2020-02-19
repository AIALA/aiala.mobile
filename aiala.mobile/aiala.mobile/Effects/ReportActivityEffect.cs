using System.Threading.Tasks;
using aiala.mobile.Actions;
using aiala.mobile.Activities;
using aiala.mobile.BackgroundServices;
using aiala.mobile.Extensions;
using aiala.mobile.Models;
using Redux;
using xappido.Mobile.Core.Redux;

namespace aiala.mobile.Effects
{
    public class ReportActivityEffect : Effect<ReportActivityAction>
    {
        private readonly IStore<ApplicationState> _appStore;
        private readonly ActivityHandlerService _handlerService;

        public ReportActivityEffect(ReportActivityAction action, IStore<ApplicationState> appStore, ActivityHandlerService handlerService) : base(action)
        {
            _appStore = appStore;
            _handlerService = handlerService;
        }

        public override async Task OnExecute(ReportActivityAction action)
        {
            var selected = _appStore.GetState().Selected;
            if (selected != null)
            {
                action.Activity.ActiveTaskId = selected.Task?.Id;
                action.Activity.ActiveStepId = (selected.Step as DayStep)?.Id;
            }

            if (action.DispatchInstantly)
            {
                var result = await _handlerService.HandleActivity(action.Activity);
                if (!result)
                {
                    // Retry via high queue
                    await App.Current.Properties.Enqueue(action.Activity, ActivityQueues.High);
                }
            }
            else
            {
                var queue = action.Priority == ActivityPriority.High ? ActivityQueues.High : ActivityQueues.Low;
                await App.Current.Properties.Enqueue(action.Activity, queue);
            }

            return;
        }
    }
}
