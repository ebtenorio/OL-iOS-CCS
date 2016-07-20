// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using UIKit;

using OneTradeCentral.DTOs;
using System.Collections.Generic;

namespace OneTradeCentral.iOS
{
	public partial class StoreListingsController : UIViewController
	{
		public OrderViewController OrderController { get; set; }
		public long ProviderID { get; set; }

		static DALFacade dalFacade;
		static IList<Provider> providerList;
	

		StoreListingsSource StoreListSource;

		Customer _customer {
			get;
			set;
		}

		public Customer Customer { 
			get {
				return this._customer;
			} 
			set {
				this._customer = value;
			}
		}

		public void RefreshCustomerList(long providerID){
			StoreListSource = new StoreListingsSource (this, providerID);
			this.ProviderID = providerID;
			TableView.ReloadData ();

		}

		public StoreListingsController (IntPtr handle) : base (handle)
		{
			dalFacade = new DALFacade ();
			StoreListSource = new StoreListingsSource (this, 0);

		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			providerList = dalFacade.getDistinctProviderList ();
			if (providerList.Count > 0) {
				this.RefreshCustomerList (providerList [0].ID);
				this.StoreProvider.Text = providerList [0].Name;
			}


			// Additional codes for SearchBar Scopes
			this.SearchBar.ScopeButtonTitles = new string[2]{ "Store Name", "State" };
			this.SearchBar.ShowsScopeBar = true;
			this.SearchBar.SizeToFit ();


			TableView.Source = StoreListSource;

			SearchBar.SelectedScopeButtonIndexChanged += (object sender, UISearchBarButtonIndexEventArgs e) => {
				StoreListSource.Filter = (StoreListingsSource.SearchScope) ((int) e.SelectedScope);
				if (SearchBar.Text != null && SearchBar.Text.Trim().Length > 0)
					TableView.ReloadData();
				switch (e.SelectedScope) {
				case (int) StoreListingsSource.SearchScope.NAME:
					SearchBar.Placeholder = "Store Name";
					break;
				case (int) StoreListingsSource.SearchScope.STATE:
					SearchBar.Placeholder = "State";
					break;
				}
			};

			SearchBar.TextChanged += (object sender, UISearchBarTextChangedEventArgs e) => {
				StoreListSource.searchString = SearchBar.Text;
				TableView.ReloadData ();
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

//		public override void ViewDidAppear (bool animated)
//		{
//			base.ViewDidAppear (animated);
//
////			// Default Provider Picker here.
////			if (dalFacade.getDistinctProviderList ().Count > 0) {
////				this.StoreProvider.Text = providerList[0].Name;
////			}
//		}
			
		public override bool ShouldPerformSegue (string segueIdentifier, NSObject sender)
		{
			if (segueIdentifier == "EditCustomerSegue" && this._customer == null)
				return false;
			else
				return base.ShouldPerformSegue (segueIdentifier, sender);
		}


		public override void PrepareForSegue (UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue (segue, sender);

			switch (segue.Identifier) {
			case "StoreProviderSegue":
				// pass this controller to the next controller
				CoreGraphics.CGSize pSize = new CoreGraphics.CGSize (300, 216);
				var storeProviderController = segue.DestinationViewController as StoreProviderController;
				storeProviderController.storeListingController = this;
				storeProviderController.PreferredContentSize = pSize;
				break;
			case "EditCustomerSegue":
				var customerEditorController = segue.DestinationViewController as StoresViewController;
				customerEditorController.StoreListingsController = this;
				customerEditorController.Customer = this._customer;
				customerEditorController.ProviderName = this.StoreProvider.Text;

				break;
			}

		}

		partial void doubleTap (Foundation.NSObject sender) {

			if (OrderController == null)

				PerformSegue("EditCustomerSegue", this);
			else {
				OrderController.SelectedCustomer=_customer;
				NavigationController.PopViewController(true);
			}
		}


	}
}