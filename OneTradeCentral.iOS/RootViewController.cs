using System;
using CoreGraphics;
using System.Collections.Generic;

using Foundation;
using UIKit;

using OneTradeCentral.DTOs;

namespace OneTradeCentral.iOS
{
	public partial class RootViewController : UITableViewController
	{
//		DataSource dataSource;

		public RootViewController (IntPtr handle) : base (handle)
		{
			// Custom initialization
		}

		public OrderListViewController OrderListViewController {
			get;
			set;
		}

//		void AddNewItem (object sender, EventArgs args)
//		{
//			dataSource.Objects.Insert (0, DateTime.Now);
//
//			using (var indexPath = NSIndexPath.FromRowSection (0, 0))
//				TableView.InsertRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
//		}
		
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			NavigationItem.LeftBarButtonItem = EditButtonItem;

//			var addButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, AddNewItem);
//			NavigationItem.RightBarButtonItem = addButton;

//			TableView.Source = dataSource = new DataSource (this);
		}

		class DataSource : UITableViewSource
		{
			static readonly NSString CellIdentifier = new NSString ("DataSourceCell");
			List<Order> orderList = new List<Order> ();
			RootViewController controller;

			public DataSource (RootViewController controller)
			{
				this.controller = controller;
			}

			public IList<Order> OrderList {
				get { return orderList; }
			}

			// Customize the number of sections in the table view.
			public override nint NumberOfSections (UITableView tableView)
			{
				return 1;
			}

			public override nint RowsInSection (UITableView tableview, nint section)
			{
				return orderList.Count;
			}

			// Customize the appearance of table view cells.
			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = (UITableViewCell)tableView.DequeueReusableCell (CellIdentifier, indexPath);

				cell.TextLabel.Text = orderList [indexPath.Row].ToString ();

				return cell;
			}

			public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the specified item to be editable.
				return true;
			}

			public override void CommitEditingStyle (UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			{
				if (editingStyle == UITableViewCellEditingStyle.Delete) {
					// Delete the row from the data source.
					orderList.RemoveAt (indexPath.Row);
					controller.TableView.DeleteRows (new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
				} else if (editingStyle == UITableViewCellEditingStyle.Insert) {
					// Create a new instance of the appropriate class, insert it into the array, and add a new row to the table view.
				}
			}

			/*
			// Override to support rearranging the table view.
			public override void MoveRow (UITableView tableView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
			{
			}
			*/

			/*
			// Override to support conditional rearranging of the table view.
			public override bool CanMoveRow (UITableView tableView, NSIndexPath indexPath)
			{
				// Return false if you do not want the item to be re-orderable.
				return true;
			}
			*/

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				controller.OrderListViewController.SetDetailItem (orderList [indexPath.Row]);
			}
		}
	}
}

