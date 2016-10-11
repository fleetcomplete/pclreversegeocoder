using System;


namespace FleetComplete.Geocoder.NGeoNames
{
    public class GeocoderResult : IGeocoderResult
    {
        public string City { get; set; }
        public string StateCode { get; set; } // new
        public string State { get; set; }
        public string Country { get; set; } // new
        public string CountryCode { get; set; }
        public double DirectionInDegreesFrom { get; set; }
        public CardinalDirection DirectionFrom { get; set; }
        public Distance ApproxDistance { get; set; }
        public GeoCoordinates Coordinates { get; set; }
    }
}
