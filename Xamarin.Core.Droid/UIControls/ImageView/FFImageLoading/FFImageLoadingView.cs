using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using FFImageLoading;
using FFImageLoading.Views;
using FFImageLoading.Work;
using Uri = Android.Net.Uri;

namespace Xamarin.Core.Droid
{
	public class FFImageLoadingView : ImageViewAsync, IUrlImageView
	{
		protected string _defaultImageName;

		public bool IsVisible
		{
			get { return Visibility == ViewStates.Visible; }
			set { Visibility = value ? ViewStates.Visible : ViewStates.Gone; }
		}

		public bool HasImage { get { return Drawable != null; } }

		protected FFImageLoadingView(IntPtr javaReference, JniHandleOwnership transfer)
			: base(javaReference, transfer)
		{
		}

		public FFImageLoadingView(Context context)
			: base(context)
		{
		}

		public FFImageLoadingView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
		}

		public void SetImage(string imagePath)
		{
			if (!string.IsNullOrEmpty(imagePath))
			{
				SetImageURI(Uri.Parse(imagePath));
			}
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

		public void DownloadImageUrl(string url)
		{
			ImageService.Instance.LoadUrl(url).Success(DownloadSuccess).DownloadOnly();
		}

		private void DownloadSuccess(ImageInformation info, LoadingResult result)
		{
			if (!string.IsNullOrEmpty(info.FilePath))
			{
				SetImageURI(Uri.Parse(info.FilePath));
			}
		}
	}
}
