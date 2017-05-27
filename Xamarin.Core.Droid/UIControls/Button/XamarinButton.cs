using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Xamarin.Core.Droid
{
	public class XamarinButton : Button, IButton
	{
		public bool IsVisible
		{
			get { return Visibility == ViewStates.Visible; }
			set { Visibility = value ? ViewStates.Visible : ViewStates.Gone; }
		}

		public event EventHandler OnClick
		{
			add { Click += value; }
			remove { Click -= value; }
		}

		protected XamarinButton(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public XamarinButton(Context context) :
			base(context)
		{
		}

		public XamarinButton(Context context, IAttributeSet attrs) :
			base(context, attrs)
		{
		}

		public XamarinButton(Context context, IAttributeSet attrs, int defStyle) :
			base(context, attrs, defStyle)
		{
		}
	}
}
