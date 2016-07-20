using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AddressBook;
using UIKit;
using AssetsLibrary;
using OneTradeCentral.DTOs;
using BigTed;

namespace OneTradeCentral.iOS
{
	public partial class OrderViewController : UIViewController
	{
		private Boolean _edited = false;

		public UIInterfaceOrientation deviceOrientation = (UIInterfaceOrientation) UIDevice.CurrentDevice.Orientation;

		public DatePickerController datePickerPopover;

		public NSDate RequestedReleaseDate{ get; set; }

		public Constants.ORDERTYPE OrderType { get; set; }

		public ProductSelectionSource productSelectionSource = new ProductSelectionSource ();


		public Boolean IsEdited {
			get {
				return _edited;
			}
		}

		Order Order { get; set; }
		//		IList<OrderLine> OrderLineItems { get; set; }
		static OrderLine SelectedOrderLine { get; set; }
		static Product SelectedProduct { get; set; }
		static int SelectedProductQuantity { get; set; }
		static WebServiceFacade webservice;
		static DALFacade dalFacade;

		public OrderViewController (IntPtr handle) : base (handle)
		{
			Order = new Order ();
			if (dalFacade == null)
				dalFacade = new DALFacade ();
		}

		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			base.WillRotate (toInterfaceOrientation, duration);
			this.ResizePresentedViewController (this.PresentedViewController, toInterfaceOrientation);

		}


		public void ResizePresentedViewController(UIViewController presentedViewController, UIInterfaceOrientation toInterfaceOrientation){

			if (presentedViewController is OrderLineViewController) {
				((OrderLineViewController)presentedViewController).ViewDidLayoutSubviews ();
				((OrderLineViewController)presentedViewController).ResizeViewController ();
			}
		}
			
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.Order.RequestedReleaseDate = (DateTime)NSDate.Now;
			this.datePickerPopover = new DatePickerController(this);


			OrderDetailTableView.Source = new OrderDetailSource (Order.OrderLineList, ProviderButton, OrderTypeSegment, ReleaseDateButton);

			if (webservice == null)
				webservice = new WebServiceFacade ();

			StoreIDField.EditingChanged += (object sender, EventArgs e) => {
				if (StoreIDField.Text != null && StoreIDField.Text.Trim ().Length > 0) {
					Order.Customer.Code = Order.StoreID = StoreIDField.Text.Trim() ;
				}
				else{
					Order.Customer.Code = Order.StoreID = "";
				}

			};

//			this.ReleaseDateField.EditingDidBegin += (object sender, EventArgs e) => {
//				this.ReleaseDateField.ResignFirstResponder();
//				this.showDatePicker();
//			}; 
				
		

//			// Default Order Type to Regular Order
//			// 1 - Regular
//			// 2 - Pre-sell
//			// Please refer to UI regarding the segment ID.
//
			this.OrderTypeSegment.SelectedSegment = 0;
			this.ReleaseDateField.Text = "Today";

			OrderTypeSegment.ValueChanged += (object sender, EventArgs e) => {
			
				// If the OrderLineList is not empty, disable the Order Type selection segment
				if (this.OrderTypeSegment.SelectedSegment == (int)Constants.ORDERTYPE.REGULAR) {
					this.ReleaseDateField.Enabled = false;
					this.ReleaseDateButton.Enabled = true;
					this.ReleaseDateField.Text = "Today";
					this.RequestedReleaseDate = NSDate.Now;
					this.OrderType = Constants.ORDERTYPE.REGULAR;

				} else if (this.OrderTypeSegment.SelectedSegment == (int)Constants.ORDERTYPE.PRESELL) {
					this.ReleaseDateField.Text = "";
					this.ReleaseDateField.Enabled = false;
					this.ReleaseDateButton.Enabled = false;
					this.RequestedReleaseDate = null;
					this.OrderType = Constants.ORDERTYPE.PRESELL;

					if(dalFacade.CountFutureProducts() == 0){
						new UIAlertView("Pre-sell Order","Cannot create pre-sell order.\nNo Products exist with a future Start date.", null, "OK").Show();
						this.StoreSelectionButton.Enabled = true;
						this.ReleaseDateButton.Enabled = false;
						this.OrderTypeSegment.SelectedSegment  = 0;
						this.ReleaseDateField.Text = "Today";	
						this.OrderType = Constants.ORDERTYPE.REGULAR;
					}
					else{
						this.StoreSelectionButton.Enabled = true;
					}
						
				}

				if(this.ReleaseDateField.Text.Trim() == ""){
					//this.Order.RequestedReleaseDate = null;
				}
				else if(this.ReleaseDateField.Text.ToUpper() == "TODAY"){
					this.Order.RequestedReleaseDate = DateTime.Now;
				}
				else{
					this.Order.RequestedReleaseDate = (DateTime) this.Order.RequestedReleaseDate;
				}

			};


			StoreIDField.Ended += (object sender, EventArgs e) => {
				if (StoreIDField.Text != null && StoreIDField.Text.Trim ().Length > 0) {
					var storeIDText = StoreIDField.Text.Trim ();

					_edited = true;
					// the edited store ID would also have to be persisted, so update the customer code
					StoreIDField.Text = Order.Customer.Code = Order.StoreID = storeIDText;


					// Uncomnented by Eldon for Testing
//					if (ProviderField.Text != null && ProviderField.Text.Trim() != "") {
//						CheckForDefaultProvider (Order.CustomerID);						
//					}
//
//					if (storeIDText.Length < 5 || storeIDText.Length > 7) {
//						new UIAlertView("Store ID", "Store ID's should be between 5 to 7 characters.", null, "OK", null).Show();
//						StoreIDField.Text = Order.StoreID;  // revert the displayed value if it fails validation
//						((UITextField) sender).ResignFirstResponder();
//						BeginInvokeOnMainThread ( () => {
//							StoreIDField.BecomeFirstResponder();
//						});
//					} else {
//						_edited = true;
//					//	 the edited store ID would also have to be persisted, so update the customer code
//
//						StoreIDField.Text = Order.Customer.Code = Order.StoreID = storeIDText;
//						WarehouseButton.Enabled = true;
//						ProviderButton.Enabled = true;
//						if (ProviderField.Text != null && ProviderField.Text.Trim() != "") {
//							CheckForDefaultProvider (Order.CustomerID);						
//						}
//					}
//					// Uncommented by Eldon for Testing

				} else {
					StoreIDField.Text = Order.StoreID = null;
				}
			};

			ProviderField.EditingChanged += (object sender, EventArgs e) => {
				_edited = true;
				WarehouseField.Text = "";
				this.Order.WarehouseID = 0;
			};

			StoreMgrFirstNameField.EditingDidEnd += (object sender, EventArgs e) => {
				if (StoreMgrFirstNameField.Text != null && StoreMgrFirstNameField.Text.Trim().Length > 0) {
					_edited = true;
				}
				// persist the changes to the customer record
				Order.Customer.ContactFirstName = Order.StoreMgrFirstName = StoreMgrFirstNameField.Text;
			};

			StoreMgrLastNameField.EditingDidEnd += (object sender, EventArgs e) => {
				if (StoreMgrLastNameField.Text != null && StoreMgrLastNameField.Text.Trim().Length > 0) {
					_edited = true;
				}
				// persist the changes to the customer record
				Order.Customer.ContactLastName = Order.StoreMgrLastName = StoreMgrLastNameField.Text;
			};

			StoreMgrEmailField.EditingDidEnd += (object sender, EventArgs e) => {
				if (StoreMgrEmailField.Text != null && StoreMgrEmailField.Text.Trim().Length > 0 ) {
					_edited = true;
				}
					
				// persist the changes to the customer record
				Order.Customer.ContactEmail = Order.StoreMgrEmail = StoreMgrEmailField.Text;
			};

			StoreMgrEmailField.EditingChanged += (object sender, EventArgs e) => {
				if (StoreMgrEmailField.Text != null && StoreMgrEmailField.Text.Trim().Length > 0 ) {
					Order.Customer.ContactEmail = Order.StoreMgrEmail = StoreMgrEmailField.Text;
					_edited = true;

				}
			};


			StoreMgrPhoneField.EditingDidEnd += (object sender, EventArgs e) => {
				if (StoreMgrPhoneField.Text != null && StoreMgrPhoneField.Text.Trim().Length > 0) {
					_edited = true;
				}
				// persist the changes to the customer record
//				Order.Customer.ContactNumber = Order.StoreMgrPhone = StoreMgrPhoneField.Text;
				Order.Customer.ContactMobile = Order.StoreMgrPhone = StoreMgrPhoneField.Text;
			};

			// not using address as of now, 12-October-2013
//			NSError abError;
//			ABAddressBook addressBook = ABAddressBook.Create (out abError);
			// see if we can get a list of people, if not disable the contact lookup button
//			if (abError != null || addressBook.GetPeople().Length <= 0)
//				ContactLookupButton.Enabled = false;

		}

		partial void switchEditMode (Foundation.NSObject sender)
		{
			if (OrderDetailTableView.NumberOfRowsInSection (0) > 0 && !OrderDetailTableView.Editing) {
				((OrderDetailSource)OrderDetailTableView.Source).BeginEditing ();
				OrderDetailTableView.SetEditing (true, true);
				OrderLineDeleteButton.Title = "Done";
			} else if (OrderDetailTableView.Editing) {
				OrderDetailTableView.SetEditing (false, true);
				((OrderDetailSource)OrderDetailTableView.Source).EndEditing ();
				OrderLineDeleteButton.Title = "Delete";
			}
		}

		public bool HasValue(string someString) {
			return (someString != null && someString.Trim ().Length > 0);
		}

		public void SetStoreMgrFields (string firstname, string lastname, string email, string phone)
		{
			if (HasValue (firstname) || HasValue (lastname) || HasValue(email) || HasValue(phone))
				_edited = true;
			StoreMgrFirstNameField.Text = firstname;
			StoreMgrLastNameField.Text = lastname;
			StoreMgrEmailField.Text = email;
			StoreMgrPhoneField.Text = phone;
			SetOrderStoreMgr ();
		}

		private void SetOrderStoreMgr ()
		{
			Order.StoreMgrFirstName = StoreMgrFirstNameField.Text;
			Order.StoreMgrLastName = StoreMgrLastNameField.Text;
			Order.StoreMgrEmail = StoreMgrEmailField.Text;
			Order.StoreMgrPhone = StoreMgrPhoneField.Text;
		}

		void setCustomer (Customer customer)
		{
			_edited = true;
			this.Order.Customer = customer;
			this.Order.CustomerID = customer.ID;
			CustomerNameField.Text = this.Order.CustomerName = customer.Name;
			if (customer.StateName != null && customer.StateName.Trim ().Length >= 0)
				StateLabel.Text = "[ " + customer.StateName + " ]";
			else
				StateLabel.Text = "[ State ]";
			StoreIDField.Enabled = true;
		}

		public Customer SelectedCustomer
		{
			get {
				return this.Order.Customer;
			}
			set {
				if (value != null && value.ID != this.Order.CustomerID) {

					if (this.Order.ProviderID == 0) {
						// provider has not been selected yet so simply set the customer value and check for defaults
						setCustomer (value);
						CheckForDefaultProvider (value.ID);
						ProviderButton.Enabled = true;
					} else {
						// check if the current provider is applicable to the selected customer
						var provider = dalFacade.getProviderByCustomerAndProviderID (value.ID, this.Order.ProviderID);
						if (provider == null || this.Order.ProviderID != provider.ID) {
							// the newly selected store is not mapped with the previously selected provider
							var alert = new UIAlertView ("Store Change", "Changing to this store will Clear up the Order Details.  Do you want to continue?", 
								            null, "Cancel", "Yes");
							alert.Clicked += (object s, UIButtonEventArgs e) => {
								if (e.ButtonIndex == 1) {
									reset ();
									setCustomer (value);
									CheckForDefaultProvider (value.ID);
									ProviderButton.Enabled = true;
								}
							};

							alert.Show ();
						} else {
							setCustomer (value);
							setCustomerFieldsFromProvider ();
						}
					}
				} else if (value.ID == 0) {
					// this must be a reset
					setCustomer (value);
					_edited = false;
				}
			}
		}

		void setCustomerFieldsFromProvider ()
		{
			if (this.Order.ProviderID > 0 && this.Order.CustomerID > 0) {
				// ProviderID > 0 means the Provider has been either manually or automatically selected
				Customer c = dalFacade.getCustomerByProviderID (this.Order.ProviderID, this.Order.CustomerID);
				if (c != null && c.ID != 0) {
					this.Order.Customer = c;
					StoreIDField.Text = this.Order.StoreID = c.Code;
					SetStoreMgrFields (c.ContactFirstName, c.ContactLastName, c.ContactEmail, c.ContactMobile);
				}
			} else {
				StoreIDField.Text = this.Order.StoreID = "";
				SetStoreMgrFields ("", "", "", "");
			}
		}

		public long CustomerID {
			get {
				return this.Order.CustomerID;
			}
		}

		private void CheckForDefaultProvider (long customerID)
		{
			// PROVIDER LIST TEST
			var providerList = dalFacade.getProviderListByCustomerIDFiltered (customerID);
			if (providerList.Count == 1) {
				SelectProvider (providerList [0]);
				setCustomerFieldsFromProvider ();
			}
		}

		public void SelectProvider(Provider provider) {
			if (provider != null && this.Order.ProviderID != provider.ID) {
				_edited = true;
				this.Order.ProviderID = provider.ID;
				ProviderField.Text = this.Order.ProviderName = provider.Name;
				CheckForDefaultWarehouse ();
				WarehouseButton.Enabled = true;
				setCustomerFieldsFromProvider ();
				OrderLineAddButton.Enabled = true;
				OrderLineDeleteButton.Enabled = true;
			}
		}

		public long ProviderID {
			get {
				return this.Order.ProviderID;
			}
		}

		public void DisableProviderButton ()
		{
			ProviderButton.Enabled = false;
		}

		public void EnableProviderButton ()
		{
			ProviderButton.Enabled = true;
		}

		public void CheckForDefaultWarehouse() {
			var warehouseList = dalFacade.getWarehouseListByProviderID (this.Order.ProviderID);
			if (warehouseList != null && warehouseList.Count == 1) {
				SelectWarehouse (warehouseList.First ());
			} else {
				SelectWarehouse(new ProviderWarehouse());
			}
		}

		public void SelectWarehouse(ProviderWarehouse warehouse) {
			if (warehouse != null) {
				_edited = true;
				this.Order.WarehouseID = warehouse.ID;
				WarehouseField.Text = warehouse.Name;
			}
		}

		public OrderLine GetOrderLineItem (Product product)
		{
			if (Order.OrderLineList == null || Order.OrderLineList.Count <= 0)
				return null;
			else {
				var orderLine = Order.OrderLineList.Where (o => o.Product.PK == product.PK);
				if (orderLine.Count () > 0)
					return orderLine.First ();
				else
					return null;
			}
		}


		// Adds an order line item to the list.
		public void AddOrderLineItem (Product product, int quantity)
		{
			_edited = true;
			if (Order.OrderLineList == null)
				Order.OrderLineList = new List<OrderLine> ();

			if (Order.OrderLineList != null || Order.OrderLineList.Count > 0) {

				// Disable Order Type and Release Date

				this.OrderTypeSegment.Enabled = false;
				//this.ReleaseDateField.Enabled = false;
				this.ReleaseDateButton.Enabled = false;

			} else {
				this.OrderTypeSegment.Enabled = true;
				//this.ReleaseDateField.Enabled = true;
				this.ReleaseDateButton.Enabled = true;

			
			}

			// zero discounts, not required yet
			var orderDetail = new OrderLine (product, quantity, 0);

			var productLineItem = GetOrderLineItem (product);
			if (productLineItem == null) { 
				Order.OrderLineList.Add (orderDetail);
			} else {
				productLineItem.Quantity = quantity;
			}
			OrderDetailTableView.ReloadData ();

//			// Enable/Disable the Order Type Segment 
//			if (Order.OrderLineList.Count == 0) {
//				// disable the Order Type Segment
//				this.OrderTypeSegment.Enabled = true;
//				this.ReleaseDateButton.Enabled = true;
//			} else {
//				this.OrderTypeSegment.Enabled = false;
//				this.ReleaseDateButton.Enabled = false;
//			}



		}

		partial void saveOrder (Foundation.NSObject sender)
		{
			_edited = false;
			new UIAlertView ("Not Yet Implemented", "This will save the order for later editing.", null, "OK", null).Show ();
		}

		void resetOrderDetailTableView ()
		{
			OrderDetailTableView.Source = null;
			OrderDetailTableView.Source = new OrderDetailSource (Order.OrderLineList, ProviderButton, OrderTypeSegment, ReleaseDateButton);
			OrderDetailTableView.ReloadData ();
		}

		public void reset ()
		{
			// Fix for #60.  Remove Required Date and Payment Terms, not needed by Wrigley's
			//			setRequiredDate(DateTime.MinValue);
			//			setTerms(null);

			Order = null;
			Order = new Order ();
			SelectedCustomer=Order.Customer;
			StoreIDField.Enabled = false;
			StoreIDField.Text = "";
			ProviderButton.Enabled = false;
			ProviderField.Text = "";
			WarehouseButton.Enabled = false;
			WarehouseField.Text = "";
			OrderLineAddButton.Enabled = false;
			OrderLineDeleteButton.Enabled = false;
			setCustomerFieldsFromProvider ();
			resetOrderDetailTableView ();
			this.OrderTypeSegment.Enabled = true;
			this.OrderTypeSegment.SelectedSegment = 0;
			this.ReleaseDateButton.Enabled = true;
			this.ReleaseDateField.Text = "Today";

			_edited = false;
		}

		partial void newOrder (Foundation.NSObject sender)
		{
			if (_edited) {
				var alert = new UIAlertView("New Order", "This will Clear up the entire Purchase Order Form.  Do you want to continue?", 
				                            null, "Cancel", "Yes");
				alert.Clicked += (object s, UIButtonEventArgs e) => {
					if (e.ButtonIndex == 1) {
						reset();
					}
				};

				alert.Show();
			}
		}
		//		partial void editLineItems(MonoTouch.Foundation.NSObject sender) {
		//			OrderDetailTableView.SetEditing(true, true);
		//		}
		public override bool ShouldPerformSegue (string segueIdentifier, NSObject sender)
		{
			if (segueIdentifier == "OrderLineInfoSegue" && SelectedOrderLine == null)
				return false;
			else if (segueIdentifier == "OrderConfirmationSegue" && IsInformationMissing) {
				new UIAlertView ("Incomplete Order Details", "Please Complete Order Details.", null, "OK", null).Show ();
				return false;
			} else if (segueIdentifier == "OrderConfirmationSegue" && !(IsValidEmail)) {
				new UIAlertView ("Invalid Email Address", "Please enter a valid email address.", null, "OK", null).Show ();
				StoreMgrEmailField.BecomeFirstResponder ();
				return false;
			} 
			else if (segueIdentifier == "OrderLineSelectionSegue") {
				this.productSelectionSource.ProviderID = this.Order.ProviderID;

				DateTime _DateCriteria;

				if (this.OrderTypeSegment.SelectedSegment == (int)Constants.ORDERTYPE.REGULAR) {
					if (this.ReleaseDateField.Text == "Today") {
						_DateCriteria = DateTime.Now;
					} else {
						_DateCriteria = DateTime.Parse (this.ReleaseDateField.Text);
					}
				} else {
					_DateCriteria = this.Order.OrderDate;
				}

				this.productSelectionSource.IsRegularOrder = this.OrderTypeSegment.SelectedSegment == (int)Constants.ORDERTYPE.REGULAR ? true : false;

				this.productSelectionSource.DateCriteria = _DateCriteria;

				// Added for Regular Order Release Date
				this.productSelectionSource.ReleaseDate = this.Order.ReleaseDate;

				if (!this.productSelectionSource.HasProductGroups ()) {
					new UIAlertView ("Pre-sell Order", "Cannot create pre-sell order.\nNo Products exist with a future Start date.", null, "OK", null).Show ();
					return false;
				}
				return true;
			}

			else 
				return base.ShouldPerformSegue (segueIdentifier, sender);
		}

		private bool IsInformationMissing {
			get {
			
				return (Order.Customer == null || 
						Order.Customer.PK <= 0 ||
						TotalOrderQuantity () <= 0 ||
				        Order.StoreID == null ||
				        Order.StoreID.Trim().Length <= 0 ||
						Order.StoreMgrFirstName == null ||
						Order.StoreMgrFirstName.Trim ().Length <= 0 ||
						Order.StoreMgrLastName == null ||
						Order.StoreMgrLastName.Trim ().Length <= 0 ||
				    	Order.StoreMgrEmail == null ||
				    	Order.StoreMgrEmail.Trim().Length <=0 ||
				    	Order.WarehouseID <= 0);

			}
		}
			
		private bool IsValidEmail {
			get {
				return (Regex.IsMatch(StoreMgrEmailField.Text.Trim(), @"\A(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)\Z"));
			}
		}

		private int TotalOrderQuantity ()
		{
			if (Order == null || Order.OrderLineList == null)
				return 0;
			else {
				var totalQuantity = 0;
				foreach (var lineItem in Order.OrderLineList) {
					totalQuantity += lineItem.Quantity;
				}
				return totalQuantity;
			}
		}


		private void showDatePicker(){
			// Create custom Popover here
			UIView _view = new UIView();
			CoreGraphics.CGRect pickerFrame = new CoreGraphics.CGRect(0,0,500,500);
			UIDatePicker pickerView = new UIDatePicker(pickerFrame);
			pickerView.Mode = UIDatePickerMode.Date;

			this.datePickerPopover.View = pickerView;
			_view.AddSubview(this.datePickerPopover.View);

			UIViewController popoverContent = new UIViewController();
			popoverContent.View = _view;

			popoverContent.ContentSizeForViewInPopover =  new CoreGraphics.CGSize(500, 500);

			if(this.datePickerPopover != null){

			}

			UIPopoverController _popupController = new UIPopoverController(this.datePickerPopover);
			//_popupController.Delegate = this;

			_popupController.PresentFromRect(pickerFrame, this.View, UIPopoverArrowDirection.Up, false);
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{

			base.PrepareForSegue (segue, sender);

			CoreGraphics.CGSize pSize = new CoreGraphics.CGSize (290, 216);
	

			if (OrderDetailTableView != null && OrderDetailTableView.Editing) {
				switchEditMode (null);
			}
			switch (segue.Identifier) {
			case "OrderConfirmationSegue":
				var confirmationController = segue.DestinationViewController as OrderConfirmationController;
				this.OrderType =  this.OrderTypeSegment.SelectedSegment == (int)Constants.ORDERTYPE.REGULAR ? Constants.ORDERTYPE.REGULAR : Constants.ORDERTYPE.PRESELL;
				confirmationController.orderViewController = this;
				confirmationController.Order = Order;
				confirmationController.WarehouseName = WarehouseField.Text;
				break;
			case "CustomerSelectionSegue":
				var customerListController = segue.DestinationViewController as StoreSelectionController;
				if (customerListController != null) {
					customerListController.OrderViewController = this;
				}
				break;
			case "ProviderSelectionSegue":
				var providerController = segue.DestinationViewController as ProviderController;
				providerController.PreferredContentSize = pSize;
				if (providerController != null) {
					providerController.OrderController = this;
				}
				break;
			case "HoldDateSegue":
				var holdDateController = segue.DestinationViewController as HoldDateController;
				if (holdDateController != null) {
					holdDateController.OrderController = this;
				}
				break;
			case "WarehouseSelectionSegue":
				var warehouseController = segue.DestinationViewController as WarehouseController;

				warehouseController.PreferredContentSize = pSize;
				if (warehouseController != null) {
					warehouseController.OrderController = this;
				}
				break;
			case "OrderLineTableSegue":
				var tableViewController = segue.DestinationViewController as UITableViewController;
				OrderDetailTableView = tableViewController.TableView;
				break;
			case "OrderLineSelectionSegue":
				var orderLineController = segue.DestinationViewController as OrderLineViewController;
				orderLineController.IsRegularOrder = this.OrderTypeSegment.SelectedSegment == (int)Constants.ORDERTYPE.REGULAR ? true : false;

				orderLineController.ProviderName = this.ProviderField.Text;

				DateTime _DateCriteria;

				if (this.OrderTypeSegment.SelectedSegment == (int)Constants.ORDERTYPE.REGULAR) {
					if (this.ReleaseDateField.Text == "Today") {
						_DateCriteria = DateTime.Now;
					} else {
						_DateCriteria = DateTime.Parse (this.ReleaseDateField.Text);
					}
				} else {
					_DateCriteria = this.Order.OrderDate;
				}

				orderLineController.ReleaseDate = _DateCriteria.ToUniversalTime();

				orderLineController.DateCriteria = _DateCriteria.ToUniversalTime();

				// Added for Regular Order:

				if (orderLineController != null) {
					SelectedProduct = null;
					orderLineController.OrderController = this;
				}
				break;
			case "ContactSelectionSegue":
				var contactSelectionController = segue.DestinationViewController as ContactSelectionController;
				if (contactSelectionController != null) {
					contactSelectionController.OrderViewController = this;
				}
				break;
			case "EditOrderLineSegue":
				var orderEditorLineController = segue.DestinationViewController as OrderLineViewController;
				if (orderEditorLineController != null) {
					orderEditorLineController.OrderController = this;
					orderEditorLineController.SelectedProduct = SelectedProduct;

					orderEditorLineController.IsRegularOrder = this.OrderTypeSegment.SelectedSegment == (int)Constants.ORDERTYPE.REGULAR ? true : false;
					orderEditorLineController.ProviderName = this.ProviderField.Text;

				}
				break;
			case "OrderDetailsSegue":
				var orderDetailsController = segue.DestinationViewController as OrderDetailsViewController;
				if (orderDetailsController != null)
					orderDetailsController.OrderController = this;
				break;
			case "OrderLineInfoSegue":
				var orderLineInfoController = segue.DestinationViewController as OrderLineInfoController;
				orderLineInfoController.OrderViewController = this;
				orderLineInfoController.OrderLineItem = SelectedOrderLine;
				break;
//			case "RequiredDatePickerSegue":
//				var requiredDateController = segue.DestinationViewController as RequiredDateController;
//				if (Order.RequiredDate <= DateTime.MinValue)
//					requiredDateController.SelectedDate = Order.RequiredDate;
//				requiredDateController.OrderController = this;
//				break;
			case "TermsPickerSegue":
				var termsController = segue.DestinationViewController as TermsController;
				termsController.OrderController = this;
				break;

			case "ReleaseDateSegue":
				var releaseDateController = segue.DestinationViewController as DatePickerController;
				releaseDateController._orderViewController = this;
				break;
			}

		}

		public class OrderDetailSource : UITableViewSource
		{
			static readonly NSString _cellID = new NSString ("OrderLineCell");
			IList<OrderLine> _orderLineItems;
			UIKit.UIButton _providerButton;
			UIKit.UISegmentedControl _orderTypeSegmentedControl;
			UIKit.UIButton _releaseDateButton;

			Boolean _editMode = false;

			public OrderDetailSource (IList<OrderLine> orderlineItems, UIButton providerButton, UISegmentedControl orderTypeControl, UIButton releaseDteButton)
			{
				_orderLineItems = orderlineItems;
				_providerButton = providerButton;
				_orderTypeSegmentedControl = orderTypeControl;
				_releaseDateButton = releaseDteButton;
			}

			public void BeginEditing ()
			{
				_editMode = true;
			}

			public void EndEditing ()
			{
				_editMode = false;
			}

			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return _orderLineItems.Count ();
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				SelectedOrderLine = _orderLineItems [indexPath.Row];
				SelectedProduct = SelectedOrderLine.Product;
				SelectedProductQuantity = SelectedOrderLine.Quantity;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Returns true for all rows if we are in edit mode.  
				// Disables swipe to delete unless user explicitly goes into edit mode.
				return _editMode;
			}

			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete) {
					_orderLineItems.RemoveAt (indexPath.Row);
					tableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
					tableView.ReloadSections (new NSIndexSet (0), UITableViewRowAnimation.None);

					if (_orderLineItems.Count == 0) {

						if (this._orderTypeSegmentedControl.SelectedSegment == (int)Constants.ORDERTYPE.PRESELL) {
							_releaseDateButton.Enabled = false;
						} else {
							_releaseDateButton.Enabled = true;
						}

						_providerButton.Enabled = true;
						// _releaseDateButton.Enabled = true;
						_orderTypeSegmentedControl.Enabled = true;
					} else {
						_releaseDateButton.Enabled = false;
						_orderTypeSegmentedControl.Enabled = false;
					}
				}
			}

			public override bool CanMoveRow (UITableView tableView, NSIndexPath indexPath)
			{
				return false;
			}

			public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
			{
				return UITableViewCellEditingStyle.Delete;
			}

			public override string TitleForHeader(UITableView tableView, nint section)
			{
				// removed header to provide more space for Hold Until Date
//				if (_orderLineItems != null && _orderLineItems.Count > 0)
//					return "Line Items";
//				else
//					return "";
				return "";
			}

			public override string TitleForFooter (UITableView tableView, nint section)
			{
				var footer = "";
//				var total = new Decimal (0);
				if (_orderLineItems != null) {
					footer = _orderLineItems.Count.ToString () + " items.";
					// Fix for #61 - Total tally is not needed by Wrigley's
//					foreach (var lineItem in _orderLineItems) 
//						total += new Decimal (lineItem.Quantity) * lineItem.Product.UnitPrice;
//					if (total > 0)
//						footer += String.Format (", total amount: {0:C2}", total);
				}
				return footer;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = (UITableViewCell)tableView.DequeueReusableCell (_cellID, indexPath);
				cell.TextLabel.Text = _orderLineItems [indexPath.Row].Product.Name;
//				cell.DetailTextLabel.Text = _orderLineItems [indexPath.Row].Quantity.ToString ();
				var orderLine = _orderLineItems [indexPath.Row];
				cell.DetailTextLabel.Text = String.Format ("Code: {0}, Qty: {1}", orderLine.Product.Code,
				                                           orderLine.Quantity);
				return cell;
			}



		}
	}
}
