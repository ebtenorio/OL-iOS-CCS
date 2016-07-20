// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("AddressEditorController")]
	partial class AddressEditorController
	{
		[Outlet]
		UIKit.UILabel AddressTypeLabel { get; set; }

		[Outlet]
		UIKit.UITextField Street1Field { get; set; }

		[Outlet]
		UIKit.UITextField Street2Field { get; set; }

		[Outlet]
		UIKit.UITextField CityField { get; set; }

		[Outlet]
		UIKit.UITextField ZipCodeField { get; set; }

		[Outlet]
		UIKit.UIPickerView CountryPicker { get; set; }

		[Action ("dismiss:")]
		partial void dismiss (Foundation.NSObject sender);

		[Action ("saveAddress:")]
		partial void saveAddress (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AddressTypeLabel != null) {
				AddressTypeLabel.Dispose ();
				AddressTypeLabel = null;
			}

			if (Street1Field != null) {
				Street1Field.Dispose ();
				Street1Field = null;
			}

			if (Street2Field != null) {
				Street2Field.Dispose ();
				Street2Field = null;
			}

			if (CityField != null) {
				CityField.Dispose ();
				CityField = null;
			}

			if (ZipCodeField != null) {
				ZipCodeField.Dispose ();
				ZipCodeField = null;
			}

			if (CountryPicker != null) {
				CountryPicker.Dispose ();
				CountryPicker = null;
			}
		}
	}
}
