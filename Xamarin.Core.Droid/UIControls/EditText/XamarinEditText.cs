using System;
using Android.Content;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Xamarin.Core.Droid
{
	public class XamarinEditText : EditText, IInputField
	{
		public string Input
		{
			get { return Text; }
			set { Text = value; }
		}

		public bool IsVisible
		{
			get { return Visibility == ViewStates.Visible; }
			set { Visibility = value ? ViewStates.Visible : ViewStates.Gone; }
		}

		public InputType InputFieldType
		{
			set
			{
				switch (value)
				{
					case Core.InputType.Text:
						InputType = InputTypes.ClassText;
						break;
					case Core.InputType.TextMultiline:
						InputType = InputTypes.ClassText | InputTypes.TextFlagMultiLine;
						break;
					case Core.InputType.Numeric:
						InputType = InputTypes.ClassNumber;
						break;
					case Core.InputType.NumericSign:
						InputType = InputTypes.ClassNumber | InputTypes.NumberFlagSigned;
						break;
					case Core.InputType.Email:
						InputType = InputTypes.ClassText | InputTypes.TextVariationEmailAddress;
						break;
					case Core.InputType.Password:
						InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
						break;
				}
			}
		}

		protected XamarinEditText(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		public XamarinEditText(Context context) :
			base(context)
		{
		}

		public XamarinEditText(Context context, IAttributeSet attrs) :
			base(context, attrs)
		{
		}

		public XamarinEditText(Context context, IAttributeSet attrs, int defStyle) :
			base(context, attrs, defStyle)
		{
		}
	}
}
