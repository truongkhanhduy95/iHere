using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Uri = Android.Net.Uri;

namespace Xamarin.Core.Droid
{
	public class XamarinImageView : ImageView, IImageView
	{
		public bool IsVisible
		{
			get { return Visibility == ViewStates.Visible; }
			set { Visibility = value ? ViewStates.Visible : ViewStates.Gone; }
		}

		public bool HasImage { get { return Drawable != null; } }

		protected XamarinImageView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public XamarinImageView(Context context) :
			base(context)
		{
		}

		public XamarinImageView(Context context, IAttributeSet attrs) :
			base(context, attrs)
		{
		}

		public XamarinImageView(Context context, IAttributeSet attrs, int defStyle) :
			base(context, attrs, defStyle)
		{
		}

		public void SetImage(string imagePath)
		{
			if (!string.IsNullOrEmpty(imagePath))
			{
				SetImageURI(Uri.Parse(imagePath));
			}
		}
	}
}
