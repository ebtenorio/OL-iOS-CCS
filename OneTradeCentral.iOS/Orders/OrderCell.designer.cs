// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("OrderCell")]
	partial class OrderCell
	{
		[Outlet]
		UIKit.UILabel orderNumberLabel { get; set; }

		[Outlet]
		UIKit.UILabel customerNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel orderDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel totalAmountLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (orderNumberLabel != null) {
				orderNumberLabel.Dispose ();
				orderNumberLabel = null;
			}

			if (customerNameLabel != null) {
				customerNameLabel.Dispose ();
				customerNameLabel = null;
			}

			if (orderDateLabel != null) {
				orderDateLabel.Dispose ();
				orderDateLabel = null;
			}

			if (totalAmountLabel != null) {
				totalAmountLabel.Dispose ();
				totalAmountLabel = null;
			}
		}
	}
}
