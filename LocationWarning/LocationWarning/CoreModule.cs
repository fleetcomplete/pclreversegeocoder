using System;
using Acr.Notifications;
using Acr.UserDialogs;
using Autofac;
using FleetComplete.Geocoder.NGeoNames;
using Plugin.ExternalMaps;
using Plugin.ExternalMaps.Abstractions;
using Plugin.Messaging;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;


namespace LocationWarning
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
                .RegisterAssemblyTypes(this.ThisAssembly)
                .Where(x => x.Namespace.StartsWith("LocationWarning.Tasks"))
                .AsImplementedInterfaces()
                .SingleInstance()
                .AutoActivate();

            builder
                .RegisterAssemblyTypes(this.ThisAssembly)
                .Where(x => x.Namespace.StartsWith("LocationWarning.ViewModels"))
                .AsSelf()
                .InstancePerDependency();

            builder
                .Register(x => CrossMessaging.Current)
                .As<IMessaging>()
                .SingleInstance();

            builder
                .Register(x => CrossPermissions.Current)
                .As<IPermissions>()
                .SingleInstance();

            builder
                .Register(x => Notifications.Instance)
                .As<INotifications>()
                .SingleInstance();

            builder
                .Register(x => UserDialogs.Instance)
                .As<IUserDialogs>()
                .SingleInstance();

            builder
                .Register(x => CrossExternalMaps.Current)
                .As<IExternalMaps>()
                .SingleInstance();

            builder
                .RegisterType<GeocoderImpl>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
