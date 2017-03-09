using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using FleetComplete.Geocoder;
using OgreTest.Services;
using Plugin.Geolocator.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace OgreTest.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        readonly IUserDialogs dialogs;
        readonly IGeolocator geolocator;
        readonly IGeoDataSourceService geoDataSourceService;
        readonly Subject<object> zoomRequest;
        bool isStarted = false;


        public MainViewModel(IUserDialogs dialogs, IGeolocator geolocator, IGeoDataSourceService geoDataSourceService)
        {
            this.dialogs = dialogs;
            this.geolocator = geolocator;
            this.geoDataSourceService = geoDataSourceService;
            this.zoomRequest = new Subject<object>();

            geoDataSourceService.SetGeocoder(Geocoders.NGeoNames);

            this.Zoom = ReactiveCommand.CreateAsyncTask(
                this.WhenAny(
                    x => x.CurrentCoordinates,
                    x => x.ResolvedCityCoordinates,
                    (current, resolved) => current.Value != null && resolved != null
                ),
                async _ => this.zoomRequest.OnNext(null)
            );

            this.SelectGeoData = ReactiveCommand.CreateAsyncTask(
                async _ =>
                {
                    var result = await this.dialogs.ActionSheetAsync("Select Data Source", 
                        "Cancel", 
                        null, 
                        null,
                        geoDataSourceService.GetGeocoders().Select(x => x.ToString()).ToArray());

                    if (result != "Cancel")
                    {
                        var selectedGeocoder = (Geocoders) Enum.Parse(typeof(Geocoders), result);
                        geoDataSourceService.SetGeocoder(selectedGeocoder);
                    }
                }
            );

            geoDataSourceService.WhenAnyValue(x => x.Geocoder)
                .Subscribe(x =>
                {
                    this.CurrentGeocoder = geoDataSourceService.ActiveGeocoder;
                    ResolveLocation(this.CurrentCoordinates);
                });

            this.WhenAnyValue(x => x.CurrentCoordinates)
                .Skip(1)
                .Subscribe(x =>
                {
                    ResolveLocation(x);
                });
        }

        private async Task ResolveLocation(Coordinates coords)
        {
            RemoveValues();

            IGeocoderResult result = null;

            await Task.Run(async () =>
            {
                var results = await geoDataSourceService.Geocoder.FindClosestCitiesAsync(coords.Latitude, coords.Longitude, 1);
                result = results.First();
            });

            this.Distance = Convert.ToInt32(Math.Round(result.ApproxDistance.TotalKilometers, 0));
            this.LocationName = $"{result.City}, {result.StateProvince}, {result.Country}";
            this.LocationNameAbbreviated = $"{result.City}, {result.StateProvinceCode}, {result.CountryCode}";
            this.DirectionInDegrees = Math.Round(result.DirectionInDegreesFrom, 0);
            this.Direction = result.DirectionFrom;
            this.ResolvedCityCoordinates = new Coordinates(result.Coordinates.Latitude, result.Coordinates.Longitude);

            this.CurrentCoordinatesText = this.CurrentCoordinates.ToString();
            this.ResolvedCoordinatesText = this.ResolvedCityCoordinates.ToString();
            if (!this.isStarted)
            {
                this.isStarted = true;
                this.zoomRequest.OnNext(null);
            }
        }

        private void RemoveValues()
        {
            this.Distance = 0;
            this.LocationName = "";
            this.LocationNameAbbreviated = "";
            this.DirectionInDegrees = 0;
            this.Direction = CardinalDirection.North;
            this.CurrentCoordinatesText = "";
            this.ResolvedCoordinatesText = "";
        }

        public IObservable<object> WhenZoomRequested() => this.zoomRequest;


        public async void Start()
        {
            if (this.isStarted)
                return;

            try
            {
                using (this.dialogs.Loading("Resolving Current Location..."))
                {
                    var position = await this.geolocator.GetPositionAsync();
                    this.CurrentCoordinates = new Coordinates(position.Latitude, position.Longitude);
                }
            }
            catch
            {
                this.dialogs.Alert("Could not retrieve current location");
            }
        }


        public ICommand Zoom { get; }
        public ICommand SelectGeoData { get; }

        [Reactive] public Geocoders CurrentGeocoder { get; private set; }
        [Reactive] public Coordinates CurrentCoordinates { get; set; }
        [Reactive] public Coordinates ResolvedCityCoordinates { get; private set; }
        [Reactive] public string CurrentCoordinatesText { get; private set; }
        [Reactive] public string ResolvedCoordinatesText { get; private set; }
        [Reactive] public double DirectionInDegrees { get; private set; }
        [Reactive] public CardinalDirection Direction { get; private set; }
        [Reactive] public string LocationName { get; private set; }
        [Reactive] public string LocationNameAbbreviated { get; private set; }
        [Reactive] public int Distance { get; private set; }
    }
}
