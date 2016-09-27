using System;
using Xamarin;


namespace OgreTest.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            FormsGoogleMaps.Init("");
            this.LoadApplication(new OgreTest.App());
        }
    }
}
