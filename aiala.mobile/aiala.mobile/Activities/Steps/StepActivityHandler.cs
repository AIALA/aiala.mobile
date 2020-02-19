using Flurl;
using aiala.mobile.Actions;
using aiala.mobile.BackgroundServices;
using aiala.mobile.Resources;
using Redux;
using System;
using System.Threading.Tasks;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Models;
using xappido.Mobile.Core.Services;

namespace aiala.mobile.Activities.Steps
{
    public class StepActivityHandler : ActivityHandler<StepActivityBase>, IActivityHandler<StepStateActivity>
    {
        private readonly IStore<AuthState> _authStore;
        private readonly IStore<ApplicationState> _appStore;
        private readonly IRestClient _restClient;

        public StepActivityHandler(IStore<AuthState> authStore, IStore<ApplicationState> appStore, IRestClient restClient)
        {
            _authStore = authStore;
            _appStore = appStore;
            _restClient = restClient;
        }

        public override async Task<bool> HandleInternal(StepActivityBase activity)
        {
            var url = "v1/"
                .AppendPathSegment("activities")
                .AppendPathSegment("step")
                .AppendPathSegment(activity.StepId);

            if (activity is StepStateActivity)
            {
                url = url.AppendPathSegment("state");
            }
            else
            {
                // Not implemented type
                return false;
            }

            var authState = _authStore.GetState();
            var restResult = await _restClient.PostAsync<StepActivityBase, object>(url.ToString(), activity, authState.AccessTokenType, authState.AccessToken);

            if (restResult.Type == ServiceResultType.AccessDenied || restResult.Type == ServiceResultType.Error)
            {
                _appStore.Dispatch(new ReportActivityFailedAction(ServerMessages.StepActivity_Error));
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
