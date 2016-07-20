// This file has been autogenerated from parsing an Objective-C header file added in Xcode.

using System;
using CoreGraphics;

using Foundation;
using UIKit;
using ObjCRuntime;

namespace OneTradeCentral.iOS
{
	public partial class ProductBrowserPageController : UIPageViewController
	{
		private static Int32 PageIndex = 1;

		public ProductBrowserPageController (IntPtr handle) : base (handle)
		{
		}

		public override void LoadView ()
		{
			base.LoadView ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			ProductImageController pageZero = new ProductImageController (PageIndex);
			SetViewControllers (new UIViewController[]{ pageZero }, 
					UIPageViewControllerNavigationDirection.Forward, false, null);

			this.DataSource = new PageDataSource ();
		}

		private class PageDataSource : UIPageViewControllerDataSource
		{
			public override UIViewController GetNextViewController (UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				PageIndex ++;
				return new ProductImageController (PageIndex);
			}

			public override UIViewController GetPreviousViewController (UIPageViewController pageViewController, UIViewController referenceViewController)
			{
				PageIndex --;
				return new ProductImageController (PageIndex);
			}
		}
	}
}