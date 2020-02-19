using System.Collections.Generic;
using xappido.Mobile.Auth.Models;
using xappido.Mobile.Core.Services;
using xappido.Mobile.Models;

namespace aiala.mobile.Configuration
{
    /// <summary>
    /// Default configuration for developmenet
    /// </summary>
    public class ApplicationSettings : IAuthSettings, ISettings
    {
        public ApplicationSettings()
        {
            var preset = ApplicationSettingsPreset.Local;

            RegistrationLink = preset.PublicWebBaseUrl;
            PasswordResetLink = $"{preset.PublicWebBaseUrl}/password-reset";
            BaseEndpoint = preset.ApiBaseUrl;
            Authority = preset.StsBaseUrl;
            ClientId = preset.ClientId;
            ClientSecret = preset.ClientSecret;

            LogLevel = 0;
            RefreshTimeInterval = 0;
            TouchIdRequired = false;
            Scopes = "openid profile offline_access";
            RedirectUri = "aiala.mobile://callback";
            PostLogoutRedirectUri = "aiala.mobile://callback";

            Culture = string.IsNullOrWhiteSpace(preset.Culture) ? null : preset.Culture;
        }

        public string RegistrationLink { get; }
        public string PasswordResetLink { get; }
        public string BaseEndpoint { get; }
        public int LogLevel { get; }
        public int RefreshTimeInterval { get; }
        public bool TouchIdRequired { get; }
        public string Authority { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string Scopes { get; }
        public string RedirectUri { get; }
        public string PostLogoutRedirectUri { get; }
        public string Culture { get; set; }
    }
}
