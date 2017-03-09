using System;
using Acr.UserDialogs;
using Autofac;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;


namespace OgreTest
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder
                .Register(_ => UserDialogs.Instance)
                .As<IUserDialogs>()
                .SingleInstance();

            builder
                .Register(_ => CrossGeolocator.Current)
                .As<IGeolocator>()
                .SingleInstance();

            builder
                .RegisterAssemblyTypes(this.ThisAssembly)
                .Where(x => x.Namespace.StartsWith("OgreTest.Services"))
                .AsImplementedInterfaces()
                .AutoActivate()
                .SingleInstance();

            builder
                .RegisterAssemblyTypes(this.ThisAssembly)
                .Where(x => x.Namespace.StartsWith("OgreTest.Pages"))
                .AsSelf()
                .InstancePerDependency();

            builder
                .RegisterAssemblyTypes(this.ThisAssembly)
                .Where(x => x.Namespace.StartsWith("OgreTest.ViewModels"))
                .AsSelf()
                .InstancePerDependency();
        }
    }
}
