
using System;
using CoreGraphics;
using System.Linq;
using Foundation;
using UIKit;
using System.Collections.Generic; 
using OneTradeCentral.DTOs;

namespace OneTradeCentral.iOS
{
	public partial class ProductSelectionViewController : UIViewController
	{
		ProductSelectionSource ProductSelectionSource = new ProductSelectionSource();

		private OrderLineViewController _orderLineViewController;

		private long _providerID;
		private bool _isRegularOrder;
		private DateTime _dateCriteria;
		private DateTime _releaseDate;

//		static DALFacade dalFacade;
//		static IList<Provider> providerList;


		public OrderLineViewController OrderLineViewController  { 
			get {
				return _orderLineViewController;
			}
			set {
				_orderLineViewController = value;

				// Commented Oct 16 2014.
//				if (TableView.Source != null && TableView.Source is ProductSelectionSource) {
//					var productListSource = TableView.Source as ProductSelectionSource;
//					productListSource.OrderLineViewController = _orderLineViewController;
//				} else {
//					ProductSelectionSource.OrderLineViewController = _orderLineViewController;
//				}
			} }

		public long ProviderID { 
			get {
				return this._providerID;
			}
			set {
				this._providerID = value;
			}
		}

		public bool IsRegularOrder { 
			get {
				return this._isRegularOrder;
			}
			set {
				this._isRegularOrder = value;
			}
		}

		public DateTime DateCriteria { 
			get {
				return this._dateCriteria;
			}
			set {
				this._dateCriteria = value;
			}
		}


		public DateTime ReleaseDate { 
			get {
				return this._releaseDate;
			}
			set {
				this._releaseDate = value;
			}
		}

//		public void RefreshProductList(long providerID, bool isRegularOrder){
//			this._providerID = providerID;
//
//			// OrderType Testing
//			this._isRegularOrder = isRegularOrder;
//			TableView.ReloadData ();
//		}

		public ProductSelectionViewController (IntPtr handle) : base (handle)
		{
			// Initialize the Source here?
			//dalFacade = new DALFacade ();
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
			ProductSelectionSource.OrderLineViewController = OrderLineViewController;
			ProductSelectionSource.ProviderID = ProviderID;
			ProductSelectionSource.IsRegularOrder = IsRegularOrder;
			ProductSelectionSource.DateCriteria = DateCriteria;
			ProductSelectionSource.ReleaseDate = ReleaseDate;
					
			this.TableView.Source = ProductSelectionSource;
			if (OrderLineViewController != null && OrderLineViewController.SelectedProduct != null) {
				var selectedProduct = OrderLineViewController.SelectedProduct;
				// workaround for now for #43 - modify item quantity
				searchBar.SelectedScopeButtonIndex = 1;
				ProductSelectionSource.Filter = ProductSelectionSource.SearchScope.PRODUCT_CODE;
				searchBar.Text = selectedProduct.Code;
				ProductSelectionSource.searchString = selectedProduct.Code;
				TableView.ReloadData ();
			}

			searchBar.SelectedScopeButtonIndexChanged += (object sender, UISearchBarButtonIndexEventArgs e) => {
				ProductSelectionSource.Filter = (ProductSelectionSource.SearchScope) ((int)e.SelectedScope);
				if (searchBar.Text != null && searchBar.Text.Trim().Length > 0)
					TableView.ReloadData();
				switch (ProductSelectionSource.Filter) {
				case ProductSelectionSource.SearchScope.PRODUCT_NAME:
					searchBar.Placeholder = "Product Name";
					break;
				case ProductSelectionSource.SearchScope.PRODUCT_CODE:
					searchBar.Placeholder = "Product Code";
					break;
				case ProductSelectionSource.SearchScope.PRODUCT_GROUP:
					searchBar.Placeholder = "Product Group";
					break;
				default:
					searchBar.Placeholder = "Product Name";
					break;
				}
			};

			searchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => {
				ProductSelectionSource.searchString = searchBar.Text;	
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
				
		}

	}
}

