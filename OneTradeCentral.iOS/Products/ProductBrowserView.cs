using System;
using CoreGraphics;

using UIKit;
using Foundation;
using ObjCRuntime;

using OneTradeCentral.DTOs;

namespace OneTradeCentral.iOS
{
	public partial class ProductBrowserView : UIView
	{
		public Product Product { get; set; }

		public ProductBrowserView(IntPtr h) : base(h)
		{
		}

		public ProductBrowserView ()
		{
			var arr = NSBundle.MainBundle.LoadNib ("ProductBrowserView", this, null);
			var view = Runtime.GetNSObject (arr.ValueAt(0)) as UIView;
			view.Frame = new CGRect (0, 0, Frame.Width, Frame.Height);
			AddSubview (view);
		}
	}
}

