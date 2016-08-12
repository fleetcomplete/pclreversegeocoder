using System;


namespace LocationWarning.Services
{
    public class GeocodeResult
    {
        public Geolocation Geo { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public long TimeTakenInMilliseconds { get; set; }
    }
}
