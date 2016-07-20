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
	[Register ("StoreProviderController")]
	partial class StoreProviderController
	{
		[Outlet]
		UIKit.UIPickerView StoreProviderPicker { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (StoreProviderPicker != null) {
				StoreProviderPicker.Dispose ();
				StoreProviderPicker = null;
			}
		}
	}
}
