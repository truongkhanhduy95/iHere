using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Xamarin.Core.Droid
{
	public class XamarinLinearLayout : LinearLayout, IControl
	{
		public bool IsVisible
		{
			get { return Visibility == ViewStates.Visible; }
			set { Visibility = value ? ViewStates.Visible : ViewStates.Gone; }
		}

		protected XamarinLinearLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public XamarinLinearLayout(Context context) :
			base(context)
		{
		}

		public XamarinLinearLayout(Context context, IAttributeSet attrs) :
			base(context, attrs)
		{
		}

		public XamarinLinearLayout(Context context, IAttributeSet attrs, int defStyle) :
			base(context, attrs, defStyle)
		{
		}
	}
}
