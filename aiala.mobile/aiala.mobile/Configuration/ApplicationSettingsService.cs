using xappido.Mobile.Auth.Models;
using xappido.Mobile.Core.Services;
using xappido.Mobile.Services;
using Xamarin.Essentials;
using xappido.Mobile.Auth.Services;
using System.Linq;
using System.Threading;
using System.Globalization;
using System;

namespace aiala.mobile.Configuration
{
    public class ApplicationSettingsService : AuthSettingsServiceBase<ApplicationSettings>, IApplicationSettingsService
    {
        public override ApplicationSettings Defaults { get; } = new ApplicationSettings();

        public string Culture
        {
            get => Preferences.Get(nameof(Culture), Defaults.Culture);
            set
            {
                if (value != null)
                {
                    var cultureInfo = new CultureInfo(value);
                    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
                }
                Preferences.Set(nameof(Culture), value);
            }
        }

        public string PresetId
        {
            get => Preferences.Get(nameof(PresetId), null);
            set => Preferences.Set(nameof(PresetId), value);
        }

        public string LocalIP
        {
            get => Preferences.Get(nameof(LocalIP), "_._._._");
            set => Preferences.Set(nameof(LocalIP), value);
        }

        public bool IsDevModeActive
        {
            get
            {
                try
                {
                    return Preferences.Get(nameof(IsDevModeActive), false);
                }
                catch
                {
                    var value = Convert.ToBoolean(Preferences.Get(nameof(IsDevModeActive), null));
                    IsDevModeActive = value;
                    return value;
                }
            }

            set => Preferences.Set(nameof(IsDevModeActive), value);
        }

        public string RegistrationLink
        {
            get => Preferences.Get(nameof(RegistrationLink), Defaults.RegistrationLink);
            set => Preferences.Set(nameof(RegistrationLink), value);
        }

        public string PasswordResetLink
        {
            get => Preferences.Get(nameof(PasswordResetLink), Defaults.PasswordResetLink);
            set => Preferences.Set(nameof(PasswordResetLink), value);
        }

        public int ScheduleRefreshMinutes
        {
            get => Preferences.Get(nameof(ScheduleRefreshMinutes), 60);
            set => Preferences.Set(nameof(ScheduleRefreshMinutes), Math.Min(value, 1));
        }
    }
}
