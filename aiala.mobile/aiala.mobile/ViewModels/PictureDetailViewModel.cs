using System.Reactive.Linq;
using System.Threading.Tasks;
using aiala.mobile.Models;
using Redux;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.ViewModels;
using System;
using Xamarin.Forms;
using aiala.mobile.Actions;
using System.Linq;
using System.Collections.Generic;
using aiala.mobile.Storage;
using System.Collections.ObjectModel;

namespace aiala.mobile.ViewModels
{
    public class PictureDetailViewModel : ViewModelBase, INavigatableViewModel, ICurrentTaskViewModel
    {
        protected readonly IStore<ApplicationState> _appStore;
        private readonly PictureGalleryDatabase _pictureGalleryDatabase;
        private string _pictureReference;

        public PictureDetailViewModel(IStore<ApplicationState> appStore, PictureGalleryDatabase pictureGalleryDatabase)
        {
            _pictureGalleryDatabase = pictureGalleryDatabase;

            _appStore = appStore;
            
            _appStore
               .DistinctUntilChanged(state => new { state.Selected })
               .Subscribe(state =>
               {
                   this.CurrentTask = state.Selected?.Task;
               });

            _appStore
                .DistinctUntilChanged(state => new { state.IsPictureUploading, state.LatestPictureUploadReference })
                .Subscribe(state =>
                {
                    this.IsUploading = state.IsPictureUploading;

                    if(state.LatestPictureUploadReference.Equals(_pictureReference))
                    {
                        this.UpdatePicture(state.LatestPictureUpload);
                    }
                });
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

        public bool IsUploading
        {
            get
            {
                return _isUploading;
            }
            set
            {
                _isUploading = value;
                RaisePropertyChanged(() => IsUploading);
            }
        }
        private bool _isUploading;

        public Picture CurrentPicture
        {
            get
            {
                return _currentPicture;
            }
            set
            {
                _currentPicture = value;
                RaisePropertyChanged(() => CurrentPicture);
            }
        }
        private Picture _currentPicture;

        public ObservableCollection<Picture> Related
        {
            get
            {
                return _related;
            }
            set
            {
                _related = value;
                RaisePropertyChanged(() => Related);
            }
        }
        private ObservableCollection<Picture> _related;

        public string Tags
        {
            get
            {
                return _tags;
            }
            set
            {
                _tags = value;
                RaisePropertyChanged(() => Tags);
            }
        }
        private string _tags;

        public Task NavigateTo(object navigationParameter)
        {
            if(navigationParameter is string reference)
            {
                _pictureReference = reference;
            }

            if(navigationParameter is Picture picture)
            {
                this.UpdatePicture(picture);
            }

            return Task.CompletedTask;
        }

        public Command EmergencyCommand => new Command(() => _appStore.Dispatch(new NavigateAction(NavigationState.Emergency)));

        public Command BackCommand => new Command(() => _appStore.Dispatch(new NavigateAction(NavigationState.Picture)));

        public Command DeletePictureCommand => new Command(() => _appStore.Dispatch(new DeletePictureAction(CurrentPicture.Id)));

        private void UpdatePicture(Picture picture)
        {
            this.CurrentPicture = picture;
            this.Tags = this.CurrentPicture.GetConfidentTags(3);

            var related = _pictureGalleryDatabase.GetRelatedItems(picture);
            this.Related = related.Any() ? new ObservableCollection<Picture>(related) : null;
        }
    }
}
