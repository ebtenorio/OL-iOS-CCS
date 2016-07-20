
using System;
using CoreGraphics;
using System.Linq;
using Foundation;
using UIKit;

using OneTradeCentral.DTOs;

namespace OneTradeCentral.iOS
{
	public partial class ProductListController : UITableViewController
	{
		ProductListSource ProductListSource = new ProductListSource();
		private OrderLineViewController _orderLineViewController;
		private long _providerID;

		public OrderLineViewController OrderLineViewController  { 
			get {
				return _orderLineViewController;
			}
			set {
				_orderLineViewController = value;
				if (TableView.Source != null && TableView.Source is ProductListSource) {
					var productListSource = TableView.Source as ProductListSource;
					productListSource.OrderLineViewController = _orderLineViewController;
				} else {
					ProductListSource.OrderLineViewController = _orderLineViewController;
				}
			} }

		public long ProviderID { 
			get {
				return _providerID;
			}
			set {
				_providerID = value;
				if (TableView.Source != null && TableView.Source is ProductListSource) {
					var productListSource = TableView.Source as ProductListSource;
					productListSource.ProviderID = _providerID;
				} else {
					ProductListSource.ProviderID = _providerID;
				}
			}
		}

		public ProductListController (IntPtr handle) : base (handle)
		{
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

			// ADDITIONAL
			this.searchBar.ScopeButtonTitles = new string[3]{ "Product Name", "Product Code", "Product Group" };
			this.searchBar.ShowsScopeBar = true;
			this.searchBar.SizeToFit ();


			// Register the TableView's data source
//			ProductListSource.OrderLineViewController = OrderLineViewController;
//			ProductListSource.ProviderID = ProviderID;
			TableView.Source = ProductListSource;
			TableView.ReloadData ();
			if (OrderLineViewController != null && OrderLineViewController.SelectedProduct != null) {
				var selectedProduct = OrderLineViewController.SelectedProduct;
				// workaround for now for #43 - modify item quantity
				searchBar.SelectedScopeButtonIndex = 1;
				ProductListSource.Filter = ProductListSource.SearchScope.PRODUCT_CODE;
				searchBar.Text = selectedProduct.Code;
				ProductListSource.searchString = selectedProduct.Code;
				TableView.ReloadData ();
			}

			searchBar.SelectedScopeButtonIndexChanged += (object sender, UISearchBarButtonIndexEventArgs e) => {
				ProductListSource.Filter = (ProductListSource.SearchScope) (int) e.SelectedScope;
				if (searchBar.Text != null && searchBar.Text.Trim().Length > 0)
					TableView.ReloadData();
				switch (ProductListSource.Filter) {
				case ProductListSource.SearchScope.PRODUCT_NAME:
					searchBar.Placeholder = "Product Name";
					break;
				case ProductListSource.SearchScope.PRODUCT_CODE:
					searchBar.Placeholder = "Product Code";
					break;
				case ProductListSource.SearchScope.PRODUCT_GROUP:
					searchBar.Placeholder = "Product Group";
					break;
				default:
					searchBar.Placeholder = "Product Name";
					break;
				}
			};

			searchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => {
				ProductListSource.searchString = searchBar.Text;
				TableView.ReloadData();
			};
			
			searchBar.SearchButtonClicked += (sender, e) => {
				searchBar.ShowsCancelButton = false;
				searchBar.ResignFirstResponder();
			};
			
			searchBar.OnEditingStarted += (sender, e) => {
				searchBar.ShowsCancelButton = true;
			};
			
			searchBar.CancelButtonClicked += (sender, e) => {
				searchBar.ShowsCancelButton = false;
				searchBar.ResignFirstResponder();
			};

			// TODO: find out why ResignFirstResponder doesn't work on modal windows
//			searchBar.SearchButtonClicked += (sender, e) => {
//				var sb = sender as UISearchBar;
//				sb.ResignFirstResponder();
//			};
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			// TODO: this makes the window move to the right/left, weird.  
//			if (ProductListSource.RowsInSection(TableView, 0) == 1) {
//				var indexPath = NSIndexPath.FromRowSection (0, 0);
//				TableView.SelectRow (indexPath, false, UITableViewScrollPosition.None);
//				ProductListSource.RowSelected (TableView, indexPath);
//			}
		}
	}
}

