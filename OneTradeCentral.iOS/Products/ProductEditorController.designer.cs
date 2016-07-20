// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("ProductEditorController")]
	partial class ProductEditorController
	{
		[Outlet]
		UIKit.UITextField CategoryField { get; set; }

		[Outlet]
		UIKit.UITextField ProductCodeField { get; set; }

		[Outlet]
		UIKit.UITextField ProductNameField { get; set; }

		[Outlet]
		UIKit.UITextField SKUField { get; set; }

		[Outlet]
		UIKit.UITextField UOMField { get; set; }

		[Action ("dismiss:")]
		partial void dismiss (Foundation.NSObject sender);

		[Action ("save:")]
		partial void save (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CategoryField != null) {
				CategoryField.Dispose ();
				CategoryField = null;
			}

			if (ProductCodeField != null) {
				ProductCodeField.Dispose ();
				ProductCodeField = null;
			}

			if (ProductNameField != null) {
				ProductNameField.Dispose ();
				ProductNameField = null;
			}

			if (SKUField != null) {
				SKUField.Dispose ();
				SKUField = null;
			}

			if (UOMField != null) {
				UOMField.Dispose ();
				UOMField = null;
			}
		}
	}
}
