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
	[Register ("ProductsListingsController")]
	partial class ProductsListingsController
	{
		[Outlet]
		public UIKit.UITextField ProductProvider { get; private set; }

		[Outlet]
		public UIKit.UISearchBar SearchBar { get; private set; }

		[Outlet]
		public UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ProductProvider != null) {
				ProductProvider.Dispose ();
				ProductProvider = null;
			}

			if (SearchBar != null) {
				SearchBar.Dispose ();
				SearchBar = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
