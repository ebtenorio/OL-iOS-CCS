// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("OrderDetailsViewController")]
	partial class OrderDetailsViewController
	{
		[Outlet]
		UIKit.UIDatePicker requiredDatePicker { get; set; }

		[Outlet]
		UIKit.UIPickerView paymentTermsPicker { get; set; }

		[Action ("dismiss:")]
		partial void dismiss (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (requiredDatePicker != null) {
				requiredDatePicker.Dispose ();
				requiredDatePicker = null;
			}

			if (paymentTermsPicker != null) {
				paymentTermsPicker.Dispose ();
				paymentTermsPicker = null;
			}
		}
	}
}
