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
    public class GeocoderImpl : IGeocoder
    {
        IList<ExtendedGeoName> geodata;
        IList<Admin1Code> adminCodes;


        public async Task<IEnumerable<IGeocoderResult>> FindClosestCities(double latitude, double longitude, int take)
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

                    var stateCode = this.GetState(x.Geo);
                    var direction = this.CalculateDirection(center, x.CityCoord);

                    return new GeocoderResult
                    {
                        City = x.Geo.NameASCII,
                        CountryCode = x.Geo.CountryCode,
                        StateCode = stateCode,
                        Direction = direction,
                        ApproxDistanceTo = Distance.FromMeters(x.Distance)
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


        string GetState(ExtendedGeoName result)
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
                                    //x.FeatureCode.Equals("PPL", StringComparison.OrdinalIgnoreCase) &&
                                    //(
                                        x.CountryCode.Equals("CA", StringComparison.CurrentCultureIgnoreCase) ||
                                        x.CountryCode.Equals("US", StringComparison.CurrentCultureIgnoreCase)
                                    //)
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
