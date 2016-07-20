// This file has been autogenerated from parsing an Objective-C header file added in Xcode.

using System;

using Foundation;
using UIKit;

using OneTradeCentral.DTOs;

namespace OneTradeCentral.iOS
{
	public partial class OrderLineInfoController : UIViewController
	{
		public OrderViewController OrderViewController { get; set; }
		public OrderLine OrderLineItem { get; set; }
		Product SelectedProduct;
		Int32 Quantity;

		public OrderLineInfoController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			SelectedProduct = OrderLineItem.Product;
			Quantity = OrderLineItem.Quantity;
			if (SelectedProduct != null && Quantity > 0) {
				ProductNameField.Text = SelectedProduct.Name;
//				BarcodeLabel.Text = SelectedProduct.BarcodeID;
				UOMLabel.Text = SelectedProduct.UOM;
//				UnitPriceLabel.Text = String.Format ("{0:C2}", SelectedProduct.UnitPrice);
				QuantityStepper.Value = Quantity;
//				setAmountFields ();
			}

			if (OrderViewController == null) {
				QuantityStepper.Hidden = true;
//				QuantityStepper.Enabled = false;
			}
		}

//		void setAmountFields ()
//		{
//			AmountLabel.Text = String.Format ("{0:C2}", Quantity * SelectedProduct.UnitPrice);
//			QuantityField.Text = String.Format ("{0}", Quantity);
//		}

		partial void StepperValueChanged (Foundation.NSObject sender) {
			Quantity = Convert.ToInt32(QuantityStepper.Value);
//			setAmountFields ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			if (OrderViewController != null)
				OrderViewController.AddOrderLineItem (SelectedProduct, Quantity);
			base.ViewWillDisappear (animated);
		}

	}
}