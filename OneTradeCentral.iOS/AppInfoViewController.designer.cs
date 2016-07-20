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
	[Register ("AppInfoViewController")]
	partial class AppInfoViewController
	{
		[Outlet]
		UIKit.UILabel CustomerCountLabel { get; set; }

		[Outlet]
		UIKit.UILabel iOSVersionLabel { get; set; }

		[Outlet]
		UIKit.UILabel MachineIDLabel { get; set; }

		[Outlet]
		UIKit.UILabel ModeLabel { get; set; }

		[Outlet]
		UIKit.UILabel OrderCountLabel { get; set; }

		[Outlet]
		UIKit.UILabel OrderLinesCountLabel { get; set; }

		[Outlet]
		UIKit.UILabel OrdersForUploadLabel { get; set; }

		[Outlet]
		UIKit.UILabel ProductCountLabel { get; set; }

		[Outlet]
		UIKit.UILabel ProductGroupCountLabel { get; set; }

		[Outlet]
		UIKit.UILabel ProviderCountLabel { get; set; }

		[Outlet]
		UIKit.UILabel RegisteredUserLabel { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem SyncBarButton { get; set; }

		[Outlet]
		UIKit.UILabel SyncDateLabel { get; set; }

		[Outlet]
		UIKit.UILabel SyncTimeLabel { get; set; }

		[Outlet]
		UIKit.UILabel UnsentTitleLabel { get; set; }

		[Outlet]
		UIKit.UIImageView UserLogo { get; set; }

		[Outlet]
		UIKit.UILabel VersionLabel { get; set; }

		[Outlet]
		UIKit.UILabel WareHouseLabel { get; set; }

		[Action ("ClearOrderTables:")]
		partial void ClearOrderTables (Foundation.NSObject sender);

		[Action ("SynchronizeData:")]
		partial void SynchronizeData (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CustomerCountLabel != null) {
				CustomerCountLabel.Dispose ();
				CustomerCountLabel = null;
			}

			if (iOSVersionLabel != null) {
				iOSVersionLabel.Dispose ();
				iOSVersionLabel = null;
			}

			if (MachineIDLabel != null) {
				MachineIDLabel.Dispose ();
				MachineIDLabel = null;
			}

			if (ModeLabel != null) {
				ModeLabel.Dispose ();
				ModeLabel = null;
			}

			if (OrderCountLabel != null) {
				OrderCountLabel.Dispose ();
				OrderCountLabel = null;
			}

			if (OrderLinesCountLabel != null) {
				OrderLinesCountLabel.Dispose ();
				OrderLinesCountLabel = null;
			}

			if (OrdersForUploadLabel != null) {
				OrdersForUploadLabel.Dispose ();
				OrdersForUploadLabel = null;
			}

			if (ProductCountLabel != null) {
				ProductCountLabel.Dispose ();
				ProductCountLabel = null;
			}

			if (ProductGroupCountLabel != null) {
				ProductGroupCountLabel.Dispose ();
				ProductGroupCountLabel = null;
			}

			if (ProviderCountLabel != null) {
				ProviderCountLabel.Dispose ();
				ProviderCountLabel = null;
			}

			if (RegisteredUserLabel != null) {
				RegisteredUserLabel.Dispose ();
				RegisteredUserLabel = null;
			}

			if (SyncBarButton != null) {
				SyncBarButton.Dispose ();
				SyncBarButton = null;
			}

			if (SyncDateLabel != null) {
				SyncDateLabel.Dispose ();
				SyncDateLabel = null;
			}

			if (SyncTimeLabel != null) {
				SyncTimeLabel.Dispose ();
				SyncTimeLabel = null;
			}

			if (UserLogo != null) {
				UserLogo.Dispose ();
				UserLogo = null;
			}

			if (VersionLabel != null) {
				VersionLabel.Dispose ();
				VersionLabel = null;
			}

			if (WareHouseLabel != null) {
				WareHouseLabel.Dispose ();
				WareHouseLabel = null;
			}

			if (UnsentTitleLabel != null) {
				UnsentTitleLabel.Dispose ();
				UnsentTitleLabel = null;
			}
		}
	}
}
