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
	[Register ("StoreListController")]
	partial class StoreListController
	{
		[Outlet]
		UIKit.UISearchBar searchBar { get; set; }

		[Action ("dismiss:")]
		partial void dismiss (Foundation.NSObject sender);

		[Action ("doubleTap:")]
		partial void doubleTap (Foundation.NSObject sender);

		[Action ("save:")]
		partial void save (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (searchBar != null) {
				searchBar.Dispose ();
				searchBar = null;
			}
		}
	}
}
