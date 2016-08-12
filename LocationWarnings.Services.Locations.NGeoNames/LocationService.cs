using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using FleetComplete.Geocoder;
using Plugin.Geolocator.Abstractions;


namespace LocationWarning.Services.Locations.NGeoNames
{
    public class LocationService : ILocationService
    {
        readonly IGeolocator geolocator;
        readonly IGeocoder geocoder;
        readonly object syncLock = new object();
        event EventHandler<Geolocation> GeolocationUpdated;
        event EventHandler<Geolocation> GeocoderFailed;
        event EventHandler<GeocodeResult> GeocodeAvailable;


        public LocationService(IGeolocator geolocator, IGeocoder geocoder)
        {
            this.geolocator = geolocator;
        }


        public IObservable<Geolocation> WhenGeolocationChanged()
        {
            return Observable.Create<Geolocation>(ob =>
            {
                var handler = new EventHandler<Geolocation>((sender, args) => ob.OnNext(args));
                this.GeolocationUpdated += handler;
                return () => this.GeolocationUpdated -= handler;
            });
        }


        public IObservable<Geolocation> WhenGeocodeFailed()
        {
            return Observable.Create<Geolocation>(ob =>
            {
                var handler = new EventHandler<Geolocation>((sender, args) => ob.OnNext(args));
                this.GeocoderFailed += handler;
                return () => this.GeocoderFailed -= handler;
            });
        }


        public IObservable<GeocodeResult> WhenGeocodeAvailable()
        {
            return Observable
                .Create<GeocodeResult>(ob =>
                {
                    var handler = new EventHandler<GeocodeResult>((sender, args) => ob.OnNext(args));
                    this.GeocodeAvailable += handler;
                    return () => this.GeocodeAvailable -= handler;
                })
                .DistinctUntilChanged(x => x.City);
        }


        public DateTimeOffset? CacheValidUntil { get; set; }
        public TimeSpan CacheValidityTime { get; set; } = TimeSpan.FromMinutes(1);
        public DateTimeOffset? LastGeocodeAttempt { get; private set; }
        public DateTimeOffset? LastGeocodeSuccess { get; private set; }


        public async Task Engage()
        {
            this.geolocator.AllowsBackgroundUpdates = true;
            this.geolocator.PositionChanged += this.OnPositionChanged;
            await this.geolocator.StartListeningAsync(0, 10, true);
        }


        public async void TryResolve()
        {
            using (var cancelSrc = new CancellationTokenSource())
            {
                try
                {

                    Task.Delay(TimeSpan.FromSeconds(5))
                        .ContinueWith(_ => cancelSrc.Cancel());

                    var position = await this.geolocator.GetPositionAsync(token: cancelSrc.Token);
                    if (position != null)
                    {
                        this.TryResolveInternal(new Geolocation
                        {
                            Latitude = position.Latitude,
                            Longitude = position.Longitude,
                            Heading = position.Heading
                        });
                    }
                }
                catch (Exception ex)
                {
                    //this.GeocoderFailed?.Invoke(this, null);
                }
            }
        }


        void OnPositionChanged(object sender, PositionEventArgs args)
        {
            lock (this.syncLock)
            {
                var loc = new Geolocation
                {
                    Longitude = args.Position.Longitude,
                    Latitude = args.Position.Latitude,
                    Heading = args.Position.Heading
                };
                this.GeolocationUpdated?.Invoke(this, loc);

                if (!this.IsWriteTime())
                    return;

                this.LastGeocodeAttempt = DateTimeOffset.UtcNow;
                this.TryResolveInternal(loc);
            }
        }


        async Task TryResolveInternal(Geolocation loc)
        {
            var sw = new Stopwatch();
            sw.Start();
            var results = await this.geocoder.FindClosestCities(loc.Latitude, loc.Longitude, 1);
            sw.Stop();

            var result = results.FirstOrDefault();

            if (result == null)
                this.GeocoderFailed?.Invoke(this, loc);
            else
            {
                this.LastGeocodeSuccess = DateTimeOffset.UtcNow;
                this.GeocodeAvailable?.Invoke(this, new GeocodeResult
                {
                    Geo = loc,
                    Country = result.CountryCode,
                    State = result.StateCode,
                    City = result.City,
                    TimeTakenInMilliseconds = sw.ElapsedMilliseconds
                });
            }
        }


        bool IsWriteTime()
        {
            if (this.CacheValidUntil == null)
            {
                this.CacheValidUntil = DateTimeOffset.UtcNow;
                return true;
            }
            var next = this.CacheValidUntil.Value.Add(this.CacheValidityTime);
            if (next < DateTimeOffset.UtcNow)
            {
                this.CacheValidUntil = DateTimeOffset.UtcNow;
                return true;
            }
            return false;
        }
    }
}
