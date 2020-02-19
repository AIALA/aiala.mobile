using aiala.mobile.Actions;
using aiala.mobile.Models;
using aiala.mobile.Resources;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Models;
using xappido.Mobile.Core.Redux;
using xappido.Mobile.Core.Services;

namespace aiala.mobile.Effects
{
    public class LoadUserInfoEffect : Effect<LoadUserInfoAction>
    {
        private readonly IRestClient _restClient;
        private readonly IStore<AuthState> _authStore;
        private readonly IStore<ApplicationState> _appStore;

        public LoadUserInfoEffect(IStore<AuthState> authStore, IStore<ApplicationState> appStore, IRestClient restClient, LoadUserInfoAction action) : base(action)
        {
            _restClient = restClient;
            _authStore = authStore;
            _appStore = appStore;
        }

        public override async Task OnExecute(LoadUserInfoAction action)
        {
            var authState = _authStore.GetState();

            var restResult = await _restClient.GetAsync<User>("v1/profile", authState.AccessTokenType, authState.AccessToken);

            if (restResult.Type == ServiceResultType.AccessDenied)
            {
                _authStore.Dispatch(new LoadUserInfoFailedAction(ServerMessages.Directory_LoadUserInfoError));
                return;
            }

            if (restResult.Type == ServiceResultType.Error)
            {
                _authStore.Dispatch(new LoadUserInfoFailedAction(ServerMessages.Directory_LoadUserInfoError));
                return;
            }

            if (restResult.Type == ServiceResultType.Ok)
            {
                _appStore.Dispatch(new LoadUserInfoSuccessAction(restResult.Result));
                return;
            }
        }
    }
}
