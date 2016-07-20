
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace OneTradeCentral.iOS
{
	public class StoresListingCell : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("StoresListingCell");

		public StoresListingCell () : base (UITableViewCellStyle.Value1, Key)
		{
			// TODO: add subviews to the ContentView, set various colors, etc.
			TextLabel.Text = "TextLabel";
		}
	}
}

