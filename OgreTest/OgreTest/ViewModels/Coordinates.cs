using System;


namespace OgreTest.ViewModels
{
    public class Coordinates
    {
        public Coordinates(double lat, double lng)
        {
            this.Latitude = lat;
            this.Longitude = lng;
        }


        public double Latitude { get; }
        public double Longitude { get; }


        public override string ToString()
        {
            return $"{this.Latitude}, {this.Longitude}";
        }
    }
}
