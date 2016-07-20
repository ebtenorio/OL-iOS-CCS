using System;
using CoreGraphics;
using System.Collections.Generic;
using Foundation;
using UIKit;
using OneTradeCentral.DTOs;
using OneTradeCentral.DAO;
using OneTradeCentral.Utility;

namespace OneTradeCentral.iOS
{
	public partial class OrderListViewController : UITableViewController
	{
		public UIViewController sourceController { get; set; }
		OrderListSource orderListSource;
		// the scope elements below should match the declaration sequence in XCode
		Order selectedOrder;
		private static DALFacade dalFacade = new DALFacade ();

		public OrderListViewController (IntPtr handle) : base (handle)
		{					
			orderListSource = new OrderListSource(this);
		}

		public void SetDetailItem (Order newDetailItem)
		{
			if (selectedOrder != newDetailItem) {
				selectedOrder = newDetailItem;
			}
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.SearchBar.ScopeButtonTitles = new string[2]{ "Store Name", "Order Number" };
			this.SearchBar.ShowsScopeBar = true;
			this.SearchBar.SizeToFit ();

			TableView.Source = orderListSource;
//			if (sourceController != null)
//				sourceController.DismissViewController (true, null);
			SearchBar.SelectedScopeButtonIndexChanged += (object sender, UISearchBarButtonIndexEventArgs e) => {
				SearchBar.ResignFirstResponder();
				orderListSource.Filter = (OrderListSource.SearcScope) (int) e.SelectedScope;
				if (SearchBar.Text != null && SearchBar.Text.Trim().Length > 0)
					TableView.ReloadData();
				switch (orderListSource.Filter) {
				case OrderListSource.SearcScope.STORE_NAME:
					SearchBar.Placeholder = "Store Name";
					SearchBar.KeyboardType = UIKeyboardType.ASCIICapable;
					break;
				case OrderListSource.SearcScope.STORE_ID:
					SearchBar.Placeholder = "Store ID";
					SearchBar.KeyboardType = UIKeyboardType.NumberPad;
					break;
				case OrderListSource.SearcScope.ORDER_NO:
					SearchBar.Placeholder = "Order Number";
					SearchBar.KeyboardType = UIKeyboardType.NumbersAndPunctuation;
					break;
				case OrderListSource.SearcScope.ORDER_DATE:
					SearchBar.Placeholder = "Order Date";
					SearchBar.KeyboardType = UIKeyboardType.NumbersAndPunctuation;
					break;
				default:
					SearchBar.Placeholder = "Store Name";
					SearchBar.KeyboardType = UIKeyboardType.ASCIICapable;
					break;
				}
			};

			SearchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => {
				orderListSource.SearchString = SearchBar.Text;
				TableView.ReloadData();
			};

			// TODO: find out why ResignFirstResponder doesn't work on modal windows
			SearchBar.SearchButtonClicked += (sender, e) => {
				SearchBar.ShowsCancelButton = false;
				SearchBar.ResignFirstResponder();
			};

			SearchBar.OnEditingStarted += (sender, e) => {
				SearchBar.ShowsCancelButton = true;
			};

			SearchBar.CancelButtonClicked += (sender, e) => {
				SearchBar.ShowsCancelButton = false;
				SearchBar.ResignFirstResponder();
			};
		}

		public override bool ShouldPerformSegue (string segueIdentifier, NSObject sender)
		{
			// execute the segue only if there is a selected order
			if (segueIdentifier == "OrderInfoViewSegue" && selectedOrder == null)
				return false;
			else if (segueIdentifier != null)
				return base.ShouldPerformSegue (segueIdentifier, sender);
			else
				return true;
		}

		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);
			if (segue.Identifier == "OrderInfoViewSegue") {
				var orderInfoViewController = segue.DestinationViewController as OrderInfoViewController;

				if (DateTime.Parse (selectedOrder.OrderDate.ToString ("d")) < DateTime.Parse (Identity.LastVersionReleaseDate ()) && dalFacade.hasCustomerOldID (selectedOrder)) {
					dalFacade.populateOrderWithOldIDs (selectedOrder);
				} else {
					dalFacade.populateOrder (selectedOrder);
				}

				orderInfoViewController.Order = selectedOrder;
			}
		}

		class OrderListSource : UITableViewSource
		{
			static readonly NSString CellIdentifier = new NSString ("OrderCell");
			//			private IList<Order> _orderList = new List<Order> ();

			public enum SearcScope { 
				STORE_NAME = 0,
				ORDER_NO = 1,
				STORE_ID = 2,
				ORDER_DATE = 3
			};

			public SearcScope Filter { get; set; }

			public string SearchString { get; set; }
			// FIXME: do we really need to declare the parent controller here?
			OrderListViewController controller;

			public OrderListSource (OrderListViewController controller)
			{
				this.controller = controller;
			}

			private IList<Order> FilteredList { 
				get {
					if (SearchString != null && SearchString.Trim ().Length > 0) {
						switch (Filter) {
						case SearcScope.STORE_NAME:
							return dalFacade.getOrderListByStoreName (SearchString);
						case SearcScope.STORE_ID:
							return dalFacade.getOrderListByStoreID (SearchString);
						case SearcScope.ORDER_NO:
							return dalFacade.getOrderListByOrderNumber (SearchString);
						default:
							return dalFacade.getOrderList ((int)dalFacade.getUserAccount ().AccountID);
						}
					} else {
						return dalFacade.getOrderList ((int)dalFacade.getUserAccount ().AccountID);
					}
				}
			}
			// Customize the number of sections in the table view.
			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return FilteredList.Count;
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = (UITableViewCell)tableView.DequeueReusableCell (CellIdentifier, indexPath);

				var order = FilteredList [indexPath.Row];
				
				cell.TextLabel.Text = order.CustomerName; // get order by either the OldID or the Customer ID depending on the OrderDate

				var poNumberOrStatus = "";
				if (order.UploadStatus == (int) Order.STATUS.Completed) {
					poNumberOrStatus = "PO No: " + order.OrderNumber;
				} else {
					poNumberOrStatus = ((Order.STATUS) order.UploadStatus).ToString();
				}


				if ((order.IsRegularOrder == true) && order.RequestedReleaseDate.ToLocalTime().Date > order.OrderDate.ToLocalTime().Date) {
					cell.DetailTextLabel.Text = poNumberOrStatus + ", " + order.DateCreated.ToLocalTime ().ToString ("dd/MM/yy") + " (" + order.RequestedReleaseDate.ToLocalTime().ToString("dd/MM/yy") + ")";
				}

				if ((order.IsRegularOrder == true) && order.RequestedReleaseDate.ToLocalTime ().Date == order.OrderDate.ToLocalTime ().Date) {
					cell.DetailTextLabel.Text = poNumberOrStatus + ", " + order.DateCreated.ToLocalTime ().ToString ("dd/MM/yy");
				}

				if (order.IsRegularOrder == false) {
					cell.DetailTextLabel.Text = poNumberOrStatus + ", " + order.DateCreated.ToLocalTime ().ToString ("dd/MM/yy") + " (Pre-sell)";	
				}

				if ((order.IsRegularOrder == null)) {
					cell.DetailTextLabel.Text = poNumberOrStatus + ", " + order.DateCreated.ToLocalTime ().ToString ("dd/MM/yy");
				}

								

				return cell;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Returns false, we do not want the submitted orders to be editable.
				return false;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				controller.SetDetailItem (FilteredList [indexPath.Row]);
			}
		}
	}
}

