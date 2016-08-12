using System;
using SQLite.Net.Attributes;


namespace LocationWarning.Domain
{
    public class Error
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DateCreatedUtc { get; set; }
    }
}
