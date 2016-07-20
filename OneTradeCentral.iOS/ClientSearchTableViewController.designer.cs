// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("ClientSearchTableViewController")]
	partial class ClientSearchTableViewController
	{
		[Outlet]
		MonoTouch.UIKit.UITableView datasource { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (datasource != null) {
				datasource.Dispose ();
				datasource = null;
			}
		}
	}
}
