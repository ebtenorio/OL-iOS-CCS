using System;
using Foundation;
using UIKit;
using TimesSquare.iOS;

namespace OneTradeCentral.iOS
{
	[Register ("CalendarRowCell")]
	public class CalendarRowCell : TSQCalendarRowCell
	{
		public CalendarRowCell ()
		{
		}

		public CalendarRowCell (IntPtr handler) : base (handler)
		{
		}

		public override void LayoutViews (nuint index, CoreGraphics.CGRect rect)
		{
			rect.Y += ColumnSpacing;
			rect.Height -= BottomRow ? 2.0f : 1.0f * ColumnSpacing;
			base.LayoutViews (index, rect);
		}

		public override UIImage TodayBackgroundImage {
			get {
				var uIImage = UIImage.FromBundle ("CalendarTodaysDate.png");
				return uIImage.StretchableImage (4, 4);
			}
		}

		public override UIImage SelectedBackgroundImage {
			get {
				return UIImage.FromBundle ("CalendarSelectedDate.png").StretchableImage (4, 4);
			}
		}

		public override UIImage NotThisMonthBackgroundImage {
			get {
				return UIImage.FromBundle ("CalendarPreviousMonth.png").StretchableImage (0, 0);
			}
		}

		public override UIImage BackgroundImage {
			get {
				return UIImage.FromBundle ( BottomRow ? "CalendarRowBottom.png" : "CalendarRow.png");
			}
		}
	}
}

