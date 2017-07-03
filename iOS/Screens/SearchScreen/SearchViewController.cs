using System;
using iHere.Shared.ViewModels;
using UIKit;
using Xamarin.Core.iOS.Views;

namespace iHere.iOS
{
    public partial class SearchViewController : BaseViewController<LoginViewModel>
    {
		public override UIStatusBarStyle PreferredStatusBarStyle()
		{
			return UIStatusBarStyle.LightContent;
		}

        public SearchViewController() : base("SearchViewController")
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        protected override void SetBindings()
        {
            
        }
    }
}

