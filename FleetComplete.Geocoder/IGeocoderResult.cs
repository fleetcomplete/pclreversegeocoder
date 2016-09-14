using System;


namespace FleetComplete.Geocoder
{
    public interface IGeocoderResult
    {
        string City { get; }
        string State { get; }
        string CountryCode { get; }
        CardinalDirection Direction { get; }

        // TODO: TimeZoneInfo TimeZone { get; }
        // TODO: classification of place (FeatureClass/Code from ngeonames)
        double DirectionInDegrees { get; }
        Distance ApproxDistanceTo { get; }
        GeoCoordinates Coordinates { get; }
    }
}
