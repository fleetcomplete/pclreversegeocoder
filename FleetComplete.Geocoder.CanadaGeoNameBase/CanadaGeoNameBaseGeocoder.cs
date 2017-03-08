using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetComplete.Geocoder.CanadaGeoNameBase
{
    public class CanadaGeoNameBaseGeocoder : IGeocoder
    {
        public Task<IEnumerable<IGeocoderResult>> FindClosestCities(double latitude, double longitude, int take = 5)
        {
        }
    }
}
