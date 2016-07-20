// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("ProductBrowserView")]
	partial class ProductBrowserView
	{
		[Outlet]
		UIKit.UIImageView ProductImage { get; set; }

		[Outlet]
		UIKit.UILabel ProductNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel VendorNameLabel { get; set; }

		[Outlet]
		UIKit.UILabel PriceLabel { get; set; }

		[Outlet]
		UIKit.UILabel UOMLabel { get; set; }

		[Outlet]
		UIKit.UILabel PageNumLabel { get; set; }

		[Outlet]
		UIKit.UILabel TotalPagesLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ProductImage != null) {
				ProductImage.Dispose ();
				ProductImage = null;
			}

			if (ProductNameLabel != null) {
				ProductNameLabel.Dispose ();
				ProductNameLabel = null;
			}

			if (VendorNameLabel != null) {
				VendorNameLabel.Dispose ();
				VendorNameLabel = null;
			}

			if (PriceLabel != null) {
				PriceLabel.Dispose ();
				PriceLabel = null;
			}

			if (UOMLabel != null) {
				UOMLabel.Dispose ();
				UOMLabel = null;
			}

			if (PageNumLabel != null) {
				PageNumLabel.Dispose ();
				PageNumLabel = null;
			}

			if (TotalPagesLabel != null) {
				TotalPagesLabel.Dispose ();
				TotalPagesLabel = null;
			}
		}
	}
}
