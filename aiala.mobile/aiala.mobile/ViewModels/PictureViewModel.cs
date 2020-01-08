using System.Reactive.Linq;
using System.Threading.Tasks;
using aiala.mobile.Models;
using Redux;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.ViewModels;
using xappido.Mobile.State;
using System;
using Xamarin.Forms;
using aiala.mobile.Actions;
using Plugin.Media;
using aiala.mobile.Activities.Generic;
using System.Collections.Generic;
using aiala.mobile.Activities;
using System.IO;
using System.Linq;
using aiala.mobile.Storage;
using System.Collections.ObjectModel;

namespace aiala.mobile.ViewModels
{
    public class PictureViewModel : ViewModelBase, INavigatableViewModel, ICurrentTaskViewModel
    {
        protected readonly IStore<ApplicationState> _appStore;
        private readonly INavigationService _navigationService;
        private readonly PictureGalleryDatabase _galleryDatabase;

        public PictureViewModel(IStore<ApplicationState> appStore, INavigationService navigationService, PictureGalleryDatabase galleryDatabase)
        {
            _navigationService = navigationService;
            _galleryDatabase = galleryDatabase;

            _appStore = appStore;
            _appStore
               .DistinctUntilChanged(state => new { state.Selected })
               .Subscribe(state =>
               {
                   this.CurrentTask = state.Selected?.Task;
               });

            _appStore
               .DistinctUntilChanged(state => new { state.IsGalleryLoading })
               .Subscribe(state =>
               {
                   this.IsGalleryLoading = state.IsGalleryLoading;
                   if (!this.IsGalleryLoading)
                   {
                       this.LoadPictures();
                   }
               });

            this.IsCameraEnabled = CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;

            this.LoadPictures();
        }

        public DayTask CurrentTask
        {
            get
            {
                return _currentTask;
            }
            set
            {
                _currentTask = value;
                RaisePropertyChanged(() => CurrentTask);
            }
        }
        private DayTask _currentTask;

        public ObservableCollection<Picture> Pictures
        {
            get
            {
                return _pictures;
            }
            set
            {
                _pictures = value;
                RaisePropertyChanged(() => Pictures);
            }
        }
        private ObservableCollection<Picture> _pictures;

        public bool IsGalleryLoading
        {
            get
            {
                return _isGalleryLoading;
            }
            set
            {
                _isGalleryLoading = value;
                RaisePropertyChanged(() => IsGalleryLoading);
            }
        }
        private bool _isGalleryLoading;

        public bool IsCameraEnabled
        {
            get
            {
                return _cameraEnabled;
            }
            set
            {
                _cameraEnabled = value;
                RaisePropertyChanged(() => IsCameraEnabled);
            }
        }
        private bool _cameraEnabled;

        public Task NavigateTo(object navigationParameter)
        {
            this.LoadPictures();
            return Task.CompletedTask;
        }

        public Command EmergencyCommand => new Command(() => _appStore.Dispatch(new NavigateAction(NavigationState.Emergency)));

        public Command PictureTappedCommand => new Command(async (picture) =>
        {
            await _navigationService.NavigateToAsync<PictureDetailViewModel>(picture);
        });

        public Command RefreshCommand => new Command(() => {
            _appStore.Dispatch(new LoadGalleryAction());
        });

        public Command TakePictureCommand => new Command(async () =>
        {
            try
            {
                var opts = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                    SaveToAlbum = false,

                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    CompressionQuality = 90,
                    AllowCropping = true,
                    SaveMetaData = true
                };

                var result = await CrossMedia.Current.TakePhotoAsync(opts);

                // taking photo cancelled
                if (result == null)
                    return;

                // content
                var stream = result.GetStreamWithImageRotatedForExternalStorage();
                byte[] binaryContent;
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    binaryContent = ms.ToArray();
                }

                // content type
                var extension = result.Path.Split('.').LastOrDefault();
                var mimeType = string.Format("image/{0}", extension ?? "*");

                // reference
                var reference = $"{Guid.NewGuid().ToString()}.{extension ?? "jpg"}";

                _appStore.Dispatch(new UploadPictureAction(binaryContent, reference, mimeType));

                await _navigationService.NavigateToAsync<PictureDetailViewModel>(reference);
            }
            catch(Exception ex)
            {
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
            }
        });

        private void LoadPictures()
        {
            var pics = _galleryDatabase.GetItems(18);
            this.Pictures = new ObservableCollection<Picture>(pics);
        }
    }
}
