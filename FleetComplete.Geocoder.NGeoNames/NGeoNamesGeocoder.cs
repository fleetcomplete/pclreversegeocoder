using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GeoCoordinatePortable;
using NGeoNames;
using NGeoNames.Entities;


namespace FleetComplete.Geocoder.NGeoNames
{
    public class NGeoNamesGeocoder : IGeocoder
    {
        IList<ExtendedGeoName> geodata;
        IList<Admin1Code> adminCodes;


        public async Task<IEnumerable<IGeocoderResult>> FindClosestCitiesAsync(double latitude, double longitude, int take)
        {
            await Task.Run(() => this.EnsureLoaded());
            var center = new GeoCoordinate(latitude, longitude);

            return this.geodata
                .Select(x =>
                {
                    var coord = new GeoCoordinate(x.Latitude, x.Longitude);
                    return new
                    {
                        Geo = x,
                        CityCoord = coord,
                        Distance = center.GetDistanceTo(coord)
                    };
                })
                .OrderBy(x => x.Distance)
                .Take(take)
                .Select(x =>
                {
                    var stateProvince = this.GetStateProvince(x.Geo);
                    var direction = this.CalculateDirection(x.CityCoord, center);

                    return new GeocoderResult
                    {
                        City = x.Geo.NameASCII,
                        Country = CodeMapping.Countries.SafeGet(x.Geo.CountryCode),
                        CountryCode = x.Geo.CountryCode,
                        StateProvince = stateProvince,
                        StateCode = CodeMapping.StateProvinces.SafeGet(stateProvince),
                        Coordinates = new GeoCoordinates(x.CityCoord.Latitude, x.CityCoord.Longitude),
                        DirectionInDegreesFrom = direction,
                        DirectionFrom = GetDirection(direction),
                        ApproxDistance = Distance.FromMeters(x.Distance)
                    };
                });
        }


        double CalculateDirection(GeoCoordinate coordFrom, GeoCoordinate coordTo)
        {
            var dx = coordTo.Latitude - coordFrom.Latitude;
            var dy = coordTo.Longitude - coordFrom.Longitude;
            var result = Math.Atan2(dy, dx) * (180 / Math.PI);
            if (result < 0)
                result = 360 - Math.Abs(result);

            return result;
        }

        //https://gist.github.com/adrianstevens/8163205
        //public static string DegreesToCardinalDetailed(double degrees)
        //        {
        //    string[] caridnals = { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW", "N" };
        //    return caridnals[ (int)Math.Round(((double)degrees*10 % 3600) / 225) ];
        //  }
        static CardinalDirection GetDirection(double degrees)
        {
            var caridnals = new [] { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
            var value = caridnals[(int)Math.Round((degrees % 360) / 45)];
            switch (value)
            {
                case "NW": return CardinalDirection.NorthWest;
                case "W" : return CardinalDirection.West;
                case "SW": return CardinalDirection.SouthWest;
                case "S" : return CardinalDirection.South;
                case "SE": return CardinalDirection.SouthEast;
                case "E" : return CardinalDirection.East;
                case "NE": return CardinalDirection.NorthEast;
                default  :
                case "N" : return CardinalDirection.North;

            }
        }


        string GetStateProvince(ExtendedGeoName result)
        {
            var state = String.Empty;
            var admin = result.Admincodes.FirstOrDefault();

            if (admin != null)
            {
                var code = $"{result.CountryCode}.{admin}";
                var adminCode = this.adminCodes.FirstOrDefault(x => x.Code.Equals(code, StringComparison.CurrentCultureIgnoreCase));
                if (adminCode != null)
                    state = adminCode.NameASCII;
            }
            return state;
        }


        readonly object syncLock = new object();
        void EnsureLoaded()
        {
            if (this.geodata == null)
            {
                lock(this.syncLock)
                {
                    if (this.geodata == null)
                    {
                        using (var stream = this.LoadStream("admin1CodesASCII.txt"))
                        {
                            this.adminCodes = GeoFileReader
                                .ReadAdmin1Codes(stream)
                                .ToList();
                        }
                        using (var stream = this.LoadStream("cities5000.txt"))
                        {
                            this.geodata = GeoFileReader
                                .ReadExtendedGeoNames(stream)
                                .Where(x =>
                                        x.CountryCode.Equals("CA", StringComparison.CurrentCultureIgnoreCase) ||
                                        x.CountryCode.Equals("US", StringComparison.CurrentCultureIgnoreCase)
                                )
                                .OrderBy(x => x.Latitude)
                                .ThenBy(x => x.Longitude)
                                .ToList();
                        }
                    }
                }
            }
        }


        Stream LoadStream(string name)
        {
            return this
                .GetType()
                .GetTypeInfo()
                .Assembly
                .GetManifestResourceStream("FleetComplete.Geocoder.NGeoNames.Resources." + name);
        }
    }
}
