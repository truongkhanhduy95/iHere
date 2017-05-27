using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Xamarin.Core.Droid
{
	public class XamarinTextView : TextView, ILabel
	{
		public bool IsVisible
		{
			get { return Visibility == ViewStates.Visible; }
			set { Visibility = value ? ViewStates.Visible : ViewStates.Gone; }
		}

		protected XamarinTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public XamarinTextView(Context context) :
			base(context)
		{
		}

		public XamarinTextView(Context context, IAttributeSet attrs) :
			base(context, attrs)
		{
		}

		public XamarinTextView(Context context, IAttributeSet attrs, int defStyle) :
			base(context, attrs, defStyle)
		{
		}
	}
}
