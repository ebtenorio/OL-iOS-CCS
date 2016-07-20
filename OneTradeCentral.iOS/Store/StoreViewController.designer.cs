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
	[Register ("StoreViewController")]
	partial class StoreViewController
	{
		[Outlet]
		UIKit.UITextField CompanyNameField { get; set; }

		[Outlet]
		UIKit.UITextField ContactFirstNameField { get; set; }

		[Outlet]
		UIKit.UITextField ContactLastNameField { get; set; }

		[Outlet]
		UIKit.UITextField ContactNumberField { get; set; }

		[Outlet]
		UIKit.UITextField EmailAddressField { get; set; }

		[Outlet]
		UIKit.UILabel StateLabel { get; set; }

		[Outlet]
		UIKit.UITextField StoreIDField { get; set; }

		[Action ("dismiss:")]
		partial void dismiss (Foundation.NSObject sender);

		[Action ("saveCustomerRecord:")]
		partial void saveCustomerRecord (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CompanyNameField != null) {
				CompanyNameField.Dispose ();
				CompanyNameField = null;
			}

			if (ContactFirstNameField != null) {
				ContactFirstNameField.Dispose ();
				ContactFirstNameField = null;
			}

			if (ContactLastNameField != null) {
				ContactLastNameField.Dispose ();
				ContactLastNameField = null;
			}

			if (ContactNumberField != null) {
				ContactNumberField.Dispose ();
				ContactNumberField = null;
			}

			if (EmailAddressField != null) {
				EmailAddressField.Dispose ();
				EmailAddressField = null;
			}

			if (StateLabel != null) {
				StateLabel.Dispose ();
				StateLabel = null;
			}

			if (StoreIDField != null) {
				StoreIDField.Dispose ();
				StoreIDField = null;
			}
		}
	}
}
