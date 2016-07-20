// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace OneTradeCentral.iOS
{
	[Register ("HoldDateController")]
	partial class HoldDateController
	{
		[Outlet]
		UIKit.UIDatePicker HoldDatePicker { get; set; }

		[Action ("ClearHoldDate:")]
		partial void ClearHoldDate (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (HoldDatePicker != null) {
				HoldDatePicker.Dispose ();
				HoldDatePicker = null;
			}
		}
	}
}
