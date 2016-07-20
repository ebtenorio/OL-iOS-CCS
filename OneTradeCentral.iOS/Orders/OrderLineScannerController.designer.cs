// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("OrderLineScannerController")]
	partial class OrderLineScannerController
	{
		[Outlet]
		UIKit.UINavigationBar NavigationBar { get; set; }

		[Action ("dismiss:")]
		partial void dismiss (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (NavigationBar != null) {
				NavigationBar.Dispose ();
				NavigationBar = null;
			}
		}
	}
}
