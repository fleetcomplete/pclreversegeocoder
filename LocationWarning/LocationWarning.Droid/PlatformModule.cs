using System;
using Autofac;
using LocationWarning.Services.Locations.NGeoNames;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SQLite.Net.Platform.XamarinAndroid;


namespace LocationWarning.Droid
{
    public class PlatformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule(new CoreModule());

            builder
                .RegisterType<LocationService>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder
                .Register(x => new LwSqlConnection(
                    new SQLitePlatformAndroid(),
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal)
                ))
                .AsSelf()
                .SingleInstance();

            builder
                .Register(x => CrossGeolocator.Current)
                .As<IGeolocator>()
                .SingleInstance();
        }
    }
}