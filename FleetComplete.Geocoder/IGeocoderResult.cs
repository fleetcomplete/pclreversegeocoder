using System;


namespace FleetComplete.Geocoder
{
    public interface IGeocoderResult
    {
        string City { get; }
        string StateCode { get; }
        string CountryCode { get; }

        // TODO: TimeZoneInfo TimeZone { get; }
        // TODO: classification of place (FeatureClass/Code from ngeonames)
        double Direction { get; }
        Distance ApproxDistanceTo { get; }
    }
}
