using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GeoCoordinatePortable;

namespace FleetComplete.Geocoder.CanadaGeoNameBase
{
    public class CanadaGeoNameBaseGeocoder : IGeocoder
    {
        private IList<GeoNameEntry> geodata;

        public async Task<IEnumerable<IGeocoderResult>> FindClosestCitiesAsync(double latitude, double longitude, int take = 5)
        {
            await Task.Run(() => this.EnsureLoaded());
            var center = new GeoCoordinate(latitude, longitude);

            return this.geodata
                .Select(x =>
                {
                    var coord = new GeoCoordinate(x.Latitude, x.Longitude);
                    return new
                    {
                        GeoNameEntry = x,
                        Coordinate = coord,
                        Distance = center.GetDistanceTo(coord)
                    };
                })
                .OrderBy(x => x.Distance)
                .Take(take)
                .Select(x =>
                {
                    var direction = Direction.CalculateDirection(x.Coordinate, center);

                    return new GeocoderResult
                    {
                        City = x.GeoNameEntry.GeographicalName,
                        Country = "Canada",
                        CountryCode = "CA",
                        StateProvince = x.GeoNameEntry.ProvinceTerritory,
                        StateProvinceCode = CodeMapping.Provinces.SafeGet(x.GeoNameEntry.ProvinceTerritory),
                        Coordinates = new GeoCoordinates(x.Coordinate.Latitude, x.Coordinate.Longitude),
                        DirectionInDegreesFrom = direction,
                        DirectionFrom = Direction.GetDirection(direction),
                        ApproxDistance = Distance.FromMeters(x.Distance)
                    };
                });
        }

        readonly object syncLock = new object();
        private void EnsureLoaded()
        {
            if (this.geodata == null)
            {
                lock (this.syncLock)
                {
                    if (this.geodata == null)
                    {
                        using (var stream = LoadStream("cgn_canada_csv_eng_light.csv"))
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string[] result = reader
                                .ReadToEnd()
                                .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                            this.geodata = result
                                .Skip(1)
                                .Select(GeoNameEntry.FromCsvLight)
                                //.OrderBy(x => x.Latitude)
                                //.ThenBy(x => x.Longitude)
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
                .GetManifestResourceStream("FleetComplete.Geocoder.CanadaGeoNameBase.Resources." + name);
        }
    }
}
