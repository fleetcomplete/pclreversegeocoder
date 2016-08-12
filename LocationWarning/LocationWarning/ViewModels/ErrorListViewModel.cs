using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Acr;
using Acr.UserDialogs;
using LocationWarning.Domain;
using LocationWarning.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;


namespace LocationWarning.ViewModels
{
    public class ErrorListViewModel : AbstractViewModel
    {
        readonly LwSqlConnection conn;
        readonly IDisposable refresher;


        public ErrorListViewModel(LwSqlConnection conn, IUserDialogs dialogs, ILocationService locations)
        {
            this.conn = conn;

            this.Reload = new Command(this.Load);
            this.Clear = ReactiveCommand.CreateAsyncTask(async _ =>
            { 
                var result = await dialogs.ConfirmAsync(new ConfirmConfig()
                                                        .UseYesNo()
                                                        .SetTitle("DANGER")
                                                        .SetMessage("Are you sure you want to delete this history data?  This can be considered a crime punishable by death"));

                if (result) 
                {
                    conn.DeleteAll<Error>();
                    this.Load();
                }
            });
            this.refresher = locations
                .WhenGeocodeFailed()
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
                .Errors
                .OrderByDescending(x => x.DateCreatedUtc)
                .Select(x => new DataItemViewModel 
                {
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    DateTime = x.DateCreatedUtc.ToDateString()
                })
                .ToList();
            this.IsLoading = false;
        }


        public ICommand Reload { get; }
        public ICommand Clear { get; }
        [Reactive] public bool IsLoading { get; private set; }
        [Reactive] public IList<DataItemViewModel> List { get; private set; }
    }
}
