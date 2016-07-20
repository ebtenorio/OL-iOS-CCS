
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
	public class StoreListSource : UITableViewSource
	{	
		static DALFacade dalFacade = new DALFacade ();
		static IList<Customer> CustomerList = new List<Customer> ();

		public string searchString { get; set; }
		public bool IsEditable { get; set; }
		// see ticket #132, store ID is user-editable and no longer referenceable
//		public enum SearchScope { NAME=0, ID=1, STATE=2};
		public enum SearchScope { NAME=0, STATE=1};
		public SearchScope Filter { get; set; }

		StoreListController customerListController;

		public StoreListSource (StoreListController c)
		{
			this.customerListController = c;
			refreshCustomerList();
		}

		public void refreshCustomerList() {
//			CustomerList = dalFacade.getActiveCustomerList ();
			CustomerList = dalFacade.getActiveUniqueCustomerList ();
		}

		private IList<Customer> getFilteredList ()
		{
			if (searchString == null) {
				return CustomerList;
			} else {
				IList<Customer> filteredCustomerList = new List<Customer> ();
				switch (Filter) {
//				case SearchScope.ID:
//					foreach (var c in CustomerList) {
//						if (c.Code != null && c.Code.ToString ().Contains (searchString.ToUpper ()))
//							filteredCustomerList.Add (c);
//					}
//					break;
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
		
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return getFilteredList ().Count;
		}
		
//		public override string TitleForHeader (UITableView tableView, int section)
//		{
//			return "Customer Information";
//		}
		
//		public override string TitleForFooter (UITableView tableView, int section)
//		{
//			return "";
//		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (customerListController != null) {
				customerListController.Customer = getFilteredList () [indexPath.Row];
			}
		}
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			var cell = (UITableViewCell)tableView.DequeueReusableCell (StoreListCell.CellID, indexPath);
			if (cell == null) {
				cell = new UITableViewCell ();
			}

			var customer =  getFilteredList () [indexPath.Row];
			cell.TextLabel.Text = customer.Name;
			// #132, can no longer reference store ID
//			var detailText = String.Format ("ID: {0}", customer.Code);
			var detailText = "";
			if (customer.StateName != null && customer.StateName.Trim ().Length > 0)
				detailText += customer.StateCode;
//			else
//				detailText += ".";
			cell.DetailTextLabel.Text = detailText;

			// fix for #101 - addresses are not needed by Wrigley's
//			var address = getFilteredList() [indexPath.Row].getDisplayAddress();
//			if (address != null || address.Trim() == "") {
//				cell.DetailTextLabel.Text = address;
//			} else {
//				cell.DetailTextLabel.Text = "Unknown Address";
//			}
			
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

