namespace aiala.mobile.Models
{

    public class NavigationLocation
    {
        public string Name { get; set; }

        public Picture Picture { get; set; }

        public Geolocation Location { get; set; }

        public class Geolocation
        {
            public double Latitude { get; set; }

            public double Longitude { get; set; }
        }

        public static NavigationLocation Map(LocationSetting setting)
        {
            Geolocation location = null;

            if (setting.Longitude.HasValue && setting.Latitude.HasValue)
                location = new Geolocation { Latitude = setting.Latitude.Value, Longitude = setting.Longitude.Value };

            return new NavigationLocation
            {
                Name = setting.Name,
                Picture = setting.Picture,
                Location = location
            };
        }

        public static NavigationLocation Map(Place place)
        {
            Geolocation location = null;

            if (place.Longitude.HasValue && place.Latitude.HasValue)
                location = new Geolocation { Latitude = place.Latitude.Value, Longitude = place.Longitude.Value };

            return new NavigationLocation
            {
                Name = place.Name,
                Picture = place.Picture,
                Location = location
            };
        }
    }
}
