using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
	[Register("XamarinView")]
	public class XamarinView : UIView, IControl
	{
		public bool Enabled
		{
			get { return UserInteractionEnabled; }
			set { UserInteractionEnabled = value; }
		}

		public bool IsVisible
		{
			get { return !Hidden; }
			set { Hidden = !value; }
		}

		protected XamarinView(IntPtr handle) : base(handle) { }

		public XamarinView() : base() { }

		public XamarinView(CGRect frame) : base(frame) { }
	}
}