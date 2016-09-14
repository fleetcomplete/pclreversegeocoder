using System;


namespace FleetComplete.Geocoder
{
    public class GeoCoordinates
    {
        public GeoCoordinates(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }


        public double Latitude { get; }
        public double Longitude { get; }
    }
}
