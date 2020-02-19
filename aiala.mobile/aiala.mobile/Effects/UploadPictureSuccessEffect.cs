using aiala.mobile.Actions;
using aiala.mobile.Storage;
using System.Threading.Tasks;
using xappido.Mobile.Core.Redux;

namespace aiala.mobile.Effects
{
    public class UploadPictureSuccessEffect : Effect<UploadPictureSuccessAction>
    {
        private readonly PictureGalleryDatabase _galleryDatabase;
        
        public UploadPictureSuccessEffect(PictureGalleryDatabase galleryDatabase, UploadPictureSuccessAction action) : base(action)
        {
            _galleryDatabase = galleryDatabase;
        }

        public override async Task OnExecute(UploadPictureSuccessAction action)
        {
            _galleryDatabase.UpsertItem(action.Picture);
        }
    }
}
