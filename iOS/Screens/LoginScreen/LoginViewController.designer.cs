// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace iHere.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		Xamarin.Core.iOS.XamarinButton btnSignUp { get; set; }

		[Outlet]
		UIKit.UIView viewLogo { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnSignUp != null) {
				btnSignUp.Dispose ();
				btnSignUp = null;
			}

			if (viewLogo != null) {
				viewLogo.Dispose ();
				viewLogo = null;
			}
		}
	}
}
