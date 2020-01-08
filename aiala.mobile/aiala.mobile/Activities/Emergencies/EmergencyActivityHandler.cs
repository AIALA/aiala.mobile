using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using aiala.mobile.Actions;
using aiala.mobile.BackgroundServices;
using aiala.mobile.Resources;
using Redux;
using Xamarin.Essentials;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Models;
using xappido.Mobile.Core.Services;

namespace aiala.mobile.Activities.Emergencies
{
    public class EmergencyActivityHandler : ActivityHandler<EmergencyActivityBase>,
        IActivityHandler<StartEmergencyActivity>,
        IActivityHandler<EmergencyMoodActivity>,
        IActivityHandler<EndEmergencyActivity>
    {
        private readonly IStore<AuthState> _authStore;
        private readonly IStore<ApplicationState> _appStore;
        private readonly IRestClient _restClient;

        public EmergencyActivityHandler(IStore<AuthState> authStore, IStore<ApplicationState> appStore, IRestClient restClient)
        {
            _authStore = authStore;
            _appStore = appStore;
            _restClient = restClient;
        }

        public override async Task<bool> HandleInternal(EmergencyActivityBase activity)
        {
            var url = "v1/"
                .AppendPathSegment("activities")
                .AppendPathSegment("emergency")
                .AppendPathSegment(activity.EmergencyId);
            string errorMessage;

            if (activity is StartEmergencyActivity)
            {
                url = url.AppendPathSegment("start");
                errorMessage = ServerMessages.EmergencyActivity_StartError;
            }
            else if (activity is EmergencyMoodActivity)
            {
                url = url.AppendPathSegment("mood");
                errorMessage = ServerMessages.EmergencyActivity_UpdateError;
            }
            else if (activity is EndEmergencyActivity)
            {
                url = url.AppendPathSegment("end");
                errorMessage = ServerMessages.EmergencyActivity_EndError;
            }
            else
            {
                // Not implemented type
                return false;
            }

            var request = new GeolocationRequest(GeolocationAccuracy.Medium);
            var location = await Geolocation.GetLocationAsync(request);
            if (location != null)
            {
                activity.Latitude = location.Latitude;
                activity.Longitude = location.Longitude;
            }

            var authState = _authStore.GetState();
            var restResult = await _restClient.PostAsync<EmergencyActivityBase, object>(url.ToString(), activity, authState.AccessTokenType, authState.AccessToken);

            if (restResult.Type == ServiceResultType.AccessDenied || restResult.Type == ServiceResultType.Error)
            {
                _appStore.Dispatch(new ReportActivityFailedAction(errorMessage));
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
