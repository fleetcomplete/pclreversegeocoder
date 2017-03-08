using System;


namespace FleetComplete.Geocoder.NGeoNames
{
    public class GeocoderResult : IGeocoderResult
    {
        public string City { get; set; }
        public string StateCode { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public double DirectionInDegreesFrom { get; set; }
        public CardinalDirection DirectionFrom { get; set; }
        public Distance ApproxDistance { get; set; }
        public GeoCoordinates Coordinates { get; set; }
    }
}
