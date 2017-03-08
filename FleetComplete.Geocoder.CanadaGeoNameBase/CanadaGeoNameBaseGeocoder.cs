using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FleetComplete.Geocoder.CanadaGeoNameBase
{
    public class CanadaGeoNameBaseGeocoder : IGeocoder
    {
        private IList<GeoNameEntry> geodata;

        public async Task<IEnumerable<IGeocoderResult>> FindClosestCitiesAsync(double latitude, double longitude, int take = 5)
        {
            await Task.Run(() => this.EnsureLoaded());

            return null;
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
                        using (var stream = LoadStream("cgn_canada_csv_eng.csv"))
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string[] result = reader
                                .ReadToEnd()
                                .Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                            this.geodata = result.Skip(1)
                                .Select(GeoNameEntry.FromCsv)
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
                .GetManifestResourceStream("FleetComplete.Geocoder.CanadaGeoNameBase.Resources." + name);
        }
    }
}
