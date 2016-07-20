using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;

using Foundation;
using UIKit;

using OneTradeCentral.DTOs;
using OneTradeCentral.DAO;


namespace OneTradeCentral.iOS
{
	public class StoreListingsSource : UITableViewSource
	{

		static DALFacade dalFacade = new DALFacade ();
		static IList<Customer> CustomerList = new List<Customer> ();

		public string searchString { get; set; }
		public bool IsEditable { get; set; }
		public enum SearchScope { NAME=0, STATE=1};
		public SearchScope Filter { get; set; }
		private long _providerID;

		StoreListingsController customerListController;

		public StoreListingsSource (StoreListingsController c, long providerID)
		{
			this.customerListController = c;
			this._providerID = providerID;
			refreshCustomerList();

		}

		public void refreshCustomerList() {
			CustomerList = dalFacade.getAllCustomerByProviderID (this._providerID);
		}


		private IList<Customer> getFilteredList ()
		{
			if (searchString == null) {
				return CustomerList;
			} else {
				IList<Customer> filteredCustomerList = new List<Customer> ();
				switch (Filter) {
				case SearchScope.STATE:
					foreach (var c in CustomerList) {
						if (c.StateName != null && c.StateCode.ToUpper().Contains (searchString.ToUpper ()))
							filteredCustomerList.Add (c);
					}
					break;
				default:
					foreach (var c in CustomerList) {
						if (c.Name != null && c.Name.ToUpper ().Contains (searchString.ToUpper ()))
							filteredCustomerList.Add (c);
					}
					break;
				}
				return filteredCustomerList;
			}
		}


		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{

			if (section == 0)
				return 0;
			return 40;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (getFilteredList () == null) {
				return 0;
			}
			return getFilteredList ().Count;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (customerListController != null) {
				customerListController.Customer = getFilteredList () [indexPath.Row];
			}
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (UITableViewCell)tableView.DequeueReusableCell (StoreListingsCell.CellID, indexPath);
			if (cell == null) {
				cell = new UITableViewCell ();
			}

			var customer =  getFilteredList () [indexPath.Row];
			cell.TextLabel.Text = customer.Name;
			var detailText = "";
			if (customer.StateName != null && customer.StateName.Trim ().Length > 0)
				detailText += customer.StateCode;
			cell.DetailTextLabel.Text = detailText;

			return cell;
		}

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return IsEditable;
		}

		public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			if (editingStyle == UITableViewCellEditingStyle.Delete) {
				// delete the record from the database
				var customer = CustomerList[indexPath.Row];
				// delete the record from the view
				dalFacade.DeleteCustomer(customer);
				CustomerList.RemoveAt(indexPath.Row);
				customerListController.TableView.DeleteRows(new NSIndexPath[]{indexPath}, UITableViewRowAnimation.Fade);

			}
		}

	}
}

