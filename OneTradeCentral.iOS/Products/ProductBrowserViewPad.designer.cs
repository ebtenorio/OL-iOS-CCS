// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("ProductBrowserViewPad")]
	partial class ProductBrowserViewPad
	{
		[Outlet]
		UIKit.UILabel PageNumLabel { get; set; }

		[Outlet]
		UIKit.UILabel TotalPagesLabel { get; set; }

		[Outlet]
		UIKit.UILabel ProductNameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PageNumLabel != null) {
				PageNumLabel.Dispose ();
				PageNumLabel = null;
			}

			if (TotalPagesLabel != null) {
				TotalPagesLabel.Dispose ();
				TotalPagesLabel = null;
			}

			if (ProductNameLabel != null) {
				ProductNameLabel.Dispose ();
				ProductNameLabel = null;
			}
		}
	}
}
