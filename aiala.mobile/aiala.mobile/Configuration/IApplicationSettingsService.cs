using xappido.Mobile.Auth.Services;
using xappido.Mobile.Core.Services;

namespace aiala.mobile.Configuration
{
    public interface IApplicationSettingsService : ISettingsService, IAuthSettingsService
    {
        string Culture { get; set; }

        bool IsDevModeActive { get; set; }

        string LocalIP { get; set; }

        string PresetId { get; set; }

        string RegistrationLink { get; set; }

        string PasswordResetLink { get; set; }

        int ScheduleRefreshMinutes { get; set; }
    }
}
