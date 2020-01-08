using Newtonsoft.Json;

namespace aiala.mobile.Models
{
    public class LocationSetting
    {
        public string Name { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public Picture Picture { get; set; }
    }
}
