using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Xamarin.Core.Droid
{
	public class XamarinFrameLayout : FrameLayout, IControl
	{
		public bool IsVisible
		{
			get { return Visibility == ViewStates.Visible; }
			set { Visibility = value ? ViewStates.Visible : ViewStates.Gone; }
		}

		protected XamarinFrameLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public XamarinFrameLayout(Context context) :
			base(context)
		{
		}

		public XamarinFrameLayout(Context context, IAttributeSet attrs) :
			base(context, attrs)
		{
		}

		public XamarinFrameLayout(Context context, IAttributeSet attrs, int defStyle) :
			base(context, attrs, defStyle)
		{
		}
	}
}
