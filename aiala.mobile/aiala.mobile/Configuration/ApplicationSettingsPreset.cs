using System;
using System.Collections.Generic;
using System.Text;

namespace aiala.mobile.Configuration
{
    public class ApplicationSettingsPreset
    {
        public const string LocalIPPlaceholder = "%IP%";

        public static ApplicationSettingsPreset Dev { get; } = new ApplicationSettingsPreset
        {
            Id = "Dev",
            Name = "Dev",
            StsBaseUrl = "https://aiala-sts-dev.azurewebsites.net",
            ApiBaseUrl = "https://aiala-api-dev.azurewebsites.net/api",
            PublicWebBaseUrl = "https://aiala-app-dev.azurewebsites.net/public/en-us",
            ClientId = "aiala.mobile",
            ClientSecret = "xxx",
            Culture = null
        };
                
        public static ApplicationSettingsPreset Local { get; } = new ApplicationSettingsPreset
        {
            Id = "Local",
            Name = "Local",
            StsBaseUrl = $"http://{LocalIPPlaceholder}:5000",
            ApiBaseUrl = $"http://{LocalIPPlaceholder}:5500/api",
            PublicWebBaseUrl = $"http://{LocalIPPlaceholder}:4567",
            ClientId = "aiala.mobile",
            ClientSecret = "xxx",
            Culture = null
        };

        public static List<ApplicationSettingsPreset> All { get; } = new List<ApplicationSettingsPreset>
        {
            Dev,
            Local
        };

        public string Id { get; set; }

        public string Name { get; set; }

        public string StsBaseUrl { get; set; }

        public string ApiBaseUrl { get; set; }

        public string PublicWebBaseUrl { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Culture { get; set; }

        public override string ToString() => this.Name;
    }
}
