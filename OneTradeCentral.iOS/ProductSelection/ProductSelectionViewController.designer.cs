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
	[Register ("ProductSelectionViewController")]
	partial class ProductSelectionViewController
	{
		[Outlet]
		public UIKit.UISearchBar searchBar { get; set; }

		[Outlet]
		public UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (searchBar != null) {
				searchBar.Dispose ();
				searchBar = null;
			}

			if (TableView != null) {
				TableView.Dispose ();
				TableView = null;
			}
		}
	}
}
