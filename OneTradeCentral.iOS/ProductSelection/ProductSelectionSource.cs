using System;
using CoreGraphics;
using System.Collections.Generic;

using Foundation;
using UIKit;

using OneTradeCentral.DTOs;
using OneTradeCentral.DAO;

namespace OneTradeCentral.iOS
{
	public class ProductSelectionSource : UITableViewSource
	{
		static DALFacade dalFacade = new DALFacade ();
		//		static IList<Product> ProductList = new List<Product>();
		public long ProviderID { get; set; }
		public bool IsRegularOrder { get; set; }
		public DateTime DateCriteria { get; set; }
		public DateTime ReleaseDate { get; set; } = DateTime.Now;
		public int SectionHeaderCount = 0;
//		public bool HasAtLeastOneProduct { get; set;}

		public enum SearchScope { PRODUCT_NAME=0, PRODUCT_CODE=1, PRODUCT_GROUP=2 }
		public string searchString { get; set; }
		public OrderLineViewController OrderLineViewController { get; set;}
		public SearchScope Filter { get; set; }
		private IList<ProductGroup> ProductGroupList { get; set; }
		public Dictionary<string, IList<Product>> ProductDictionary; // changed to public 


		public ProductSelectionSource ()
		{

		}

		public void PopulateProductDictionary ()
		{
			foreach (var group in ProductGroupList) {
				if (ProviderID > 0) {
					var productList = dalFacade.getProductListByProductGroupAndProviderID (ProviderID, group.Name, IsRegularOrder, DateCriteria.ToLocalTime().Date);
					if (productList != null && productList.Count != 0){

						if (!ProductDictionary.ContainsKey (group.Name)) {
							ProductDictionary.Add (group.Name, productList);
						}

					}
				} else {

					ProductDictionary.Add (group.Name, dalFacade.getProductListByProductGroup (ProviderID, group.Name));
				}
			}

		}

		public bool HasProductGroups(){
			bool tmpHasProductGroups = false;

			IList<ProductGroup> _ProductGroupList = dalFacade.getProductGroupsByProviderID (ProviderID);
			foreach (var group in _ProductGroupList) {
				var productList = dalFacade.getProductListByProductGroupAndProviderID (ProviderID, group.Name, IsRegularOrder, DateCriteria.Date);
				if (productList != null && productList.Count != 0) {
					tmpHasProductGroups = true;

				}

			}

			return tmpHasProductGroups;
		}


		public void RefreshData ()
		{
			ProductDictionary = new Dictionary<string, IList<Product>>();
			if (searchString == null || searchString.Trim ().Length <= 0) {
				if (ProviderID > 0)
					ProductGroupList = dalFacade.getProductGroupsByProviderID (ProviderID);
				else 
					ProductGroupList = dalFacade.getAllProductGroups ();

				PopulateProductDictionary ();
			} else {
				switch (Filter) {
				case SearchScope.PRODUCT_NAME:
					if (ProviderID > 0)
						ProductGroupList = dalFacade.getProductGroupsByProductName (ProviderID, searchString, IsRegularOrder);
					else
						ProductGroupList = dalFacade.getProductGroupsByProductName (searchString);
					foreach (var group in ProductGroupList) {
						ProductDictionary.Add (group.Name, dalFacade.getGroupedProductsByProductName (ProviderID, group.Name, searchString, IsRegularOrder, DateCriteria.Date));
					}
					break;
				case SearchScope.PRODUCT_CODE:
					if (ProviderID > 0)
						ProductGroupList = dalFacade.getProductGroupsByProductCode (ProviderID, searchString, IsRegularOrder);
					else 
						ProductGroupList = dalFacade.getProductGroupsByProductCode (searchString);
					foreach (var group in ProductGroupList) {
						ProductDictionary.Add (group.Name, dalFacade.getGroupedProductsByProductCode (ProviderID, group.Name, searchString, IsRegularOrder));
					}
					break;
				case SearchScope.PRODUCT_GROUP:
					if (ProviderID > 0)
						ProductGroupList = dalFacade.getProductGroupsByName (ProviderID, searchString, IsRegularOrder);
					else
						ProductGroupList = dalFacade.getProductGroupsByName (searchString);
					PopulateProductDictionary ();
					break;
				default:
					if (ProviderID > 0)
						ProductGroupList = dalFacade.getProductGroupsByName (ProviderID, searchString, IsRegularOrder);
					else
						ProductGroupList = dalFacade.getProductGroupsByName (searchString);
					PopulateProductDictionary ();
					break;
				}
			}
		}

		public override nint NumberOfSections (UITableView tableView)
		{

			if (ProviderID != 0) {
				RefreshData ();
				return ProductGroupList.Count;
			}

			return 0;

		}

		public override nfloat GetHeightForHeader (UITableView tableView, nint section)
		{
			return 40;
		}

	
		IList<Product> ProductList (int section)
		{
			string key = ProductGroupList [section].Name;

			if (ProductDictionary.ContainsKey (key)) {
				IList<Product> productList = ProductDictionary [key];

				if (productList.Count == 0) {
					ProductGroupList [section].Name = ""; //"No products for " + key;
					return null;
				}

				this.SectionHeaderCount += 1;
				return productList;

			} else {
				ProductGroupList [section].Name = ""; //"No products for " + key;
				return null;
			}

		}

		Product SelectedProduct (NSIndexPath indexPath) {
			var productList = ProductList (indexPath.Section);
			return productList [indexPath.Row];
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			if (ProductList((int)section) == null)
				return 0;
			else
				return ProductList ((int)section).Count;
		}

		public override string TitleForHeader (UITableView tableView, nint section)
		{
			return ProductGroupList [(int)section].Name;
		}

		public override nfloat EstimatedHeightForHeader (UITableView tableView, nint section)
		{			
			if (ProductList((int)section) == null) {
				return 0.0f;
			} else {
				if (ProductList ((int)section).Count == 0) {
					return 0.0f;
				} else { 
					return 40f; // my default height for the row. please check Xcode Interface Builder foro the MainStoryBoard
				}
			}

		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (OrderLineViewController != null) {
				OrderLineViewController.SelectedProduct = SelectedProduct (indexPath);
			}
			//			OrderLineViewController.SelectedProduct = getFilteredList()[indexPath.Row];
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell ("ProductCell");
			if (cell == null)
				cell = new UITableViewCell ();

			Product product;
			var productList = ProductList(indexPath.Section);
			if (productList != null && productList.Count > 0) {
				product = productList [indexPath.Row];
			} else {
				product = new Product ();
			}

			cell.TextLabel.Text = product.Name;
			if (cell.DetailTextLabel != null) {
				cell.DetailTextLabel.Text = String.Format ("Code: {0}, Units: {1}", product.Code, product.SKU);
			}
			return cell;
		}

	
	}
}

