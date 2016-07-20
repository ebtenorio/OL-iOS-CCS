using System;

using Foundation;
using UIKit;

using OneTradeCentral.DTOs;

namespace OneTradeCentral.iOS
{
	public partial class StoreViewController : UIViewController
	{

		public Customer Customer { get; set; }

		public StoreListController StoreListController { get; set; }
		
		private DALFacade DalFacade = new DALFacade();

		public StoreViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (Customer == null) {
				Customer = new Customer ();
				NavigationItem.Title = "New Customer";
			} else {
				// not needed by Wrigley's
//				SameDeliveryAddressSwitch.On = Customer.SameShipToAddress;
				NavigationItem.Title = "Edit Customer";
			}
			setFields ();

			AutoHideKeyboard ();
			
			// not needed by Wrigley's
//			SameDeliveryAddressSwitch.ValueChanged += (sender, e) => {
//				if ( SameDeliveryAddressSwitch.On == true ) {
//					Customer.SameShipToAddress = true;
//					ShipToButton.Enabled = false;
//					CopyBillToShipToAddress ();
//				} else {
//					Customer.SameShipToAddress = false;
//					ShipToButton.Enabled = true;
//				}
//				RefreshAddresses();
//			};

			StoreIDField.EditingDidEnd += (object sender, EventArgs e) => {
				if (StoreIDField.Text != null && StoreIDField.Text.Trim ().Length > 0) {
					var storeIDText = StoreIDField.Text.Trim ();
					var isValueUnsignedInteger = false;
					try {
						var storeIDIntValue = Convert.ToUInt64(storeIDText);
						// the edited store ID would also have to be persisted, so update the customer code
						StoreIDField.Text = Customer.Code = storeIDIntValue.ToString();
						isValueUnsignedInteger = true;
					} catch (Exception ex) {
						Customer.Code = null;
					}

					if (Customer.Code == null || Customer.Code.Length > 6) {
						StoreIDField.Text = "";
						Customer.Code = null;
						new UIAlertView("Store ID", "Store ID's should contain numbers with up to 6 digits only.", null, "OK", null).Show();
						((UITextField) sender).ResignFirstResponder();
						BeginInvokeOnMainThread ( () => {
							StoreIDField.BecomeFirstResponder();
						});
					}
				} else {
					StoreIDField.Text = Customer.Code = null;
				}
			};
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			// not needed by Wrigley's, see #101
//			if (SameDeliveryAddressSwitch.On == true) {
//				CopyBillToShipToAddress();
//				ShipToButton.Enabled = false;
//			} else {
//				ShipToButton.Enabled = true;
//			}
//			RefreshAddresses();
		}

		public void CopyBillToShipToAddress ()
		{
			Customer.ShipToStreet1 = Customer.BillToStreet1;
			Customer.ShipToStreet2 = Customer.BillToStreet2;
			Customer.ShipToCity = Customer.BillToCity;
			Customer.ShipToZipCode = Customer.BillToZipCode;
			Customer.ShipToCountry = Customer.BillToCountry;
		}

		void AutoHideKeyboard ()
		{
			CompanyNameField.ShouldReturn += ( field ) => {
				field.ResignFirstResponder ();
				return true;
			};
			ContactFirstNameField.ShouldReturn += ( field ) => {
				field.ResignFirstResponder ();
				return true;
			};
			ContactLastNameField.ShouldReturn += ( field ) => {
				field.ResignFirstResponder ();
				return true;
			};
			ContactNumberField.ShouldReturn += ( field ) => {
				field.ResignFirstResponder ();
				return true;
			};
			EmailAddressField.ShouldReturn += (field) => {
				field.ResignFirstResponder ();
				return true;
			};
		}

		// not needed by Wrigley's
//		partial void save (MonoTouch.Foundation.NSObject sender)
//		{
//			DALFacade dal = new DALFacade ();
//			// save customer data in DB
//			Customer.Name = CompanyNameField.Text;
//			Customer.ContactFirstName = ContactFirstNameField.Text;
//			Customer.ContactLastName = ContactLastNameField.Text;
//			Customer.ContactNumber = ContactNumberField.Text;
//			dal.SaveCustomer (Customer);
//
//			NavigationController.PopViewControllerAnimated (true);
//		}

		public void setFields ()
		{
			CompanyNameField.Text = Customer.Name;
			StoreIDField.Text = Customer.Code;
			ContactFirstNameField.Text = Customer.ContactFirstName;
			ContactLastNameField.Text = Customer.ContactLastName;
			EmailAddressField.Text = Customer.ContactEmail;
			ContactNumberField.Text = Customer.ContactNumber;
			// #132, store id now comes from user input
//			StoreIDLabel.Text = Customer.Code;
			StateLabel.Text = Customer.StateCode;
			// #101 - address not needed by Wrigley's
//			RefreshAddresses ();
		}
		
		partial void dismiss (Foundation.NSObject sender)
		{
			DismissViewController (true, null);
		}

		partial void saveCustomerRecord (Foundation.NSObject sender) {
			Customer.Code = StoreIDField.Text;
			Customer.ContactFirstName = ContactFirstNameField.Text;
			Customer.ContactLastName = ContactLastNameField.Text;
			Customer.ContactEmail = EmailAddressField.Text;
			Customer.ContactNumber = ContactNumberField.Text;
			DalFacade.SaveCustomer(Customer);
			StoreListController.TableView.ReloadData();
			DismissViewController(true, null);
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
			AddressEditorController addressEditor = segue.DestinationViewController as AddressEditorController;
			addressEditor.parentController = this;
			addressEditor.Customer = this.Customer;
			var address = new Address ();
			if (segue.Identifier == "BillToSegue") {
				addressEditor.AddressTypeText = "Billing Address";
				address.Street1 = Customer.BillToStreet1;
				address.Street2 = Customer.BillToStreet2;
				address.City = Customer.BillToCity;
				address.ZipCode = Customer.BillToZipCode;
				address.Country = Customer.BillToCountry;
			} else if (segue.Identifier == "ShipToSegue") {
				addressEditor.AddressTypeText = "Shipping Address";
				address.Street1 = Customer.ShipToStreet1;
				address.Street2 = Customer.ShipToStreet2;
				address.City = Customer.ShipToCity;
				address.ZipCode = Customer.ShipToZipCode;
				address.Country = Customer.ShipToCountry;
			}
			AddressEditorController.Address = address;
		}

		// Fix for #101.  Addresses are not needed by Wrigley's
//		public void RefreshAddresses ()
//		{			// address details are displayed per field in iPads
//			if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad) {
//				BillToStreet1Label.Text = Customer.BillToStreet1;
//				BillToStreet2Label.Text = Customer.BillToStreet2;
//				BillToCityLabel.Text = Customer.BillToCity;
//				BillToZipCodeLabel.Text = Customer.BillToZipCode;
//				BillToCountryLabel.Text = Customer.BillToCountry;
				
				// not needed by Wrigley's
//				if (SameDeliveryAddressSwitch.On == true)
//					CopyBillToShipToAddress();

//				ShipToStreet1Label.Text = Customer.ShipToStreet1;
//				ShipToStreet2Label.Text = Customer.ShipToStreet2;
//				ShipToCityLabel.Text = Customer.ShipToCity;
//				ShipToZipCodeLabel.Text = Customer.ShipToZipCode;
//				ShipToCountryLabel.Text = Customer.ShipToCountry;
//			} else {
				// address is combined in iPhones due to space restrictions
//				BillToFullAddress.Text = constructFullAddress(Customer.BillToStreet1, Customer.BillToStreet2,
//				                                         Customer.BillToCity, Customer.BillToZipCode, 
//				                                         Customer.BillToCountry);
//
//				ShipToFullAddress.Text = constructFullAddress(Customer.ShipToStreet1, Customer.ShipToStreet2,
//				                                         Customer.ShipToCity, Customer.ShipToZipCode,
//				                                         Customer.ShipToCountry);
//			}
//		}

		private string constructFullAddress (string street1, string street2, string city, string zipcode, string country)
		{
			var address = street1;
			if (street2 != null & street2 != "")
				address += ", " + street2;
			if (city != null & city != "")
				address += ", " + city;
			if (zipcode != null & zipcode != "")
				address += ".  " + zipcode;
			if (country != null & country != "")
				address += ".  " + country;
			return address;
		}
	}
}
