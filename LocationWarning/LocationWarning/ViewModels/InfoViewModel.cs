using System;
using System.Collections.Generic;
using System.Windows.Input;
using Acr;
using LocationWarning.Services;
using Plugin.Messaging;
using Plugin.Permissions.Abstractions;
using ReactiveUI.Fody.Helpers;


namespace LocationWarning.ViewModels
{
    public class InfoViewModel : AbstractViewModel
    {
        readonly ILocationService locations;
        readonly IPermissions permissions;
        readonly IList<IDisposable> cleanUp = new List<IDisposable>();


        public InfoViewModel(ILocationService locations,
                             IPermissions permissions,
                             IMessaging messaging,
                             LwSqlConnection conn)
        {
            this.locations = locations;
            this.permissions = permissions;

            this.SendDatabase = new Command(() =>
            {
                var backupLocation = conn.CreateDatabaseBackup(conn.Platform);

                var mail = new EmailMessageBuilder()
                    .To("allan.ritchie@fleetcomplete.com")
                    .Subject("Location POC Database")
                    //.WithAttachment(conn.DatabasePath, "application/octet-stream")
                    .WithAttachment(backupLocation, "application/octet-stream")
                    .Body("--")
                    .Build();

                messaging.EmailMessenger.SendEmail(mail);
            });
            this.TryResolveNow = new Command(this.locations.TryResolve);
        }


        public override async void OnActivate()
        {
            this.CacheValidityTime = this.locations.CacheValidityTime.ToString();

            this.cleanUp.Add(
                this.locations
                    .WhenGeocodeFailed()
                    .Subscribe(x => this.RefreshDates())
            );

            this.cleanUp.Add(
                this.locations
                    .WhenGeolocationChanged()
                    .Subscribe(x =>
                    {
                        this.Longitude = x.Longitude;
                        this.Latitude = x.Latitude;
                        this.Heading = x.Heading;
                    })
            );

            this.cleanUp.Add(
                this.locations
                    .WhenGeocodeAvailable()
                    .Subscribe(x =>
                    {
                        this.RefreshDates();

                        this.Country = x.Country;
                        this.State = x.State;
                        this.City = x.City;
                        this.TimeToResolve = x.TimeTakenInMilliseconds;
                    })
            );

            //this.permissions.CheckPermissionStatusAsync(Permission.Location)
            var result = await this.permissions.RequestPermissionsAsync(Permission.Location);
            this.HardwarePermissions = result[Permission.Location];

            if (this.HardwarePermissions == PermissionStatus.Granted)
                await this.locations.Engage();
        }


        public override void OnDeactivate()
        {
            base.OnDeactivate();
            foreach (var cu in this.cleanUp)
                cu.Dispose();

            this.cleanUp.Clear();
        }


        public ICommand SendDatabase { get; }
        public ICommand TryResolveNow { get; }

        [Reactive] public string LastGeocodeAttempt { get; private set; }
        [Reactive] public string LastGeocodeSuccess { get; private set; }
        [Reactive] public string NextGeocodeAttempt { get; private set; }
        [Reactive] public string CacheValidityTime { get; private set; }
        [Reactive] public double? Latitude { get; private set; }
        [Reactive] public double? Longitude { get; private set; }
        [Reactive] public double? Heading { get; private set; }
        [Reactive] public long? TimeToResolve { get; private set; }
        [Reactive] public string Country { get; private set; }
        [Reactive] public string State { get; private set; }
        [Reactive] public string City { get; private set; }
        [Reactive] public PermissionStatus HardwarePermissions { get; private set; } = PermissionStatus.Unknown;


        void RefreshDates()
        {
            this.NextGeocodeAttempt = ToDateString(this.locations.CacheValidUntil);
            this.LastGeocodeAttempt = ToDateString(this.locations.LastGeocodeAttempt);
            this.LastGeocodeSuccess = ToDateString(this.locations.LastGeocodeSuccess);
        }


        static string ToDateString(DateTimeOffset? value)
        {
            if (value == null)
                return "";

            return value.Value.LocalDateTime.ToString("MMM dd - hh:mm:ss");
        }
    }
}
