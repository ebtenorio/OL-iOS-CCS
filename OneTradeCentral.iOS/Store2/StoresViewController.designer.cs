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
	[Register ("StoresViewController")]
	partial class StoresViewController
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
		UIKit.UILabel ContactNumberFieldLabel { get; set; }

		[Outlet]
		UIKit.UITextField EmailAddressField { get; set; }

		[Outlet]
		UIKit.UILabel EmailAddressFieldLabel { get; set; }

		[Outlet]
		UIKit.UILabel FirstNameFieldLabel { get; set; }

		[Outlet]
		UIKit.UILabel LastNameFieldLabel { get; set; }

		[Outlet]
		UIKit.UITextField ProviderField { get; set; }

		[Outlet]
		UIKit.UILabel ProviderFieldLabel { get; set; }

		[Outlet]
		UIKit.UILabel StateLabel { get; set; }

		[Outlet]
		UIKit.UITextField StoreIDField { get; set; }

		[Outlet]
		UIKit.UILabel StoreIDFieldLabel { get; set; }

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

			if (ProviderField != null) {
				ProviderField.Dispose ();
				ProviderField = null;
			}

			if (StateLabel != null) {
				StateLabel.Dispose ();
				StateLabel = null;
			}

			if (StoreIDField != null) {
				StoreIDField.Dispose ();
				StoreIDField = null;
			}

			if (ProviderFieldLabel != null) {
				ProviderFieldLabel.Dispose ();
				ProviderFieldLabel = null;
			}

			if (StoreIDFieldLabel != null) {
				StoreIDFieldLabel.Dispose ();
				StoreIDFieldLabel = null;
			}

			if (FirstNameFieldLabel != null) {
				FirstNameFieldLabel.Dispose ();
				FirstNameFieldLabel = null;
			}

			if (LastNameFieldLabel != null) {
				LastNameFieldLabel.Dispose ();
				LastNameFieldLabel = null;
			}

			if (EmailAddressFieldLabel != null) {
				EmailAddressFieldLabel.Dispose ();
				EmailAddressFieldLabel = null;
			}

			if (ContactNumberFieldLabel != null) {
				ContactNumberFieldLabel.Dispose ();
				ContactNumberFieldLabel = null;
			}
		}
	}
}
