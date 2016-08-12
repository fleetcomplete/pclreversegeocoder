using System;
using Acr.Notifications;
using Autofac;
using LocationWarning.ViewModels;
using Xamarin.Forms;


namespace LocationWarning
{
    public class App : Application
    {
        readonly IContainer container;


        public App(IContainer container)
        {
            this.container = container;
            this.MainPage = new HomePage
            {
                BindingContext = container.Resolve<MainViewModel>()
            };
        }


        protected override void OnResume()
        {
            base.OnResume();
            this.container.Resolve<INotifications>().Badge = 0;
        }
    }
}
