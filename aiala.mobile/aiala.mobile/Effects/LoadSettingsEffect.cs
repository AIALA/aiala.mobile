using aiala.mobile.Actions;
using aiala.mobile.Models;
using Redux;
using System.Threading.Tasks;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Models;
using xappido.Mobile.Core.Redux;
using xappido.Mobile.Core.Services;
using Flurl;
using System.Linq;
using aiala.mobile.Resources;

namespace aiala.mobile.Effects
{
    public class LoadSettingsEffect : Effect<LoadSettingsAction>
    {
        private readonly IRestClient _restClient;
        private readonly IStore<AuthState> _authStore;
        private readonly IStore<ApplicationState> _appStore;

        public LoadSettingsEffect(IStore<AuthState> authStore, IStore<ApplicationState> appStore, IRestClient restClient, LoadSettingsAction action) : base(action)
        {
            _restClient = restClient;
            _authStore = authStore;
            _appStore = appStore;
        }

        public override async Task OnExecute(LoadSettingsAction action)
        {
            var authState = _authStore.GetState();

            var url = "v1/"
                .AppendPathSegment("settings")
                .AppendPathSegment("app")
                .ToString();

            var restResult = await _restClient.GetAsync<AppSettings>(url, authState.AccessTokenType, authState.AccessToken);

            if (restResult.Type == ServiceResultType.AccessDenied)
            {
                _authStore.Dispatch(new LoadSettingsFailedAction(ServerMessages.Schedule_LoadError));
                return;
            }

            if (restResult.Type == ServiceResultType.Error)
            {
                _authStore.Dispatch(new LoadSettingsFailedAction(ServerMessages.Schedule_LoadError));
                return;
            }

            if (restResult.Type == ServiceResultType.Ok)
            {
                var emergencyContacts = restResult.Result.EmergencyContacts;
                var emergencyInformation = new EmergencyInformation
                {
                    EmergencyTextBad = restResult.Result.EmergencyTextBad ?? string.Empty,
                    EmergencyTextImproving = restResult.Result.EmergencyTextImproving ?? string.Empty
                };
                var locations = restResult.Result.Places?.Select(s => NavigationLocation.Map(s)).ToList();

                _appStore.Dispatch(new LoadSettingsSuccessAction(emergencyContacts, locations, emergencyInformation));
                return;
            }
        }
    }
}
