using System;


namespace FleetComplete.Geocoder
{
    public interface IGeocoderResult
    {
        string City { get; }
        string State { get; }
        string StateCode { get; }
        string Country { get; }
        string CountryCode { get; }
        CardinalDirection DirectionFrom { get; }

        // TODO: TimeZoneInfo TimeZone { get; }
        // TODO: classification of place (FeatureClass/Code from ngeonames)
        double DirectionInDegreesFrom { get; }
        Distance ApproxDistance { get; }
        GeoCoordinates Coordinates { get; }
    }
}
