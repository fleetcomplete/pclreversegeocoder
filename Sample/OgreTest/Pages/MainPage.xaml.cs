﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using OgreTest.ViewModels;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;


namespace OgreTest.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        MainViewModel viewModel;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.viewModel = this.BindingContext as MainViewModel;

            this.viewModel
                .WhenAnyValue(x => x.CurrentCoordinates)
                .Where(x => x != null)
                .Subscribe(coords =>
                {
                    this.MyMap.Pins.Remove(this.MyMap.FirstOrDefault(x => x.Type == PinType.Generic));

                    var current = new Position(coords.Latitude, coords.Longitude);

                    this.MyMap.Pins.Add(new Pin
                    {
                        IsDraggable = false,
                        Address = "Selected Location",
                        Label = coords.ToString(),
                        Position = current,
                        Type = PinType.Generic
                    });
                });

            this.viewModel
                .WhenAnyValue(x => x.ResolvedCityCoordinates)
                .Where(x => x != null)
                .Subscribe(coords =>
                {
                    this.MyMap.Pins.Remove(this.MyMap.FirstOrDefault(x => x.Type == PinType.SearchResult));

                    var current = new Position(this.viewModel.CurrentCoordinates.Latitude, this.viewModel.CurrentCoordinates.Longitude);
                    var position = new Position(coords.Latitude, coords.Longitude);

                    this.MyMap.Pins.Add(new Pin
                    {
                        Address = this.viewModel.LocationName,
                        Label = "Resolved City",
                        Position = position,
                        Type = PinType.SearchResult,
                        Icon = BitmapDescriptorFactory.DefaultMarker(Color.Fuchsia)
                    });
                    var line = new Polyline {StrokeColor = Color.Red};
                    line.Positions.Add(current);
                    line.Positions.Add(position);

                    this.MyMap.Polylines.Clear();
                    this.MyMap.Polylines.Add(line);
                });

            this.viewModel
                .WhenZoomRequested()
                .Subscribe(_ =>
                {
                    if (this.viewModel.CurrentCoordinates != null &&
                        this.viewModel.ResolvedCityCoordinates != null)
                    {
                        this.MyMap.MoveToRegion(MapSpan.FromPositions(new[]
                        {
                            this.viewModel.CurrentCoordinates.ToMapPosition(),
                            this.viewModel.ResolvedCityCoordinates.ToMapPosition()
                        }));
                    }
                });

            this.viewModel.Start();
        }


        void OnMapClicked(object sender, MapClickedEventArgs args)
        {
            this.viewModel.CurrentCoordinates = new Coordinates(args.Point.Latitude, args.Point.Longitude);
        }
    }
}