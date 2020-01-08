using aiala.mobile.Actions;
using aiala.mobile.Models;
using Redux;
using System.Collections.Generic;
using System.Threading.Tasks;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Models;
using xappido.Mobile.Core.Redux;
using xappido.Mobile.Core.Services;
using Flurl;
using System;
using aiala.mobile.Extensions;
using aiala.mobile.Resources;
using aiala.mobile.Activities;
using aiala.mobile.BackgroundServices;
using System.Linq;
using aiala.mobile.Activities.Steps;
using aiala.mobile.Activities.Tasks;

namespace aiala.mobile.Effects
{
    public class LoadScheduleEffect : Effect<LoadScheduleAction>
    {
        private readonly IRestClient _restClient;
        private readonly IStore<AuthState> _authStore;
        private readonly IStore<ApplicationState> _appStore;

        public LoadScheduleEffect(IStore<AuthState> authStore, IStore<ApplicationState> appStore, IRestClient restClient, LoadScheduleAction action) : base(action)
        {
            _restClient = restClient;
            _authStore = authStore;
            _appStore = appStore;
        }

        public override async Task OnExecute(LoadScheduleAction action)
        {
            var authState = _authStore.GetState();

            var url = "v1/"
                .AppendPathSegment("schedule")
                .SetQueryParam("from", action.DateFrom.ToUtc().ToString("o"))
                .SetQueryParam("to", action.DateTo.ToUtc().ToString("o"))
                .ToString();

            var restResult = await _restClient.GetAsync<List<DaySchedule>>(url, authState.AccessTokenType, authState.AccessToken);

            if (restResult.Type == ServiceResultType.AccessDenied)
            {
                _authStore.Dispatch(new LoadScheduleFailedAction(ServerMessages.Schedule_LoadError));
                return;
            }

            if (restResult.Type == ServiceResultType.Error)
            {
                _authStore.Dispatch(new LoadScheduleFailedAction(ServerMessages.Schedule_LoadError));
                return;
            }

            if (restResult.Type == ServiceResultType.Ok)
            {
                // Apply only to today
                var today = restResult.Result.FirstOrDefault(d => d.Date == DateTime.Now.Date);

                if (today != null)
                {
                    var allActivities = App.Current.Properties.PeekAll<ActivityBase>(ActivityQueues.High);
                    if (allActivities.Count != 0)
                    {
                        var stepStateActivities = allActivities.OfType<StepStateActivity>().ToList();
                        var taskFeedbackActivities = allActivities.OfType<TaskFeedbackActivity>().ToList();
                        var taskOffsetActivities = allActivities.OfType<TaskOffsetRequestActivity>().ToList();

                        foreach (var task in today.Tasks)
                        {
                            var lastTaskOffset = taskOffsetActivities.Where(toa => toa.ActiveTaskId == task.Id || toa.TaskId == task.Id).OrderBy(toa => toa.DelayedUntil).LastOrDefault();
                            if (lastTaskOffset != null)
                            {
                                task.ExpirationOffset = lastTaskOffset.DelayedUntil;
                            }

                            var lastFeedback = taskFeedbackActivities.Where(tfa => tfa.ActiveTaskId == task.Id || tfa.TaskId == task.Id).OrderBy(tfa => tfa.Timestamp).LastOrDefault();
                            if (lastFeedback != null)
                            {
                                task.Feedback = lastFeedback.Feedback;
                            }

                            foreach (var step in task.Steps)
                            {
                                var lastState = stepStateActivities.Where(ssa => ssa.StepId == step.Id).OrderBy(ssa => ssa.Timestamp).LastOrDefault();
                                if (lastState != null)
                                {
                                    step.State = lastState.State;
                                }
                            }

                            task.State = task.Steps.All(s => s.State == DayStepState.Done) ? DayTaskState.Done : DayTaskState.Undone;
                        }
                    }
                }

                _appStore.Dispatch(new LoadScheduleSuccessAction(restResult.Result));
                return;
            }

            _authStore.Dispatch(new LoadScheduleFailedAction(ServerMessages.Schedule_LoadError));
        }
    }
}
