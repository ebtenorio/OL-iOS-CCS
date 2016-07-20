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
	[Register ("OrderInfoViewController")]
	partial class OrderInfoViewController
	{
		[Outlet]
		UIKit.UITextField CustomerNameField { get; set; }

		[Outlet]
		UIKit.UILabel HoldDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel OrderDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel OrderNumberLabel { get; set; }

		[Outlet]
		UIKit.UILabel ProviderLabel { get; set; }

		[Outlet]
		UIKit.UILabel ReleaseDateLabel { get; set; }

		[Outlet]
		UIKit.UIImageView SignatureImageView { get; set; }

		[Outlet]
		UIKit.UILabel StateLabel { get; set; }

		[Outlet]
		UIKit.UILabel StoreIDLabel { get; set; }

		[Outlet]
		UIKit.UILabel StoreMgrEmailLabel { get; set; }

		[Outlet]
		UIKit.UILabel StoreMgrFullNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel StoreMgrPhoneLabel { get; set; }

		[Outlet]
		UIKit.UITableView TableView { get; set; }

		[Outlet]
		UIKit.UILabel WarehouseLabel { get; set; }

		[Action ("dismiss:")]
		partial void dismiss (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CustomerNameField != null) {
				CustomerNameField.Dispose ();
				CustomerNameField = null;
			}

			if (HoldDateLabel != null) {
				HoldDateLabel.Dispose ();
				HoldDateLabel = null;
			}

			if (OrderDateLabel != null) {
				OrderDateLabel.Dispose ();
				OrderDateLabel = null;
			}

			if (OrderNumberLabel != null) {
				OrderNumberLabel.Dispose ();
				OrderNumberLabel = null;
			}

			if (ProviderLabel != null) {
				ProviderLabel.Dispose ();
				ProviderLabel = null;
			}

			if (SignatureImageView != null) {
				SignatureImageView.Dispose ();
				SignatureImageView = null;
			}

			if (StateLabel != null) {
				StateLabel.Dispose ();
				StateLabel = null;
			}

			if (StoreIDLabel != null) {
				StoreIDLabel.Dispose ();
				StoreIDLabel = null;
			}

			if (StoreMgrEmailLabel != null) {
				StoreMgrEmailLabel.Dispose ();
				StoreMgrEmailLabel = null;
			}

			if (StoreMgrFullNameLabel != null) {
				StoreMgrFullNameLabel.Dispose ();
				StoreMgrFullNameLabel = null;
			}

			if (StoreMgrPhoneLabel != null) {
				StoreMgrPhoneLabel.Dispose ();
				StoreMgrPhoneLabel = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
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
