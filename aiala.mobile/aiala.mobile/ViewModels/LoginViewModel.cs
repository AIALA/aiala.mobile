using aiala.mobile.Actions;
using aiala.mobile.Configuration;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Redux;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using xappido.Mobile.Auth;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.State;

namespace aiala.mobile.ViewModels
{
    public class LoginViewModel : xappido.Mobile.Auth.ViewModels.LoginViewModel
    {
        private readonly IStore<ApplicationState> _appStore;
        private readonly IStore<SystemAppState> _systemStore;
        private readonly IApplicationSettingsService _applicationSettings;

        public LoginViewModel(IStore<AuthState> authStore, IStore<ApplicationState> appStore, IStore<SystemAppState> systemStore, IApplicationSettingsService applicationSettings) : base(authStore)
        {
            _appStore = appStore;
            _systemStore = systemStore;

            _applicationSettings = applicationSettings;
            this.RegistrationLink = _applicationSettings.RegistrationLink;
            this.PasswordResetLink = _applicationSettings.PasswordResetLink;

            _systemStore.Subscribe(state =>
            {
                this.HasConnectivity = state.HasConnectivity;
            });
        }

        public bool HasConnectivity
        {
            get
            {
                return _hasConnectivity;
            }
            set
            {
                _hasConnectivity = value;
                RaisePropertyChanged(() => HasConnectivity);
            }
        }
        private bool _hasConnectivity;

        public Command<string> OpenLinkCommand => new Command<string>((uri) => Device.OpenUri(new System.Uri(uri)));

        public Command SettingsCommand => new Command(() => _appStore.Dispatch(new NavigateAction(Models.NavigationState.LoginSettings)));

        public string RegistrationLink
        {
            get
            {
                return _registrationLink;
            }
            set
            {
                _registrationLink = value;
                RaisePropertyChanged(() => RegistrationLink);
            }
        }
        private string _registrationLink;

        public string PasswordResetLink
        {
            get
            {
                return _passwordResetLink;
            }
            set
            {
                _passwordResetLink = value;
                RaisePropertyChanged(() => PasswordResetLink);
            }
        }
        private string _passwordResetLink;

        public override async Task OnNavigateTo(object navigationData)
        {
            await AskForPermissions();

            await base.OnNavigateTo(navigationData);
        }

        private async Task AskForPermissions()
        {
            VersionTracking.Track();

            // Permissions already requested
            var permissionsRequested = Preferences.Get("PermissionRequested", "");
            var currentVersion = VersionTracking.CurrentBuild;

            if (permissionsRequested != currentVersion)
            {
                Preferences.Set("PermissionRequested", currentVersion);

                var permissions = new Permission[] { Permission.Camera, Permission.Storage, Permission.LocationWhenInUse };
                
                var permissionsGranted = await this.AskForPermissions(permissions);

                if (!permissionsGranted)
                {
                    await App.Current.MainPage?.DisplayAlert("Permissions required", "AIALA requires permissions not given yet.", "OK");
                    Microsoft.AppCenter.Analytics.Analytics.TrackEvent("Permissions not granted");
                }
                else
                {
                    Preferences.Set("PermissionRequested", currentVersion);
                }
            }
        }

        private async Task<bool> AskForPermissions(params Permission[] permissions)
        {
            var allGranted = false;

            foreach (var permission in permissions)
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

                if (status != PermissionStatus.Granted)
                {

                    Microsoft.AppCenter.Analytics.Analytics.TrackEvent($"Permissions not granted for {permission}.");

                    allGranted = false;
                    break;
                }
            }

            if (!allGranted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(permissions);

                foreach (var permission in permissions)
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                    if (results.ContainsKey(permission))
                        status = results[permission];

                    if (status != PermissionStatus.Granted && status != PermissionStatus.Restricted)
                    {
                        allGranted = false;
                        break;
                    }
                    else
                    {
                        allGranted = true;
                    }

                }
            }

            return allGranted;
        }
    }
}
