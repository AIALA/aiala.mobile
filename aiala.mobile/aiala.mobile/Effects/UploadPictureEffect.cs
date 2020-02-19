using Flurl;
using aiala.mobile.Actions;
using aiala.mobile.Models;
using aiala.mobile.Resources;
using Redux;
using System.Threading.Tasks;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Models;
using xappido.Mobile.Core.Redux;
using xappido.Mobile.Core.Services;

namespace aiala.mobile.Effects
{
    public class UploadPictureEffect : Effect<UploadPictureAction>
    {
        private readonly IRestClient _restClient;
        private readonly IStore<AuthState> _authStore;
        private readonly IStore<ApplicationState> _appStore;

        public UploadPictureEffect(IStore<AuthState> authStore, IStore<ApplicationState> appStore, IRestClient restClient, UploadPictureAction action) : base(action)
        {
            _restClient = restClient;
            _authStore = authStore;
            _appStore = appStore;
        }

        public override async Task OnExecute(UploadPictureAction action)
        {
            var authState = _authStore.GetState();

            var url = "v1/"
                .AppendPathSegment("pictures")
                .AppendPathSegment("gallery")
                .ToString();

            var restResult = await _restClient.PostAsync<Picture>(url, action.BinaryContent, action.ContentType, action.Reference, authState.AccessTokenType, authState.AccessToken);

            if (restResult.Type == ServiceResultType.AccessDenied)
            {
                _appStore.Dispatch(new UploadPictureFailedAction(ServerMessages.Gallery_UploadPictureError));
                return;
            }

            if (restResult.Type == ServiceResultType.Error)
            {
                _appStore.Dispatch(new UploadPictureFailedAction(ServerMessages.Gallery_UploadPictureError));
                return;
            }

            if (restResult.Type == ServiceResultType.Ok)
            {
                _appStore.Dispatch(new UploadPictureSuccessAction(restResult.Result, action.Reference));
                return;
            }
        }
    }
}
