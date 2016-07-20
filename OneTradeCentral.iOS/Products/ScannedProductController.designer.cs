// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("ScannedProductController")]
	partial class ScannedProductController
	{
		[Outlet]
		UIKit.UILabel ProductNameLabel { get; set; }

		[Outlet]
		UIKit.UIImageView ProductImageView { get; set; }

		[Outlet]
		UIKit.UILabel VendorNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel BarcodeIDLabel { get; set; }

		[Outlet]
		UIKit.UILabel SKULabel { get; set; }

		[Outlet]
		UIKit.UILabel UnitPriceLabel { get; set; }

		[Outlet]
		UIKit.UILabel AmountLabel { get; set; }

		[Outlet]
		UIKit.UITextField QuantityField { get; set; }

		[Outlet]
		UIKit.UIStepper QuantityStepper { get; set; }

		[Action ("stepperAction:")]
		partial void stepperAction (Foundation.NSObject sender);

		[Action ("addItem:")]
		partial void addItem (Foundation.NSObject sender);

		[Action ("dismiss:")]
		partial void dismiss (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ProductNameLabel != null) {
				ProductNameLabel.Dispose ();
				ProductNameLabel = null;
			}

			if (ProductImageView != null) {
				ProductImageView.Dispose ();
				ProductImageView = null;
			}

			if (VendorNameLabel != null) {
				VendorNameLabel.Dispose ();
				VendorNameLabel = null;
			}

			if (BarcodeIDLabel != null) {
				BarcodeIDLabel.Dispose ();
				BarcodeIDLabel = null;
			}

			if (SKULabel != null) {
				SKULabel.Dispose ();
				SKULabel = null;
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
