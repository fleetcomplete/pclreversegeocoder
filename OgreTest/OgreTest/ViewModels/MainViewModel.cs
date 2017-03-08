using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Input;
using Acr.UserDialogs;
using FleetComplete.Geocoder;
using Plugin.Geolocator.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Xamarin.Forms;


namespace OgreTest.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        readonly IUserDialogs dialogs;
        readonly IGeolocator geolocator;
        readonly Subject<object> zoomRequest;
        bool isStarted = false;


        public MainViewModel(IUserDialogs dialogs, IGeolocator geolocator, IGeocoder geocoder)
        {
            this.dialogs = dialogs;
            this.geolocator = geolocator;
            this.zoomRequest = new Subject<object>();

            this.Zoom = ReactiveCommand.CreateAsyncTask(
                this.WhenAny(
                    x => x.CurrentCoordinates,
                    x => x.ResolvedCityCoordinates,
                    (current, resolved) => current.Value != null && resolved != null
                ),
                async _ => this.zoomRequest.OnNext(null)
            );

            this.WhenAnyValue(x => x.CurrentCoordinates)
                .Skip(1)
                .Subscribe(async x =>
                {
                    var results = await geocoder.FindClosestCities(x.Latitude, x.Longitude, 1);
                    var result = results.First();

                    this.Distance = Convert.ToInt32(Math.Round(result.ApproxDistance.TotalKilometers, 0));
                    this.LocationName = $"{result.City}, {result.StateProvince}, {result.Country}";
                    this.LocationNameAbbreviated = $"{result.City}, {result.StateCode}, {result.CountryCode}";
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
                });
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
