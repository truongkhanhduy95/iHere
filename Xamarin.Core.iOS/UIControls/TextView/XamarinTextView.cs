using System;
using UIKit;

namespace Xamarin.Core.iOS
{
	public class XamarinTextView : UITextView, IMultilineInputField
	{
		public bool Enabled { get; set; }

		public string Hint { get; set; }

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

		public XamarinTextView() : base()
		{
		}

		public XamarinTextView(IntPtr handle) : base(handle)
		{
		}

		public XamarinTextView(CoreGraphics.CGRect frame) : base(frame)
		{
		}
	}
}
