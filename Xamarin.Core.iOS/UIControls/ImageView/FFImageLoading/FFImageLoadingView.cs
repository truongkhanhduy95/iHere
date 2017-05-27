using System;
using FFImageLoading;
using FFImageLoading.Work;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
	[Register("FFImageLoadingView")]
	public class FFImageLoadingView : XamarinImageView, IUrlImageView
	{
		public FFImageLoadingView(IntPtr p) : base(p)
		{
		}

		public FFImageLoadingView()
		{
		}

		public void SetImageUrl(string url)
		{
			if (!string.IsNullOrEmpty(url))
			{
				ImageService.Instance.LoadUrl(url).Into(this);
			}
		}

		public void SetImageUrl(string url, int width, int height)
		{
			if (!string.IsNullOrEmpty(url))
			{
				ImageService.Instance.LoadUrl(url).DownSample(width, height).Into(this);
			}
		}
	}
}
