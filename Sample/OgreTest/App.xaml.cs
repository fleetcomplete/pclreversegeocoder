using System;
using Autofac;
using OgreTest.Pages;
using OgreTest.ViewModels;
using Xamarin.Forms;


namespace OgreTest
{
    public partial class App : Application
    {
        readonly IContainer container;


        public App()
        {
            InitializeComponent();
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CoreModule());
            this.container = builder.Build();

            var viewModel = this.container.Resolve<MainViewModel>();
            MainPage = new NavigationPage(new MainPage { BindingContext = viewModel });
        }
    }
}
