
using System;
using CoreGraphics;
using System.Linq;

using Foundation;
using UIKit;
using System.Collections.Generic; 
using OneTradeCentral.DTOs;

namespace OneTradeCentral.iOS
{
	public partial class ProductsListingsController : UIViewController
	{
		ProductsListingsSource productsListingsSource = new ProductsListingsSource();
		private OrderLineViewController _orderLineViewController;
		private long _providerID;

		static DALFacade dalFacade;
		static IList<Provider> providerList;


		public OrderLineViewController OrderLineViewController  { 
			get {
				return _orderLineViewController;
			}
			set {
				_orderLineViewController = value;
				if (TableView.Source != null && TableView.Source is ProductsListingsSource) {
					var productListSource = TableView.Source as ProductsListingsSource;
					productListSource.OrderLineViewController = _orderLineViewController;
				} else {
					productsListingsSource.OrderLineViewController = _orderLineViewController;
				}
			} }

		public long ProviderID { 
			get {
				return _providerID;
			}
			set {
				_providerID = value;
				if (TableView.Source != null && TableView.Source is ProductsListingsSource) {
					var productListSource = TableView.Source as ProductsListingsSource;
					productListSource.ProviderID = _providerID;
				} else {
					productsListingsSource.ProviderID = _providerID;
				}
			}
		}


		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);

			switch (segue.Identifier) {
			case "ProductProviderSegue":
				// pass this controller to the next controller
				CGSize pSize = new CGSize (300, 216);
				var productsProviderController = segue.DestinationViewController as ProductsProviderController;
				productsProviderController.productsListingController = this;
				productsProviderController.PreferredContentSize = pSize;
				break;
			}
		}

		public void RefreshProductList(long providerID){
			
			this._providerID = providerID;
			TableView.ReloadData ();
		}

		public ProductsListingsController (IntPtr handle) : base (handle)
		{
				// Initialize the Source here?
			dalFacade = new DALFacade ();
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

			providerList = dalFacade.getDistinctProviderList ();
			if (providerList.Count > 0) {
				this.RefreshProductList (providerList [0].ID);
				this.ProductProvider.Text = providerList [0].Name;
			}

			// ADDITIONAL
			this.SearchBar.ScopeButtonTitles = new string[3]{ "Product Name", "Product Code", "Product Group" };
			this.SearchBar.ShowsScopeBar = true;
			this.SearchBar.SizeToFit ();


			// Register the TableView's data source
			productsListingsSource.OrderLineViewController = OrderLineViewController;
			productsListingsSource.ProviderID = ProviderID;


			TableView.Source = productsListingsSource;
			if (OrderLineViewController != null && OrderLineViewController.SelectedProduct != null) {
				var selectedProduct = OrderLineViewController.SelectedProduct;
				// workaround for now for #43 - modify item quantity
				SearchBar.SelectedScopeButtonIndex = 1;
				productsListingsSource.Filter = ProductsListingsSource.SearchScope.PRODUCT_CODE;
				SearchBar.Text = selectedProduct.Code;
				productsListingsSource.searchString = selectedProduct.Code;
				TableView.ReloadData ();
			}

			SearchBar.SelectedScopeButtonIndexChanged += (object sender, UISearchBarButtonIndexEventArgs e) => {
				productsListingsSource.Filter = (ProductsListingsSource.SearchScope) ((int)e.SelectedScope);
				if (SearchBar.Text != null && SearchBar.Text.Trim().Length > 0)
					TableView.ReloadData();
				switch (productsListingsSource.Filter) {
				case ProductsListingsSource.SearchScope.PRODUCT_NAME:
					SearchBar.Placeholder = "Product Name";
					break;
				case ProductsListingsSource.SearchScope.PRODUCT_CODE:
					SearchBar.Placeholder = "Product Code";
					break;
				case ProductsListingsSource.SearchScope.PRODUCT_GROUP:
					SearchBar.Placeholder = "Product Group";
					break;
				default:
					SearchBar.Placeholder = "Product Name";
					break;
				}
			};

			SearchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => {
				productsListingsSource.searchString = SearchBar.Text;	
				TableView.ReloadData();
			};

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

			// TODO: find out why ResignFirstResponder doesn't work on modal windows
			//			searchBar.SearchButtonClicked += (sender, e) => {
			//				var sb = sender as UISearchBar;
			//				sb.ResignFirstResponder();
			//			};
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

//			// TODO: this makes the window move to the right/left, weird.  
//			if (productsListingsSource.RowsInSection(TableView, 0) == 1) {
//							var indexPath = NSIndexPath.FromRowSection (0, 0);
//							TableView.SelectRow (indexPath, false, UITableViewScrollPosition.None);
//				productsListingsSource.RowSelected (TableView, indexPath);
//						}
		}
//

	}
}

