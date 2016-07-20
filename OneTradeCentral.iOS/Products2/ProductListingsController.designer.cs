// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace OneTradeCentral.iOS
{
	[Register ("ProductListingsController")]
	partial class ProductListingsController
	{
		[Outlet]
		public MonoTouch.UIKit.UITextField ProductProvider { get; set; }

		[Outlet]
		public MonoTouch.UIKit.UISearchBar searchBar { get; set; }

		[Outlet]
		public MonoTouch.UIKit.UITableView TableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ProductProvider != null) {
				ProductProvider.Dispose ();
				ProductProvider = null;
			}

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
