using System;


namespace FleetComplete.Geocoder.NGeoNames
{
    public class GeocoderResult : IGeocoderResult
    {
        public string City { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }
        public double Direction { get; set; }
        public Distance ApproxDistanceTo { get; set; }
    }
}
