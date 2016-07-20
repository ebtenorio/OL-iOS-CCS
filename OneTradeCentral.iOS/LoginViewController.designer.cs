// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace OneTradeCentral.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		UIKit.UIToolbar loginToolBar { get; set; }

		[Outlet]
		UIKit.UITextField PasswordField { get; set; }

		[Outlet]
		UIKit.UITextField UserNameField { get; set; }

		[Action ("Cancel:")]
		partial void Cancel (Foundation.NSObject sender);

		[Action ("Register:")]
		partial void Register (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (loginToolBar != null) {
				loginToolBar.Dispose ();
				loginToolBar = null;
			}

			if (PasswordField != null) {
				PasswordField.Dispose ();
				PasswordField = null;
			}

			if (UserNameField != null) {
				UserNameField.Dispose ();
				UserNameField = null;
			}
		}
	}
}
