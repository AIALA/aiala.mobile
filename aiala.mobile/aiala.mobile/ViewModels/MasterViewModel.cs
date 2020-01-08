using Redux;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.ViewModels;
using System;
using Xamarin.Forms;
using xappido.Mobile.Auth.Actions;
using aiala.mobile.Actions;
using System.Reactive.Linq;
using aiala.mobile.BackgroundServices;
using aiala.mobile.Storage;
using aiala.mobile.Configuration;

namespace aiala.mobile.ViewModels
{
    public class MasterViewModel : ViewModelBase
    {
        protected readonly IStore<ApplicationState> _appStore;
        protected readonly IStore<AuthState> _authStore;
        private readonly PictureGalleryDatabase _galleryDatabase;

        public MasterViewModel(IStore<ApplicationState> appStore, IStore<AuthState> authStore, PictureGalleryDatabase galleryDatabase, IApplicationSettingsService settingsService)
        {
            _appStore = appStore;

            _appStore
                .DistinctUntilChanged(state => new { state.User })
                .Subscribe(state =>
                {
                    this.Fullname = $"{state.User?.Firstname} {state.User?.Lastname}";

                    if (!string.IsNullOrEmpty(state.User?.PictureUrl))
                    {
                        this.ProfileImageUri = new Uri(state.User?.PictureUrl);
                    }
                });

            _appStore
                .DistinctUntilChanged(state => new { state.IsBackgroundProcessorRunning, state.PendingUpdates })
                .Subscribe(state =>
                {
                    this.IsBackgroundProcessing = state.IsBackgroundProcessorRunning;
                    this.PendingUpdates = state.PendingUpdates;
                });

            _authStore = authStore;

            IsDevMode = settingsService.IsDevModeActive;
            _galleryDatabase = galleryDatabase;
        }

        public bool IsDevMode { get; }

        public string Fullname
        {
            get
            {
                return _fullname;
            }
            set
            {
                _fullname = value;
                RaisePropertyChanged(() => Fullname);
            }
        }
        private string _fullname;

        public Uri ProfileImageUri
        {
            get
            {
                return _profileImageUri;
            }
            set
            {
                _profileImageUri = value;
                RaisePropertyChanged(() => ProfileImageUri);
            }
        }
        private Uri _profileImageUri;

        public int PendingUpdates
        {
            get
            {
                return _pendingUpdates;
            }
            set
            {
                _pendingUpdates = value;
                RaisePropertyChanged(() => PendingUpdates);
            }
        }
        private int _pendingUpdates;

        public bool IsBackgroundProcessing
        {
            get
            {
                return _isBackgroundProcessing;
            }
            set
            {
                _isBackgroundProcessing = value;
                RaisePropertyChanged(() => IsBackgroundProcessing);
            }
        }
        private bool _isBackgroundProcessing;

        public Command OpenProfileCommand => new Command(() =>
        {
            return;
            //_appStore.Dispatch(new OpenProfileAction());
        });

        public Command LogoutCommand => new Command(() => _authStore.Dispatch(new LogoutAction(LogoutReason.Manual)));

        public Command SettingsCommand => new Command(() => _appStore.Dispatch(new NavigateAction(Models.NavigationState.Settings)));

        public Command LoadDataCommand => new Command(() =>
        {
            _appStore.Dispatch(new LoadScheduleAction(DateTime.Today, DateTime.Today.AddDays(3)));
            _appStore.Dispatch(new LoadSettingsAction());
            _appStore.Dispatch(new LoadGalleryAction());
        });

        public Command StartProcessingCommand => new Command(() =>
        {
            // start long running task
            var message = new StartTaskRunnerMessage();
            MessagingCenter.Send(message, nameof(StartTaskRunnerMessage));
        });

        public Command StopProcessingCommand => new Command(() =>
        {
            // stop long running task
            var message = new StopTaskRunnerMessage();
            MessagingCenter.Send(message, nameof(StopTaskRunnerMessage));
        });

        public Command CleanPictureLibraryCommand => new Command(() => 
        {
            _galleryDatabase.CleanupDatabase();
            _appStore.Dispatch(new LoadGalleryAction());
        });
    }
}
