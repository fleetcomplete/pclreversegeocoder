using System;
using Xamarin;


namespace OgreTest.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            FormsGoogleMaps.Init("AIzaSyB5bRboqq0mZrg-cwIt2EUQwdFjNC1weUg");
            this.LoadApplication(new OgreTest.App());
        }
    }
}
