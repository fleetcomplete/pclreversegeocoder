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
                    var direction = Direction.CalculateDirection(x.CityCoord, center);

                    return new GeocoderResult
                    {
                        City = x.Geo.NameASCII,
                        Country = CodeMapping.Countries.SafeGet(x.Geo.CountryCode),
                        CountryCode = x.Geo.CountryCode,
                        StateProvince = stateProvince,
                        StateProvinceCode = CodeMapping.StateProvinces.SafeGet(stateProvince),
                        Coordinates = new GeoCoordinates(x.CityCoord.Latitude, x.CityCoord.Longitude),
                        DirectionInDegreesFrom = direction,
                        DirectionFrom = Direction.GetDirection(direction),
                        ApproxDistance = Distance.FromMeters(x.Distance)
                    };
                });
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
