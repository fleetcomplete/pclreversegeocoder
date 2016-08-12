using System;
using System.Reactive.Linq;
using Autofac;
using LocationWarning.Domain;
using LocationWarning.Services;


namespace LocationWarning.Tasks
{
    public class StoreTask : IStartable
    {
        readonly ILocationService locations;
        readonly LwSqlConnection conn;


        public StoreTask(ILocationService locations, LwSqlConnection conn)
        {
            this.locations = locations;
            this.conn = conn;
        }


        public void Start()
        {
            this.locations
                .WhenGeocodeAvailable()
                .Subscribe(x => this.conn.Insert(new Geocode
                {
                    State = x.State,
                    City = x.City,
                    Country = x.Country,

                    DateCreatedUtc = DateTime.UtcNow,
                    Heading = x.Geo.Heading,
                    Latitude = x.Geo.Latitude,
                    Longitude = x.Geo.Longitude,
                    TimeTakenInMilliseconds = x.TimeTakenInMilliseconds
                }));

            this.locations
                .WhenGeocodeFailed()
                .Subscribe(x => this.conn.Insert(new Error
                {
                    DateCreatedUtc = DateTime.UtcNow,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude
                }));
        }
    }
}
