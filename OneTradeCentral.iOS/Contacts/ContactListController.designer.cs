// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("ContactListController")]
	partial class ContactListController
	{
		[Outlet]
		UIKit.UISearchBar SearchBar { get; set; }

		[Action ("dismiss:")]
		partial void dismiss (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (SearchBar != null) {
				SearchBar.Dispose ();
				SearchBar = null;
			}
		}
	}
}
