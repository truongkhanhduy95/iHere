using System;
using iHere.Shared.ViewModels;
using UIKit;
using Xamarin.Core.iOS.Views;

namespace iHere.iOS
{
    public partial class DummyViewController : BaseViewController<DummyViewModel>
    {
        public DummyViewController() : base("DummyViewController")
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
            throw new NotImplementedException();
        }
    }
}

