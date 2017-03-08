using System.Linq;
using System.Text.RegularExpressions;

namespace FleetComplete.Geocoder.CanadaGeoNameBase
{
    public static class StringHelper
    {
        public static string[] SplitCsv(this string input)
        {
            var matches = new Regex("((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))", RegexOptions.None)
                .Matches(input);

            var result = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                result[i] = matches[i].Value;
            }

            return result;
        }
    }
}
