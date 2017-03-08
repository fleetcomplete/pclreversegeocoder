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

        [TestCase(43.6412141, -79.3794798, "East York, Ontario, CA", 46.263185311649451, 6.8803208637112485)]
        [TestCase(43.7670151, -79.3851929, "Willowdale, Ontario, CA", 268.78352721832232, 1.1173911395151426)]
        [TestCase(40.0041629, -77.2376133, "Biglerville, Pennsylvania, US", 188.04246975865627, 8.2604934064968845)]
        [TestCase(55.418415, -116.303474, "High Prairie, Alberta, CA", 274.69846244651956, 11.634830875089053)]
        [TestCase(49.7818425, -84.151597, "Hearst, Ontario, CA", 101.45990227224245, 36.56764625054074)]
        [TestCase(47.9846273, -84.780335, "Hornepayne, Ontario, CA", 0.19403151976810282, 136.87512006172636)]
        [TestCase(49.7345685, -86.9624405, "Greenstone, Ontario, CA", 269.68061726874754, 14.691785272905191)]
        [TestCase(49.2611191, -84.8983383, "Hornepayne, Ontario, CA", 110.8826281341826, 10.281652816270453)]
        public async Task Run(double lat, double lng, string expectedLocation, double expectedDirection, double expectedApproxDistance)
        {
            var results = await new NGeoNamesGeocoder().FindClosestCities(lat, lng, 1);
            var result = results.FirstOrDefault();

            result.Should().NotBeNull();

            var loc = $"{result.City}, {result.StateProvince}, {result.CountryCode}";
            loc.Should().Be(expectedLocation);

            result.ApproxDistance.TotalKilometers.Should().Be(expectedApproxDistance, "Distance is wrong");
            result.DirectionFrom.Should().Be(expectedDirection, "Direction is wrong");
        }
    }
}
