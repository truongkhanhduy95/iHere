using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Xamarin.Core.Droid
{
	public class XamarinRelativeLayout : RelativeLayout, IControl
	{
		public bool IsVisible
		{
			get { return Visibility == ViewStates.Visible; }
			set { Visibility = value ? ViewStates.Visible : ViewStates.Gone; }
		}

		protected XamarinRelativeLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public XamarinRelativeLayout(Context context) :
			base(context)
		{
		}

		public XamarinRelativeLayout(Context context, IAttributeSet attrs) :
			base(context, attrs)
		{
		}

		public XamarinRelativeLayout(Context context, IAttributeSet attrs, int defStyle) :
			base(context, attrs, defStyle)
		{
		}
	}
}
