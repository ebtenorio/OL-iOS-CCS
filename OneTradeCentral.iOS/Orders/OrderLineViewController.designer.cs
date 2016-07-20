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
	[Register ("OrderLineViewController")]
	partial class OrderLineViewController
	{
		[Outlet]
		UIKit.UIButton AddItemButton { get; set; }

		[Outlet]
		UIKit.UITextField OrderLineQuantityField { get; set; }

		[Outlet]
		UIKit.UILabel OrderLineSKULabel { get; set; }

		[Outlet]
		UIKit.UILabel ProductLabel { get; set; }

		[Outlet]
		UIKit.UILabel ProviderProductsLabel { get; set; }

		[Outlet]
		UIKit.UIStepper QuantityStepper { get; set; }

		[Action ("addItem:")]
		partial void addItem (Foundation.NSObject sender);

		[Action ("dismiss:")]
		partial void dismiss (Foundation.NSObject sender);

		[Action ("stepperAction:")]
		partial void stepperAction (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AddItemButton != null) {
				AddItemButton.Dispose ();
				AddItemButton = null;
			}

			if (OrderLineQuantityField != null) {
				OrderLineQuantityField.Dispose ();
				OrderLineQuantityField = null;
			}

			if (OrderLineSKULabel != null) {
				OrderLineSKULabel.Dispose ();
				OrderLineSKULabel = null;
			}

			if (ProductLabel != null) {
				ProductLabel.Dispose ();
				ProductLabel = null;
			}

			if (ProviderProductsLabel != null) {
				ProviderProductsLabel.Dispose ();
				ProviderProductsLabel = null;
			}

			if (QuantityStepper != null) {
				QuantityStepper.Dispose ();
				QuantityStepper = null;
			}
		}
	}
}
