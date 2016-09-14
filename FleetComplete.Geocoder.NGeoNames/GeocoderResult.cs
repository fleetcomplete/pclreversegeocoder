using System;


namespace FleetComplete.Geocoder.NGeoNames
{
    public class GeocoderResult : IGeocoderResult
    {
        public string City { get; set; }
        public string State { get; set; }
        public string CountryCode { get; set; }
        public double DirectionInDegrees { get; set; }
        public CardinalDirection Direction { get; set; }
        public Distance ApproxDistanceTo { get; set; }
        public GeoCoordinates Coordinates { get; set; }
    }
}
