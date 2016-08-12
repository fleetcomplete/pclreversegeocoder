using System;
using System.IO;
using LocationWarning.Domain;
using SQLite.Net;
using SQLite.Net.Interop;


namespace LocationWarning
{
    public class LwSqlConnection : SQLiteConnectionWithLock
    {
        public LwSqlConnection(ISQLitePlatform sqlitePlatform, string path)
            : base(sqlitePlatform, new SQLiteConnectionString(Path.Combine(path, "Locations.db"), true))
        {
            this.CreateTable<Geocode>();
            this.CreateTable<Error>();
        }


        public TableQuery<Geocode> Geocodes => this.Table<Geocode>();
        public TableQuery<Error> Errors => this.Table<Error>();
    }
}
