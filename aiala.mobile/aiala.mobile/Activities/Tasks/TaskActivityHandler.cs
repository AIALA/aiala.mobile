using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using aiala.mobile.Actions;
using aiala.mobile.BackgroundServices;
using aiala.mobile.Resources;
using Redux;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Models;
using xappido.Mobile.Core.Services;

namespace aiala.mobile.Activities.Tasks
{
    public class TaskActivityHandler : ActivityHandler<TaskActivityBase>,
        IActivityHandler<TaskFeedbackActivity>,
        IActivityHandler<TaskOffsetRequestActivity>
    {
        private readonly IStore<AuthState> _authStore;
        private readonly IStore<ApplicationState> _appStore;
        private readonly IRestClient _restClient;

        public TaskActivityHandler(IStore<AuthState> authStore, IStore<ApplicationState> appStore, IRestClient restClient)
        {
            _authStore = authStore;
            _appStore = appStore;
            _restClient = restClient;
        }

        public override async Task<bool> HandleInternal(TaskActivityBase activity)
        {
            var url = "v1/"
                .AppendPathSegment("activities")
                .AppendPathSegment("task")
                .AppendPathSegment(activity.TaskId);

            if (activity is TaskFeedbackActivity)
            {
                url = url.AppendPathSegment("feedback");
            }
            else if (activity is TaskOffsetRequestActivity)
            {
                url = url.AppendPathSegment("delay");
            }
            else
            {
                // Not implemented type
                return false;
            }

            var authState = _authStore.GetState();
            var restResult = await _restClient.PostAsync<TaskActivityBase, object>(url.ToString(), activity, authState.AccessTokenType, authState.AccessToken);

            if (restResult.Type == ServiceResultType.AccessDenied || restResult.Type == ServiceResultType.Error)
            {
                _appStore.Dispatch(new ReportActivityFailedAction(ServerMessages.TaskActivity_Error));
                return false;
            }

            if (restResult.Type == ServiceResultType.Ok)
            {
                _appStore.Dispatch(new ReportActivitySuccessAction());
                return true;
            }

            return false;
        }
    }
}
