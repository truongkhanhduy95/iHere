using System;
using Android.Content;
using Android.Runtime;
using Android.Util;

namespace Xamarin.Core.Droid
{
	public class UniversalImageLoaderView : XamarinImageView, IUrlImageView
	{
		protected UniversalImageLoaderView(IntPtr javaReference, JniHandleOwnership transfer)
					: base(javaReference, transfer)
		{
		}

		public UniversalImageLoaderView(Context context)
					: base(context)
		{
		}

		public UniversalImageLoaderView(Context context, IAttributeSet attrs)
					: base(context, attrs)
		{
		}

		public void SetImageUrl(string url)
		{
			if (!string.IsNullOrEmpty(url))
			{
				Com.Nostra13.Universalimageloader.Core.ImageLoader.Instance.DisplayImage(url, this);
			}
		}

		public void SetImageUrl(string url, int width, int height)
		{
			if (!string.IsNullOrEmpty(url))
			{
				Com.Nostra13.Universalimageloader.Core.ImageLoader.Instance.DisplayImage(url, this);
			}
		}
	}
}
