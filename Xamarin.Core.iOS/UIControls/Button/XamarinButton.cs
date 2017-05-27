using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
    [Register("XamarinButton")]
    public class XamarinButton : UIButton, IButton
    {
        public bool IsVisible
        {
            get { return !Hidden; }
            set { Hidden = !value; }
        }

        public string Text
        {
            get { return Title(UIControlState.Normal); }
            set { SetTitle(value, UIControlState.Normal); }
        }

        public event EventHandler OnClick;

        protected XamarinButton(IntPtr handle) : base(handle) { }

        public XamarinButton() : base() { }

        public XamarinButton(CGRect frame) : base(frame) { }
    }
}
