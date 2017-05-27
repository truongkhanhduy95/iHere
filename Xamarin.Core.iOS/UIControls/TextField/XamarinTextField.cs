using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Core.iOS
{
	[Register("XamarinTextField")]
	public class XamarinTextField : UITextField, IInputField
	{
		public string Hint
		{
			get { return Placeholder; }
			set { Placeholder = value; }
		}

		public string Input
		{
			get { return Text; }
			set { Text = value; }
		}

		public InputType InputFieldType
		{
			set
			{
				switch (value)
				{
					case InputType.Text:
					case InputType.TextMultiline:
						KeyboardType = UIKeyboardType.Default;
						SecureTextEntry = false;
						break;
					case InputType.Numeric:
					case InputType.NumericSign:
						KeyboardType = UIKeyboardType.NumberPad;
						SecureTextEntry = false;
						break;
					case InputType.Email:
						KeyboardType = UIKeyboardType.EmailAddress;
						SecureTextEntry = false;
						break;
					case InputType.Password:
						KeyboardType = UIKeyboardType.Default;
						SecureTextEntry = true;
						break;
				}
			}
		}

		public bool IsVisible
		{
			get { return !Hidden; }
			set { Hidden = !value; }
		}

		protected XamarinTextField(IntPtr handle) : base(handle) { }

		public XamarinTextField() : base() { }

		public XamarinTextField(CGRect frame) : base(frame) { }
	}
}
