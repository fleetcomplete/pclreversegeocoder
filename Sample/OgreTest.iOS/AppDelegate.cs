﻿using System;
using Foundation;
using UIKit;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


namespace OgreTest.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            FormsGoogleMaps.Init("AIzaSyDZv4VUpwHSsnenr_o7kIOfVRsFhL-0neo");
            this.LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
