using System;

namespace FleetComplete.Geocoder.CanadaGeoNameBase
{
    public static class StringHelper
    {
        public static string RemoveFromEnd(this string s, string suffix)
        {
            if (s.EndsWith(suffix))
            {
                return s.Substring(0, s.Length - suffix.Length);
            }
            else
            {
                return s;
            }
        }
    }
}
