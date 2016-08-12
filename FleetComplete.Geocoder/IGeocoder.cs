using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FleetComplete.Geocoder
{
    public interface IGeocoder
    {
        Task<IEnumerable<IGeocoderResult>> FindClosestCities(double latitude, double longitude, int take = 5);
    }
}
