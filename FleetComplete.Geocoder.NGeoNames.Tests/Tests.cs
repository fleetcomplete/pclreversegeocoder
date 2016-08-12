using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;


namespace FleetComplete.Geocoder.NGeoNames.Tests
{
    [TestFixture]
    public class Tests
    {

        [TestCase(43.6412141, -79.3794798, "Toronto, Ontario, CA", 327.98743807719131, 7.1935903512483472)]
        [TestCase(43.7670151, -79.3851929, "Willowdale, Ontario, CA", 268.78352721832232, 1.1173911395151426)]
        [TestCase(40.0041629, -77.2376133, "Gettysburg, Pennsylvania, US", 177.84677795557354, 19.287263070704739)] // southeast of coords
        public async Task Run(double lat, double lng, string expectedLocation, double expectedDirection, double expectedApproxDistance)
        {
            var results = await new GeocoderImpl().FindClosestCities(lat, lng, 1);
            var result = results.FirstOrDefault();

            result.Should().NotBeNull();
            result.Direction.Should().Be(expectedDirection, "Direction is wrong");
            result.ApproxDistanceTo.TotalKilometers.Should().Be(expectedApproxDistance, "Distance is wrong");

            var loc = $"{result.City}, {result.StateCode}, {result.CountryCode}";
            loc.Should().Be(expectedLocation);
        }
    }
}
