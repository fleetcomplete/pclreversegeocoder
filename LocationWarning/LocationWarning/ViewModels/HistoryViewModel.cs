using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Acr;
using Acr.UserDialogs;
using LocationWarning.Domain;
using LocationWarning.Services;
using Plugin.ExternalMaps.Abstractions;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace LocationWarning.ViewModels
{
    public class HistoryViewModel : AbstractViewModel
    {
        readonly LwSqlConnection conn;
        readonly IDisposable refresher;


        public HistoryViewModel(LwSqlConnection conn,
                                IUserDialogs dialogs,
                                ILocationService locations,
                                IExternalMaps maps)
        {
            this.conn = conn;
            this.Reload = new Command(this.Load);
            this.Clear = ReactiveCommand.CreateAsyncTask(async _ =>
            {
                var result = await dialogs.ConfirmAsync(new ConfirmConfig()
                    .UseYesNo()
                    .SetTitle("DANGER")
                    .SetMessage("Are you sure you want to delete this history data?  This can be considered a crime punishable by death")
                );

                if (result)
                {
                    conn.DeleteAll<Geocode>();
                    this.Load();
                }
            });

            this.Select = new Command<DataItemViewModel>(async x =>
                await maps.NavigateTo(x.Location, x.Latitude, x.Longitude)
            );
            this.refresher = locations
                .WhenGeocodeAvailable()
                .Subscribe(_ => this.Load());
        }


        public override void OnActivate()
        {
            base.OnActivate();
            this.Load();
        }


        void Load()
        {
            this.IsLoading = true;
            this.List = this.conn
                .Geocodes
                .OrderByDescending(x => x.DateCreatedUtc)
                .Select(x => new DataItemViewModel
                {
                    Location = $"{x.City}, {x.State}, {x.Country}",
                    TimeTaken = $"{x.TimeTakenInMilliseconds} ms",
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    DateTime = x.DateCreatedUtc.ToDateString()
                })
                .ToList();
            this.IsLoading = false;
        }


        public ICommand Reload { get; }
        public ICommand Clear { get; }
        public Command<DataItemViewModel> Select { get; }
        [Reactive] public bool IsLoading { get; private set; }
        [Reactive] public IList<DataItemViewModel> List { get; private set; }
    }
}
