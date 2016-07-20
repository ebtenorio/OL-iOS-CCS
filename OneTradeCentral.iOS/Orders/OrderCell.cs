
using System;
using CoreGraphics;

using Foundation;
using UIKit;

using OneTradeCentral.DTOs;

namespace OneTradeCentral.iOS
{
	public partial class OrderCell : UITableViewCell
	{
		public static readonly UINib Nib = UINib.FromName ("OrderCell", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("OrderCell");
		
//		UILabel headingLabel, subheadingLabel;
//		UIImageView imageView;

		Order _order;
		
		public OrderCell (NSString cellId) : base (UITableViewCellStyle.Value1, cellId)
		{
		}

		public Order Order {
			get {
				return _order;
			}
			set {
				this._order = value;
			}
		}
		
		public static OrderCell Create ()
		{
			return (OrderCell)Nib.Instantiate (null, null) [0];
		}
	}
}

