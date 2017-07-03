using System;
using CoreGraphics;
using Foundation;
using Xamarin.Core.iOS;

namespace iHere.iOS
{
    [Register("iHereButton")]
    public class iHereButton : XamarinButton
    {
        protected iHereButton(IntPtr handle) : base(handle) { Initialize(); }

        public iHereButton() : base() { Initialize(); }

        public iHereButton(CGRect frame) : base(frame) { Initialize(); }

        private void Initialize()
        {
            this.Layer.CornerRadius = this.Frame.Height / 2;
            this.ClipsToBounds = true;  
        }
    }
}
