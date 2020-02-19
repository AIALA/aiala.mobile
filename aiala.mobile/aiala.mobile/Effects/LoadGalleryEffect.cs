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
using aiala.mobile.Resources;

namespace aiala.mobile.Effects
{
    public class LoadGalleryEffect : Effect<LoadGalleryAction>
    {
        private readonly IRestClient _restClient;
        private readonly IStore<AuthState> _authStore;
        private readonly IStore<ApplicationState> _appStore;

        public LoadGalleryEffect(IStore<AuthState> authStore, IStore<ApplicationState> appStore, IRestClient restClient, LoadGalleryAction action) : base(action)
        {
            _restClient = restClient;
            _authStore = authStore;
            _appStore = appStore;
        }

        public override async Task OnExecute(LoadGalleryAction action)
        {
            var authState = _authStore.GetState();

            var url = "v1/"
                .AppendPathSegment("pictures")
                .AppendPathSegment("gallery")
                .ToString();

            var restResult = await _restClient.GetAsync<List<Picture>>(url, authState.AccessTokenType, authState.AccessToken);

            if (restResult.Type == ServiceResultType.AccessDenied)
            {
                _appStore.Dispatch(new LoadGalleryFailedAction(ServerMessages.Gallery_LoadError));
                return;
            }

            if (restResult.Type == ServiceResultType.Error)
            {
                _appStore.Dispatch(new LoadGalleryFailedAction(ServerMessages.Gallery_LoadError));
                return;
            }

            if (restResult.Type == ServiceResultType.Ok)
            {
                _appStore.Dispatch(new LoadGallerySuccessAction(restResult.Result));
                return;
            }
        }
    }
}
