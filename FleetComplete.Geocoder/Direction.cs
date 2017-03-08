using System;
using GeoCoordinatePortable;

namespace FleetComplete.Geocoder
{
    public static class Direction
    {
        public static double CalculateDirection(GeoCoordinate coordFrom, GeoCoordinate coordTo)
        {
            var dx = coordTo.Latitude - coordFrom.Latitude;
            var dy = coordTo.Longitude - coordFrom.Longitude;
            var result = Math.Atan2(dy, dx) * (180 / Math.PI);
            if (result < 0)
                result = 360 - Math.Abs(result);

            return result;
        }

        public static CardinalDirection GetDirection(double degrees)
        {
            var caridnals = new[] { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };
            var value = caridnals[(int)Math.Round((degrees % 360) / 45)];
            switch (value)
            {
                case "NW": return CardinalDirection.NorthWest;
                case "W": return CardinalDirection.West;
                case "SW": return CardinalDirection.SouthWest;
                case "S": return CardinalDirection.South;
                case "SE": return CardinalDirection.SouthEast;
                case "E": return CardinalDirection.East;
                case "NE": return CardinalDirection.NorthEast;
                default:
                case "N": return CardinalDirection.North;

            }
        }
    }
}
