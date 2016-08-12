using System;
using SQLite.Net.Attributes;


namespace LocationWarning.Domain
{
    public class Geocode
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public DateTime DateCreatedUtc { get; set; }

        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Heading { get; set; }
        public long TimeTakenInMilliseconds { get; set; }
    }
}
