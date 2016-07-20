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
	[Register ("OrderConfirmationController")]
	partial class OrderConfirmationController
	{
		[Outlet]
		UIKit.UITextField CompanyNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel HoldDateLabel { get; set; }

		[Outlet]
		UIKit.UITableView OrderConfirmationTableView { get; set; }

		[Outlet]
		UIKit.UILabel OrderDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel ProviderLabel { get; set; }

		[Outlet]
		UIKit.UILabel ReleaseDateLabel { get; set; }

		[Outlet]
		OneTradeCentral.iOS.SignatureView SignatureView { get; set; }

		[Outlet]
		UIKit.UILabel StateLabel { get; set; }

		[Outlet]
		UIKit.UILabel StoreIDLabel { get; set; }

		[Outlet]
		UIKit.UILabel StoreMgrName { get; set; }

		[Outlet]
		UIKit.UILabel WarehouseLabel { get; set; }

		[Action ("sendOrder:")]
		partial void sendOrder (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CompanyNameLabel != null) {
				CompanyNameLabel.Dispose ();
				CompanyNameLabel = null;
			}

			if (HoldDateLabel != null) {
				HoldDateLabel.Dispose ();
				HoldDateLabel = null;
			}

			if (OrderConfirmationTableView != null) {
				OrderConfirmationTableView.Dispose ();
				OrderConfirmationTableView = null;
			}

			if (OrderDateLabel != null) {
				OrderDateLabel.Dispose ();
				OrderDateLabel = null;
			}

			if (ProviderLabel != null) {
				ProviderLabel.Dispose ();
				ProviderLabel = null;
			}

			if (SignatureView != null) {
				SignatureView.Dispose ();
				SignatureView = null;
			}

			if (StateLabel != null) {
				StateLabel.Dispose ();
				StateLabel = null;
			}

			if (StoreIDLabel != null) {
				StoreIDLabel.Dispose ();
				StoreIDLabel = null;
			}

			if (StoreMgrName != null) {
				StoreMgrName.Dispose ();
				StoreMgrName = null;
			}

			if (WarehouseLabel != null) {
				WarehouseLabel.Dispose ();
				WarehouseLabel = null;
			}

			if (ReleaseDateLabel != null) {
				ReleaseDateLabel.Dispose ();
				ReleaseDateLabel = null;
			}
		}
	}
}
