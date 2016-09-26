using System;
using OgreTest.ViewModels;
using Xamarin.Forms.GoogleMaps;


namespace OgreTest
{
    public static class Extensions
    {
        public static Position ToMapPosition(this Coordinates coords)
        {
            return new Position(coords.Latitude, coords.Longitude);
        }
    }
}
