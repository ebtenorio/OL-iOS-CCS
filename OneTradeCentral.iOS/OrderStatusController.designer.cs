// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("OrderStatusController")]
	partial class OrderStatusController
	{
		[Outlet]
		UIKit.UILabel CompletedCountLabel { get; set; }

		[Outlet]
		UIKit.UILabel PartialCountLabel { get; set; }

		[Outlet]
		UIKit.UILabel PendingCountLabel { get; set; }

		[Outlet]
		UIKit.UILabel TotalCountLabel { get; set; }

		[Action ("RefreshStats:")]
		partial void RefreshStats (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CompletedCountLabel != null) {
				CompletedCountLabel.Dispose ();
				CompletedCountLabel = null;
			}

			if (PartialCountLabel != null) {
				PartialCountLabel.Dispose ();
				PartialCountLabel = null;
			}

			if (PendingCountLabel != null) {
				PendingCountLabel.Dispose ();
				PendingCountLabel = null;
			}

			if (TotalCountLabel != null) {
				TotalCountLabel.Dispose ();
				TotalCountLabel = null;
			}
		}
	}
}
