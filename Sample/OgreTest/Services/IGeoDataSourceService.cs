using System.Collections.Generic;
using System.ComponentModel;
using FleetComplete.Geocoder;

namespace OgreTest.Services
{
    public interface IGeoDataSourceService : INotifyPropertyChanged
    {
        Geocoders ActiveGeocoder { get; }
        IGeocoder Geocoder { get; }
        IList<Geocoders> GetGeocoders();
        void SetGeocoder(Geocoders geocoder);
    }
}