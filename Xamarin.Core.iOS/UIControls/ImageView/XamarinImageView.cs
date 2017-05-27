using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
	[Register("XamarinImageView")]
	public class XamarinImageView : UIImageView, IImageView
	{
		public bool IsVisible
		{
			get { return !Hidden; }
			set { Hidden = !value; }
		}

		public bool Enabled
		{
			get { return UserInteractionEnabled; }
			set { UserInteractionEnabled = value; }
		}

		public bool HasImage { get { return Image != null; } }

		protected XamarinImageView(IntPtr handle) : base(handle) { }

		public XamarinImageView() : base() { }

		public XamarinImageView(CGRect frame) : base(frame) { }

		public void SetImage(string imagePath)
		{
			if (!string.IsNullOrEmpty(imagePath))
			{
				Image = UIImage.FromFile(imagePath);
			}
		}
	}
}
