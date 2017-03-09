using System;
using System.Collections.Generic;
using FleetComplete.Geocoder;
using FleetComplete.Geocoder.CanadaGeoNameBase;
using FleetComplete.Geocoder.NGeoNames;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace OgreTest.Services
{
    public class GeoDataSourceService : ReactiveObject, IGeoDataSourceService
    {
        [Reactive]
        public IGeocoder Geocoder { get; private set; }
        public Geocoders ActiveGeocoder { get; private set; }

        private IGeocoder nGeoNamesGeocoder;
        private IGeocoder canadaGeoNameBaseGeocoder;

        public IList<Geocoders> GetGeocoders()
        {
            return (Geocoders[]) Enum.GetValues(typeof(Geocoders));
        }

        public void SetGeocoder(Geocoders geocoder)
        {
            switch (geocoder)
            {
                case Geocoders.NGeoNames:
                    nGeoNamesGeocoder = nGeoNamesGeocoder ?? new NGeoNamesGeocoder();
                    this.ActiveGeocoder = geocoder;
                    this.Geocoder = nGeoNamesGeocoder;
                    break;
                case Geocoders.CanadaGeoNameBase:
                    canadaGeoNameBaseGeocoder = canadaGeoNameBaseGeocoder ?? new CanadaGeoNameBaseGeocoder();
                    this.ActiveGeocoder = geocoder;
                    this.Geocoder = canadaGeoNameBaseGeocoder;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(geocoder), geocoder, null);
            }
        }

    }
}
