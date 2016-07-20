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
	[Register ("StoreListingsController")]
	partial class StoreListingsController
	{
		[Outlet]
		public UIKit.UISearchBar SearchBar { get; private set; }

		[Outlet]
		public UIKit.UITextField StoreProvider { get; private set; }

		[Outlet]
		public UIKit.UITableView TableView { get; private set; }

		[Action ("doubleTap:")]
		partial void doubleTap (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (SearchBar != null) {
				SearchBar.Dispose ();
				SearchBar = null;
			}

			if (StoreProvider != null) {
				StoreProvider.Dispose ();
				StoreProvider = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
