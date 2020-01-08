using System.Collections.Generic;

namespace aiala.mobile.Models
{
    public class AppSettings
    {
        public List<EmergencyContact> EmergencyContacts { get; set; }

        public string EmergencyTextBad { get; set; }

        public string EmergencyTextImproving { get; set; }

        public List<LocationSetting> Places { get; set; }
    }
}
