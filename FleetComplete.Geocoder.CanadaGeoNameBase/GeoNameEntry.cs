using System;

namespace FleetComplete.Geocoder.CanadaGeoNameBase
{
    internal class GeoNameEntry
    {
        public string Id { get; set; }
        public string GeographicalName { get; set; }
        public string Language { get; set; }
        public string SyllabicForm { get; set; }
        public string GenericTerm { get; set; }
        public string GenericCategory { get; set; }
        public string ToponymicFeatureId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Location { get; set; }
        public string ProvinceTerritory { get; set; }
        public string RelevanceAtScale { get; set; }
        public string DecisionDate { get; set; }
        public string Source { get; set; }

        public static GeoNameEntry FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            var result = new GeoNameEntry();

            result.Id = values[0];
            result.GeographicalName = values[1];
            result.Language = values[2];
            result.SyllabicForm = values[3];
            result.GenericTerm = values[4];
            result.GenericCategory = values[5];
            result.ToponymicFeatureId = values[6];
            result.Latitude = values[7];
            result.Longitude = values[8];
            result.Location = values[9];
            result.ProvinceTerritory = values[10];
            result.RelevanceAtScale = values[11];
            result.DecisionDate = values[12];
            result.Source = values[13];

            return result;
        }
    }
}
