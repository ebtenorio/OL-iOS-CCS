// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the Xcode designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;

namespace OneTradeCentral.iOS
{
	[Register ("WarehouseController")]
	partial class WarehouseController
	{
		[Outlet]
		UIKit.UIPickerView WarehousePicker { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (WarehousePicker != null) {
				WarehousePicker.Dispose ();
				WarehousePicker = null;
			}
		}
	}
}
