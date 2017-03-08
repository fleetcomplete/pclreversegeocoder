using System;
using System.Collections.Generic;

namespace FleetComplete.Geocoder.CanadaGeoNameBase
{
    public static class CodeMapping
    {
        public static TValue SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            var value = dictionary.ContainsKey(key)
                ? dictionary[key]
                : default(TValue);

            return value;
        }

        internal static IDictionary<string, string> Provinces { get; } = new Dictionary<string, string>
        {
            // Canada
            { "Alberta", "AB" },
            { "British Columbia", "BC" },
            { "Manitoba", "MB" },
            { "New Brunswick", "NB" },
            { "Newfoundland", "NF" },
            { "Nova Scotia", "NS" },
            { "North West Territories", "NT" },
            { "Ontario", "ON" },
            { "Prince Edward Island", "PE" },
            { "Quebec", "QC" },
            { "Saskatchewan", "SK" },
            { "Yukon Territory", "YT" }
        };
    }
}
