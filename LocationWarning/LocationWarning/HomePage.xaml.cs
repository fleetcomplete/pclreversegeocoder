using System;
using Acr;
using Xamarin.Forms;


namespace LocationWarning
{
    public partial class HomePage : TabbedPage
    {
        public HomePage()
        {
            InitializeComponent();
        }


        protected override void OnAppearing() 
        {
            (this.BindingContext as IViewModelLifecycle)?.OnActivate();
        }


        protected override void OnDisappearing() 
        {
            (this.BindingContext as IViewModelLifecycle)?.OnDeactivate();
        }
    }
}
