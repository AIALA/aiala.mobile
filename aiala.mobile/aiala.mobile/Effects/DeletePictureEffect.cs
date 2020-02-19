using Flurl;
using aiala.mobile.Actions;
using aiala.mobile.Models;
using aiala.mobile.Resources;
using aiala.mobile.Storage;
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
    public class DeletePictureEffect : Effect<DeletePictureAction>
    {
        private readonly IStore<AuthState> _authStore;
        private readonly IStore<ApplicationState> _appStore;
        private readonly IRestClient _restClient;
        private readonly PictureGalleryDatabase _galleryDatabase;

        public DeletePictureEffect(
            IStore<AuthState> authStore,
            IStore<ApplicationState> appStore,
            IRestClient restClient,
            PictureGalleryDatabase galleryDatabase,
            DeletePictureAction action) : base(action)
        {
            _authStore = authStore;
            _appStore = appStore;
            _restClient = restClient;
            _galleryDatabase = galleryDatabase;
        }

        public override async Task OnExecute(DeletePictureAction action)
        {
            var authState = _authStore.GetState();

            var url = "v1/"
                .AppendPathSegment("pictures")
                .AppendPathSegment(action.PictureId)
                .ToString();

            var result = await _restClient.DeleteAsync<object>(url, authState.AccessTokenType, authState.AccessToken);

            if (result.Type == ServiceResultType.Ok)
            {
                _galleryDatabase.DeletePicture(action.PictureId);
            }

            _appStore.Dispatch(new NavigateAction(NavigationState.Picture));
        }
    }
}
