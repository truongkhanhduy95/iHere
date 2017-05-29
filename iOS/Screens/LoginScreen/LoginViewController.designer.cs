// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace iHere.iOS.LoginScreen
{
	[Register("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		Xamarin.Core.iOS.XamarinButton btnSignup { get; set; }

		[Outlet]
		UIKit.UIView viewLogo { get; set; }

		void ReleaseDesignerOutlets()
		{
			if (btnSignup != null)
			{
				btnSignup.Dispose();
				btnSignup = null;
			}

			if (viewLogo != null)
			{
				viewLogo.Dispose();
				viewLogo = null;
			}
		}
	}
}
