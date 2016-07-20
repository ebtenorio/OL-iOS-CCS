using System;
using CoreGraphics;

using UIKit;
using Foundation;
using ObjCRuntime;

namespace OneTradeCentral.iOS
{
	public partial class ProductBrowserViewPad : UIView
	{
		public ProductBrowserViewPad (IntPtr h) : base (h)
		{
		}

		public ProductBrowserViewPad()
		{
			var arr = NSBundle.MainBundle.LoadNib ("ProductBrowserViewPad", this, null);
			var view = Runtime.GetNSObject (arr.ValueAt(0)) as UIView;
			view.Frame = new CGRect (0, 0, Frame.Width, Frame.Height);
			AddSubview (view);
		}

		public void SetPageNumber(Int32 pageNum)
		{
			PageNumLabel.Text = pageNum.ToString ();
		}
	}
}

