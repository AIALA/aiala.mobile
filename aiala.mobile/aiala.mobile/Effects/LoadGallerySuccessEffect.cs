using aiala.mobile.Actions;
using aiala.mobile.Storage;
using System.Threading.Tasks;
using xappido.Mobile.Core.Redux;

namespace aiala.mobile.Effects
{
    public class LoadGallerySuccessEffect : Effect<LoadGallerySuccessAction>
    {
        private readonly PictureGalleryDatabase _galleryDatabase;

        public LoadGallerySuccessEffect(PictureGalleryDatabase galleryDatabase, LoadGallerySuccessAction action) : base(action)
        {
            _galleryDatabase = galleryDatabase;
        }

        public override async Task OnExecute(LoadGallerySuccessAction action)
        {
            _galleryDatabase.ReplaceAllItems(action.Result);
        }
    }
}
