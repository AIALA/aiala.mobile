using System.Reactive.Linq;
using System.Threading.Tasks;
using aiala.mobile.Models;
using Redux;
using xappido.Mobile.Core.Navigation;
using xappido.Mobile.Core.ViewModels;
using System;
using Xamarin.Forms;
using aiala.mobile.Actions;
using System.Collections.Generic;
using aiala.mobile.Resources;
using aiala.mobile.Configuration;
using System.Linq;
using System.Globalization;
using System.Threading;

namespace aiala.mobile.ViewModels
{
    public class SettingsViewModel : ViewModelBase, INavigatableViewModel, ICurrentTaskViewModel
    {
        protected readonly IStore<ApplicationState> _appStore;
        private readonly IApplicationSettingsService _settingsService;
        private readonly ApplicationNavigationPageConnector _pageConnector;
        public string _stsBaseUrl;
        public string _apiBaseUrl;
        public string _publicWebBaseUrl;
        public string _clientId;
        public string _clientSecret;
        public string _localIP;

        public SettingsViewModel(
            IStore<ApplicationState> appStore,
            IApplicationSettingsService settingsService,
            INavigationPageConnector pageConnector)
        {
            _appStore = appStore;
            _settingsService = settingsService;
            _pageConnector = pageConnector as ApplicationNavigationPageConnector;
            var culture = _settingsService.Culture == null ? Thread.CurrentThread.CurrentCulture : new CultureInfo(_settingsService.Culture);
            var availableLanguage = AvailableLanguages.FirstOrDefault(c => c.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName);
            _language = availableLanguage ?? AvailableLanguages[0];

            _isDevModeActive = _settingsService.IsDevModeActive;
            _localIP = _settingsService.LocalIP;
            _scheduleRefreshMinutes = _settingsService.ScheduleRefreshMinutes;

            var presetId = _settingsService.PresetId;
            SelectedPreset = ApplicationSettingsPreset.All.FirstOrDefault(p => p.Id == presetId);
            if (SelectedPreset == null)
            {
                _stsBaseUrl = _settingsService.Authority;
                _apiBaseUrl = _settingsService.BaseEndpoint;
                _publicWebBaseUrl = _settingsService.RegistrationLink;
                _clientId = _settingsService.ClientId;
                _clientSecret = _settingsService.ClientSecret;
            }

            _appStore
               .DistinctUntilChanged(state => new { state.Selected, state.EmergencyContacts })
               .Subscribe(state =>
               {
                   this.CurrentTask = state.Selected?.Task;
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

        public IList<AvailableLanguage> AvailableLanguages { get; } = new List<AvailableLanguage>
        {
            new AvailableLanguage(new CultureInfo("en").NativeName, "en"),
            new AvailableLanguage(new CultureInfo("de").NativeName, "de"),
            new AvailableLanguage(new CultureInfo("fr").NativeName, "fr"),
            new AvailableLanguage(new CultureInfo("it").NativeName, "it"),
            new AvailableLanguage(new CultureInfo("es").NativeName, "es")
        };

        public AvailableLanguage Language
        {
            get
            {
                return _language;
            }
            set
            {
                if (_language != value)
                {
                    _language = value;
                    _settingsService.Culture = _language.TwoLetterISOLanguageName;
                    _pageConnector.ResetMaster();
                    _appStore.Dispatch(new NavigateAction(_targetNavigationState));
                    RaisePropertyChanged(() => Language);
                }
            }
        }
        private AvailableLanguage _language;

        public bool IsDevModeActive
        {
            get
            {
                return _isDevModeActive;
            }
            set
            {
                _isDevModeActive = value;
                _settingsService.IsDevModeActive = value;
                _pageConnector.ResetMaster();
                RaisePropertyChanged(() => IsDevModeActive);
                RaisePropertyChanged(() => ToggleDevModeText);
            }
        }
        private bool _isDevModeActive = false;

        public string ToggleDevModeText
        {
            get
            {
                return IsDevModeActive ? UiTexts.Settings_DisableDevMode : UiTexts.Settings_EnableDevMode;
            }
        }

        public IList<ApplicationSettingsPreset> Presets
        {
            get
            {
                return ApplicationSettingsPreset.All;
            }
        }

        public ApplicationSettingsPreset SelectedPreset
        {
            get
            {
                return _selectedPreset;
            }
            set
            {
                if (value == null
                    && SelectedPreset != null
                    && StsBaseUrl == SelectedPreset.StsBaseUrl
                    && ApiBaseUrl == SelectedPreset.ApiBaseUrl
                    && PublicWebBaseUrl == SelectedPreset.PublicWebBaseUrl
                    && ClientId == SelectedPreset.ClientId
                    && ClientSecret == SelectedPreset.ClientSecret)
                {
                    return;
                }

                if (value != null)
                {
                    _stsBaseUrl = value.StsBaseUrl;
                    _apiBaseUrl = value.ApiBaseUrl;
                    _publicWebBaseUrl = value.PublicWebBaseUrl;
                    _clientId = value.ClientId;
                    _clientSecret = value.ClientSecret;
                    RaisePropertyChanged(() => StsBaseUrl);
                    RaisePropertyChanged(() => ApiBaseUrl);
                    RaisePropertyChanged(() => PublicWebBaseUrl);
                    RaisePropertyChanged(() => ClientId);
                    RaisePropertyChanged(() => ClientSecret);
                }

                _settingsService.PresetId = value?.Id;
                _selectedPreset = value;
                RaisePropertyChanged(() => SelectedPreset);
            }
        }
        private ApplicationSettingsPreset _selectedPreset;

        public string StsBaseUrl
        {
            get
            {
                return _stsBaseUrl;
            }
            set
            {

                _stsBaseUrl = value;
                _settingsService.Authority = _stsBaseUrl?.Replace(ApplicationSettingsPreset.LocalIPPlaceholder, LocalIP);
                SelectedPreset = null;
                RaisePropertyChanged(() => StsBaseUrl);
            }
        }

        public string ApiBaseUrl
        {
            get
            {
                return _apiBaseUrl;
            }
            set
            {
                _apiBaseUrl = value;
                _settingsService.BaseEndpoint = _apiBaseUrl?.Replace(ApplicationSettingsPreset.LocalIPPlaceholder, LocalIP);
                SelectedPreset = null;
                RaisePropertyChanged(() => ApiBaseUrl);
            }
        }

        public string PublicWebBaseUrl
        {
            get
            {
                return _publicWebBaseUrl;
            }
            set
            {
                _publicWebBaseUrl = value;
                var baseUrl = _publicWebBaseUrl?.Replace(ApplicationSettingsPreset.LocalIPPlaceholder, LocalIP);
                _settingsService.RegistrationLink = baseUrl;
                _settingsService.PasswordResetLink = $"{baseUrl}/password-reset";
                SelectedPreset = null;
                RaisePropertyChanged(() => PublicWebBaseUrl);
            }
        }

        public string ClientId
        {
            get
            {
                return _clientId;
            }
            set
            {
                _settingsService.ClientId = _clientId = value;
                SelectedPreset = null;
                RaisePropertyChanged(() => ClientId);
            }
        }

        public string ClientSecret
        {
            get
            {
                return _clientSecret;
            }
            set
            {
                _settingsService.ClientSecret = _clientSecret = value;
                SelectedPreset = null;
                RaisePropertyChanged(() => ClientSecret);
            }
        }

        public string LocalIP
        {
            get
            {
                return _localIP;
            }
            set
            {
                _settingsService.LocalIP = _localIP = value;

                StsBaseUrl = StsBaseUrl;
                ApiBaseUrl = ApiBaseUrl;
                PublicWebBaseUrl = PublicWebBaseUrl;

                RaisePropertyChanged(() => LocalIP);
            }
        }

        public int ScheduleRefreshMinutes
        {
            get
            {
                return _scheduleRefreshMinutes;
            }
            set
            {
                _settingsService.ScheduleRefreshMinutes = _scheduleRefreshMinutes = value;

                RaisePropertyChanged(() => ScheduleRefreshMinutes);
            }
        }
        private int _scheduleRefreshMinutes;

        public Command CancelCommand => new Command(() => _appStore.Dispatch(new NavigateAction(_targetNavigationState)));
        private NavigationState _targetNavigationState = NavigationState.Home;

        public Command ToggleDevModeCommand => new Command(() => IsDevModeActive = !IsDevModeActive);

        public Task NavigateTo(object navigationParameter)
        {
            if (navigationParameter is bool navigateToLogin && navigateToLogin)
            {
                _targetNavigationState = NavigationState.Login;
            }

            return Task.CompletedTask;
        }

        public class AvailableLanguage
        {
            public AvailableLanguage(string nativeName, string twoLetterISOLanguageName)
            {
                NativeName = nativeName;
                TwoLetterISOLanguageName = twoLetterISOLanguageName;
            }

            public string NativeName { get; }
            public string TwoLetterISOLanguageName { get; }
        }
    }
}
