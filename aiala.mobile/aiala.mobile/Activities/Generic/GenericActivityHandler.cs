using Flurl;
using aiala.mobile.Actions;
using aiala.mobile.BackgroundServices;
using Redux;
using System.Threading.Tasks;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Services;
using aiala.mobile.Activities;
using xappido.Mobile.Core.Models;
using aiala.mobile.Resources;

namespace aiala.mobile.Activities.Generic
{
    public class GenericActivityHandler : ActivityHandler<GenericActivity>
    {
        private readonly IStore<AuthState> _authStore;
        private readonly IStore<ApplicationState> _appStore;
        private readonly IRestClient _restClient;

        public GenericActivityHandler(IStore<AuthState> authStore, IStore<ApplicationState> appStore, IRestClient restClient)
        {
            _authStore = authStore;
            _appStore = appStore;
            _restClient = restClient;
        }

        public override async Task<bool> HandleInternal(GenericActivity activity)
        {
            var url = "v1/"
                .AppendPathSegment("activities")
                .AppendPathSegment("general")
                .AppendPathSegment(activity.ActivityType.ToString())
                .ToString();

            var authState = _authStore.GetState();
            var restResult = await _restClient.PostAsync<GenericActivity, object>(url, activity, authState.AccessTokenType, authState.AccessToken);

            if (restResult.Type == ServiceResultType.AccessDenied)
            {
                _appStore.Dispatch(new ReportActivityFailedAction(ServerMessages.GenericActivity_Error));
                return false;
            }

            if (restResult.Type == ServiceResultType.Error)
            {
                _appStore.Dispatch(new ReportActivityFailedAction(ServerMessages.GenericActivity_Error));
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
