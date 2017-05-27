using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
	[Register("XamarinLabel")]
	public class XamarinLabel : UILabel, ILabel
	{
		public bool IsVisible
		{
			get { return !Hidden; }
			set { Hidden = !value; }
		}

		protected XamarinLabel(IntPtr handle) : base(handle) { }

		public XamarinLabel() : base() { }

		public XamarinLabel(CGRect frame) : base(frame) { }
	}
}
