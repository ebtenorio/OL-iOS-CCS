// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("OrderLineInfoController")]
	partial class OrderLineInfoController
	{
		[Outlet]
		UIKit.UITextField ProductNameField { get; set; }

		[Outlet]
		UIKit.UILabel BarcodeLabel { get; set; }

		[Outlet]
		UIKit.UILabel UOMLabel { get; set; }

		[Outlet]
		UIKit.UILabel UnitPriceLabel { get; set; }

		[Outlet]
		UIKit.UILabel AmountLabel { get; set; }

		[Outlet]
		UIKit.UITextField QuantityField { get; set; }

		[Outlet]
		UIKit.UIStepper QuantityStepper { get; set; }

		[Action ("StepperValueChanged:")]
		partial void StepperValueChanged (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ProductNameField != null) {
				ProductNameField.Dispose ();
				ProductNameField = null;
			}

			if (BarcodeLabel != null) {
				BarcodeLabel.Dispose ();
				BarcodeLabel = null;
			}

			if (UOMLabel != null) {
				UOMLabel.Dispose ();
				UOMLabel = null;
			}

			if (UnitPriceLabel != null) {
				UnitPriceLabel.Dispose ();
				UnitPriceLabel = null;
			}

			if (AmountLabel != null) {
				AmountLabel.Dispose ();
				AmountLabel = null;
			}

			if (QuantityField != null) {
				QuantityField.Dispose ();
				QuantityField = null;
			}

			if (QuantityStepper != null) {
				QuantityStepper.Dispose ();
				QuantityStepper = null;
			}
		}
	}
}
