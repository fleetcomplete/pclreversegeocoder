using System;
using System.Collections.Generic;


namespace FleetComplete.Geocoder.NGeoNames
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


        internal static IDictionary<string, string> Countries { get; } = new Dictionary<string, string>
        {
            { "CA", "Canada" },
            { "US", "United States" }
        };


        internal static IDictionary<string, string> StateProvinces { get; } = new Dictionary<string, string>
        {
            // United States
            {"Alabama", "AL" },
            {"Alaska", "AK" },
            {"American Samoa", "AS" },
            {"Arizona", "AZ" },
            {"Arkansas", "AR" },
            {"California", "CA" },
            {"Colorado", "CO" },
            {"Connecticut", "CT" },
            {"Delaware", "DE" },
            {"District of Columbia", "DC" },
            {"Florida", "FL" },
            {"Georgia", "GA" },
            {"Guam", "GU" },
            {"Hawaii", "HI" },
            {"Idaho", "ID" },
            {"Illinois", "IL" },
            {"Indiana", "IN" },
            {"Iowa", "IA" },
            {"Kansas", "KS" },
            {"Kentucky", "KY" },
            {"Louisiana", "LA" },
            {"Maine", "ME" },
            {"Maryland", "MD" },
            {"Marshall Islands", "MH" },
            {"Massachusetts", "MA" },
            {"Michigan", "MI" },
            {"Micronesia", "FM" },
            {"Minnesota", "MN" },
            {"Mississippi", "MS" },
            {"Missouri", "MO" },
            {"Montana", "MT" },
            {"Nebraska", "NE" },
            {"Nevada", "NV" },
            {"New Hampshire", "NH" },
            {"New Jersey", "NJ" },
            {"New Mexico", "NM" },
            {"New York", "NY" },
            {"North Carolina", "NC" },
            {"North Dakota", "ND" },
            {"Northern Marianas", "MP" },
            {"Ohio", "OH" },
            {"Oklahoma", "OK" },
            {"Oregon", "OR" },
            {"Palau", "PW" },
            {"Pennsylvania", "PA" },
            {"Puerto Rico", "PR" },
            {"Rhode Island", "RI" },
            {"South Carolina", "SC" },
            {"South Dakota", "SD" },
            {"Tennessee", "TN" },
            {"Texa", "TX" },
            {"Utah", "UT" },
            {"Vermont", "VT" },
            {"Virginia", "VA" },
            {"Virgin Islands", "VI" },
            {"Washington", "WA" },
            {"West Virginia", "WV" },
            {"Wisconsin", "WI" },
            {"Wyoming", "WY" },

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

