using System;
using System.Threading.Tasks;


namespace LocationWarning.Services
{
    public interface ILocationService
    {
        DateTimeOffset? CacheValidUntil { get; }
        TimeSpan CacheValidityTime { get; set; }
        IObservable<Geolocation> WhenGeolocationChanged();
        IObservable<GeocodeResult> WhenGeocodeAvailable();
        IObservable<Geolocation> WhenGeocodeFailed();
        DateTimeOffset? LastGeocodeAttempt { get; }
        DateTimeOffset? LastGeocodeSuccess { get; }
        Task Engage();

        void TryResolve();
    }
}
