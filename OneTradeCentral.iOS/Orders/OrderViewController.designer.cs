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
	[Register ("OrderViewController")]
	partial class OrderViewController
	{
		[Outlet]
		UIKit.UITextField _ReleaseDateField { get; set; }

		[Outlet]
		UIKit.UIButton ContactLookupButton { get; set; }

		[Outlet]
		UIKit.UITextField CustomerNameField { get; set; }

		[Outlet]
		UIKit.UITextField HoldDateField { get; set; }

		[Outlet]
		UIKit.UITableView OrderDetailTableView { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem OrderLineAddButton { get; set; }

		[Outlet]
		UIKit.UIBarButtonItem OrderLineDeleteButton { get; set; }

		[Outlet]
		UIKit.UISegmentedControl OrderTypeSegment { get; set; }

		[Outlet]
		UIKit.UIButton ProviderButton { get; set; }

		[Outlet]
		UIKit.UITextField ProviderField { get; set; }

		[Outlet]
		UIKit.UIButton ReleaseDateButton { get; set; }

		[Outlet]
		public UIKit.UITextField ReleaseDateField { get; private set; }

		[Outlet]
		UIKit.UILabel StateLabel { get; set; }

		[Outlet]
		UIKit.UITextField StoreIDField { get; set; }

		[Outlet]
		UIKit.UITextField StoreMgrEmailField { get; set; }

		[Outlet]
		UIKit.UITextField StoreMgrFirstNameField { get; set; }

		[Outlet]
		UIKit.UITextField StoreMgrLastNameField { get; set; }

		[Outlet]
		UIKit.UITextField StoreMgrPhoneField { get; set; }

		[Outlet]
		UIKit.UIButton StoreSelectionButton { get; set; }

		[Outlet]
		UIKit.UIButton WarehouseButton { get; set; }

		[Outlet]
		UIKit.UITextField WarehouseField { get; set; }

		[Action ("newOrder:")]
		partial void newOrder (Foundation.NSObject sender);

		[Action ("saveOrder:")]
		partial void saveOrder (Foundation.NSObject sender);

		[Action ("sendOrder:")]
		partial void sendOrder (Foundation.NSObject sender);

		[Action ("switchEditMode:")]
		partial void switchEditMode (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (_ReleaseDateField != null) {
				_ReleaseDateField.Dispose ();
				_ReleaseDateField = null;
			}

			if (ContactLookupButton != null) {
				ContactLookupButton.Dispose ();
				ContactLookupButton = null;
			}

			if (CustomerNameField != null) {
				CustomerNameField.Dispose ();
				CustomerNameField = null;
			}

			if (HoldDateField != null) {
				HoldDateField.Dispose ();
				HoldDateField = null;
			}

			if (OrderDetailTableView != null) {
				OrderDetailTableView.Dispose ();
				OrderDetailTableView = null;
			}

			if (OrderLineAddButton != null) {
				OrderLineAddButton.Dispose ();
				OrderLineAddButton = null;
			}

			if (OrderLineDeleteButton != null) {
				OrderLineDeleteButton.Dispose ();
				OrderLineDeleteButton = null;
			}

			if (OrderTypeSegment != null) {
				OrderTypeSegment.Dispose ();
				OrderTypeSegment = null;
			}

			if (ProviderButton != null) {
				ProviderButton.Dispose ();
				ProviderButton = null;
			}

			if (ProviderField != null) {
				ProviderField.Dispose ();
				ProviderField = null;
			}

			if (ReleaseDateButton != null) {
				ReleaseDateButton.Dispose ();
				ReleaseDateButton = null;
			}

			if (ReleaseDateField != null) {
				ReleaseDateField.Dispose ();
				ReleaseDateField = null;
			}

			if (StateLabel != null) {
				StateLabel.Dispose ();
				StateLabel = null;
			}

			if (StoreIDField != null) {
				StoreIDField.Dispose ();
				StoreIDField = null;
			}

			if (StoreMgrEmailField != null) {
				StoreMgrEmailField.Dispose ();
				StoreMgrEmailField = null;
			}

			if (StoreMgrFirstNameField != null) {
				StoreMgrFirstNameField.Dispose ();
				StoreMgrFirstNameField = null;
			}

			if (StoreMgrLastNameField != null) {
				StoreMgrLastNameField.Dispose ();
				StoreMgrLastNameField = null;
			}

			if (StoreMgrPhoneField != null) {
				StoreMgrPhoneField.Dispose ();
				StoreMgrPhoneField = null;
			}

			if (WarehouseButton != null) {
				WarehouseButton.Dispose ();
				WarehouseButton = null;
			}

			if (WarehouseField != null) {
				WarehouseField.Dispose ();
				WarehouseField = null;
			}

			if (StoreSelectionButton != null) {
				StoreSelectionButton.Dispose ();
				StoreSelectionButton = null;
			}
		}
	}
}
