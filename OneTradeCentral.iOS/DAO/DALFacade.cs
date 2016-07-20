using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using OneTradeCentral.DAO;
using OneTradeCentral.Utility;
using OneTradeCentral.iOS;
using System.Data;
using UIKit;
using Foundation;


namespace OneTradeCentral.DTOs
{
	public class DALFacade
	{
		//		IDAOFactory dao = new DataGenerator();
		IDAOFactory dao = new SQLiteDAOFactory ();
		//		private static string OTC_PRODUCT_URL = "http://otc.herokuapp.com/products.json";
		// TODO: this is a placeholder for static data, pending the availability of web service functions to support these
		protected static IList<Country> CountryList = null;
		protected static IList<UOM> UOMList = null;
		protected static IList<ProductCategory> ProductCategoryList = null;

		public DALFacade ()
		{
		}
		// ------ User Account ------
		public UserAccount getUserAccount ()
		{
			var userAccount = dao.getUserAccount ();
			return userAccount;
		}

		public bool hasCustomerOldID (Order order){
			return dao.hasCustomerOldID (order);
		}

		public void saveUserAccount (UserAccount userAccount)
		{
			dao.saveUserAccount (userAccount);
		}
		// ------ App Info ------
		public AppInfo getApplicationInfo ()
		{
			return dao.getApplicationInfo ();
		}

		public void updateApplicationInfo (AppInfo appInfo)
		{
			dao.updateApplicationInfo (appInfo);
		}

		public DateTime getLastSyncDate ()
		{
			return dao.getApplicationInfo ().LastSyncDate;
		}
		// ------ Data Synchronization ------
		public void synchronizeReferenceData ()
		{
			// synchronize records via web service
			WebServiceFacade ws = new WebServiceFacade ();
//			var customerList = ws.downloadCustomerList ();
//			var productList = ws.downloadProductList ();
//			var warehouseList = ws.downloadWarehouseList ();

			UserAccount userAccount = getUserAccount ();

			DataSet referenceData = ws.getReferenceData (
				userAccount.SalesOrgID,
				userAccount.AccountID,
				NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"].ToString() + "-" + NSBundle.MainBundle.InfoDictionary["CFBundleVersion"].ToString(),
				UIDevice.CurrentDevice.SystemVersion
			);


			// clean up the DB
			dao.resetReferenceTables ();

			// tblProviderProduct
			DataTable productTable = referenceData.Tables ["tblProviderProduct"];
			DataRow[] productRows = productTable.Select ();
			if (productRows != null && productRows.Length > 0) {
				for (int i = 0; i < productRows.Length; i++) {
					Product product = new Product ();
					// default values

					product.OldProductID = (productRows [i] ["OldID"] == DBNull.Value) ? 0 : (long)productRows [i] ["OldID"];

					product.Inactive = false;
					product.Deleted = false;
					product.Category = "";
					product.VendorName = "";

					// Absent in the original				
					product.SKU = productRows [i] ["PrimarySKU"].ToString ();
					product.ID = (long)productRows [i] ["ProductID"];
					product.GTINCode = productRows [i] ["GTINCode"].ToString();
					product.Name = productRows [i] ["ProductDescription"].ToString();
					product.UOM = productRows [i] ["ProductUOM"].ToString();
					product.ProviderID = (long)productRows [i] ["ProviderID"];
					product.Code = productRows [i] ["ProviderProductCode"].ToString();
					product.StartDate = productRows [i] ["StartDate"] != null ? (DateTime)productRows [i] ["StartDate"] : DateTime.MinValue;
					product.EndDate = productRows [i] ["EndDate"] != null ? (DateTime)productRows [i] ["EndDate"] : DateTime.MaxValue;

					int productPK = dao.saveProduct (product);

					if (productPK <= 0) {
						Logger.log ("Error Saving Product: " + product.Name);
					}
				}
			} else {
				Logger.log ("GetReferenceData: No Products retrieved");
			}

			// tblProductGroup
			DataTable productGroupTable = referenceData.Tables ["tblProductGroup"];
			DataRow[] productGroupRows = productGroupTable.Select ();
			if (productGroupRows != null && productGroupRows.Length > 0) {
				for (int i = 0; i < productGroupRows.Length; i++) {
					try {
						ProductGroup productGroup = new ProductGroup();

						productGroup.ID = (int) productGroupRows[i] ["ProductGroupID"];
						productGroup.Name = productGroupRows[i] ["ProductGroupText"].ToString();
						productGroup.SalesOrgID = (long) productGroupRows[i] ["SalesOrgID"];
						var pk = dao.saveProductGroup(productGroup);

						if (pk <= 0) 
							Logger.log("Error Saving ProductGroup: " + productGroup.Name);

					} catch (Exception e) {
						Logger.log ("Sync Error ProductGroup: " + productGroupRows [i] ["ProductGroupID"] + ": " + e.Message);
					}
				}
			} else {
				Logger.log ("GetReferenceData: No ProductGroups retrieved");
			}

			// tblProductGroupLine == ProductGroupLookup
			DataTable productGroupLookupTable = referenceData.Tables ["tblProductGroupLine"];
			DataRow[] productGroupLookupRows = productGroupLookupTable.Select ();
			if (productGroupLookupRows != null && productGroupLookupRows.Length > 0) {
				for (int i = 0; i < productGroupLookupRows.Length; i++) {
					try {
						ProductGroupLookup pgl = new ProductGroupLookup();
						pgl.ProductID = (long) productGroupLookupRows[i]["ProductID"];
						pgl.ProductGroupID = (int) productGroupLookupRows[i]["ProductGroupID"];
						var pk = dao.saveProductGroupLookup (pgl);

						if (pk <= 0) 
							Logger.log("Error Saving ProductGroupLookup (" + pgl.ProductGroupID + ", " + pgl.ProductID + ")");

					} catch (Exception e) {
						Logger.log ("Sync Error ProductGroupLookup: " + e.Message);
					}
				}
			} else {
				Logger.log ("GetReferenceData: No ProductGroupLookup records retrieved");
			}

			// tblCustomer
			DataTable customerTable = referenceData.Tables ["tblCustomer"];
			DataRow[] customerRows = customerTable.Select ();
			if (customerRows != null && customerRows.Length > 0) {
				for (int i = 0; i < customerRows.Length; i++) {
					try {
						Customer customer = new Customer ();

						customer.ID = (long)customerRows [i] ["CustomerID"];
						customer.OldCustomerID = (customerRows [i] ["OldID"]  == DBNull.Value) ? 0: (long)customerRows [i] ["OldID"] ;

						customer.Name = customerRows [i] ["CustomerName"] != null ? customerRows [i] ["CustomerName"].ToString() : null;
						customer.SalesRepAccountID = customerRows [i] ["SalesRepAccountID"] != null ? (long)customerRows [i] ["SalesRepAccountID"] : 0;
						customer.Code = customerRows [i] ["ProviderCustomerCode"] != null ? customerRows [i] ["ProviderCustomerCode"].ToString() : null;
						customer.ProviderID = customerRows [i] ["ProviderID"] != null ? (long)customerRows [i] ["ProviderID"] : 0;
						customer.ContactFirstName = customerRows [i] ["FirstName"] != null ? customerRows [i] ["FirstName"].ToString() : null;
						customer.ContactLastName = customerRows [i] ["LastName"] != null ? customerRows [i] ["LastName"].ToString() : null;
						customer.ContactEmail = customerRows [i] ["Email"] != null ? customerRows [i] ["Email"].ToString() : null;
						customer.ContactFax = customerRows [i] ["Fax"] != null ? customerRows [i] ["Fax"].ToString() : null;
						customer.ContactMobile = customerRows [i] ["Mobile"] != null ? customerRows [i] ["Mobile"].ToString() : null;
						customer.ContactNumber = customerRows [i] ["Phone"] != null ? customerRows [i] ["Phone"].ToString() : null;
						customer.StartDate = customerRows [i] ["StartDate"] != null ? (DateTime)customerRows [i] ["StartDate"] : DateTime.MinValue;
						customer.EndDate = customerRows [i] ["EndDate"] != null ? (DateTime)customerRows [i] ["EndDate"] : DateTime.MaxValue;
						customer.StateCode = customerRows [i] ["StateCode"] != null ? customerRows [i] ["StateCode"].ToString() : null;;
						customer.StateName = customerRows [i] ["StateName"] != null ? customerRows [i] ["StateName"].ToString() : null;;


						dao.saveCustomer (customer);
					} catch (Exception e) {
						Logger.log ("Sync Error Customer ID: " + customerRows [i] ["CustomerID"] + ": " + e.Message);
					}

				}
			}

			// tblProvider 
			DataTable providerTable = referenceData.Tables ["tblProvider"];
			DataRow[] providerRows = providerTable.Select ();
			if (providerRows != null && providerRows.Length > 0) {
				for (int i = 0; i < providerRows.Length; i++) {
					Provider provider = new Provider ();

					provider.OldProviderID = (providerRows [i] ["OldID"] == DBNull.Value) ? 0 : (int)providerRows [i] ["OldID"];
					provider.ID = (long) providerRows [i] ["ProviderID"];
					provider.Code = providerRows [i] ["ProviderCode"].ToString();
					provider.Phone = providerRows [i] ["BusinessNumber"].ToString();
					provider.Name = providerRows [i] ["ProviderName"].ToString();
					provider.SalesOrgID = (long) providerRows [i] ["SalesOrgID"];
					provider.AddressID = (long) providerRows [i] ["AddressID"];
					provider.Deleted = (bool) providerRows [i] ["Deleted"];
					provider.InActive = (bool) providerRows [i] ["InActive"];

					dao.saveProvider (provider);
				}
			} else {
				Logger.log ("GetReferenceData: No Providers retrived.");
			}

			DataTable providerWarehouseTable = referenceData.Tables ["tblProviderWarehouse"];
			DataRow[] providerWarehouseRows = providerWarehouseTable.Select ();
			if (providerWarehouseRows != null && providerWarehouseRows.Length > 0) {
				for (int i = 0; i < providerWarehouseRows.Length; i++) {
					ProviderWarehouse providerWarehouse = new ProviderWarehouse ();

					providerWarehouse.OldProviderWarehouseID = (providerWarehouseRows [i] ["OldID"] == DBNull.Value) ? 0 : (long)providerWarehouseRows [i] ["OldID"];
					providerWarehouse.ID = (int)providerWarehouseRows [i] ["ProviderWarehouseID"];
					providerWarehouse.ProviderID = (long)providerWarehouseRows [i] ["ProviderID"];
					providerWarehouse.Code = providerWarehouseRows [i] ["ProviderWarehouseCode"].ToString ();
					providerWarehouse.Name = providerWarehouseRows [i] ["ProviderWarehouseName"].ToString ();
					providerWarehouse.StartDate = providerWarehouseRows [i] ["StartDate"] != null ? (DateTime)providerWarehouseRows [i] ["StartDate"] : DateTime.MinValue;
					providerWarehouse.EndDate = providerWarehouseRows [i] ["EndDate"] != null ? (DateTime)providerWarehouseRows [i] ["EndDate"] : DateTime.MaxValue;

					dao.saveProviderWarehouse (providerWarehouse);

					// Some code here after the initial code has been checked.
				}
			} else {
				Logger.log ("GetReferenceData: No ProviderWarehouse retrived.");
			}

		}

		/// <summary>
		/// Recreates the order tables.
		/// </summary>
		public void resetOrderTables ()
		{
			dao.resetOrderTables ();
		}
		//------ Record Retrieval ------
		public IList<Customer> getActiveCustomerList ()
		{
			return dao.getActiveCustomerList ();
		}

		public IList<Customer> getActiveUniqueCustomerList ()
		{
			return dao.getActiveUniqueCustomerList ();
		}

		public Customer getCustomerByProviderID(long providerID, long customerID) {
			return dao.getCustomerByProviderID (providerID, customerID);
		}

		public Customer getCustomerByOldID(long OldID){
			return dao.getCustomerByOldID (OldID);
		}

		public List<Customer> getAllCustomerByProviderID(long providerID) {
			return dao.getAllCustomerByProviderID (providerID);
		}


		public IList<Customer> getAllCustomerList ()
		{
			return dao.getAllCustomerList ();
		}

		public IList<Distributor> getDistributorList ()
		{
			return dao.getDistributorList ();
		}

		public IList<Order> getOrderList (int salesRepAccountID)
		{
			return dao.getOrderList (salesRepAccountID);
		}

		public void populateOrder (Order order)
		{
			dao.populateOrder (order);
		}

		public void populateOrderWithOldIDs (Order order)
		{
			dao.populateOrderWithOldIDs (order);
		}

		public IList<OrderLine> getOrderLineListByOrderPK (int orderPK)
		{
			return dao.getOrderLineListByOrderPK (orderPK);
		}

		public IList<Order> getOrderListByStoreName (string storeName)
		{
			return dao.getOrderListByStoreName (storeName);
		}

		public IList<Order> getOrderListByStoreID (string storeID)
		{
			return dao.getOrderListByStoreID (storeID);
		}

		public IList<Order> getOrderListByOrderNumber (string orderNumber)
		{
			return dao.getOrderListByOrderNumber (orderNumber);
		}

		public IList<Order> getOrderListByOrderDate (DateTime orderDate)
		{
			return dao.getOrderListByOrderDate (orderDate);
		}

		public IList<Order> getAllPendingOrders (int salesRepAccountID)
		{
			return dao.getAllPendingOrders (salesRepAccountID);
		}

		public IList<PaymentTerms> getPaymentTermsList ()
		{
			return dao.getPaymentTermsList ();
		}
		// ------ Product ------
		public IList<Product> getProductList ()
		{
			return dao.getProductList ();
		}

		public IList<Product> getProductListByProductName (long providerID, string productName, bool? IsRegularOrder)
		{
			return dao.getProductListByProductName (providerID, productName, IsRegularOrder);
		}

		public IList<Product> getProductListByOldID (long oldID)
		{
			return dao.getProductListByOldID(oldID);
		}

		public IList<Product> getProductListByProductCode (long providerID, string productCode)
		{
			return dao.getProductListByProductCode (providerID, productCode);
		}
		// ------ Product Group ------
		public IList<ProductGroup> getAllProductGroups ()
		{
			return dao.getAllProductGroups ();
		}

		public int CountProductGroups() {
			return dao.CountProductGroups ();
		}

		public int CountFutureProductGroups() {
			return dao.CountFutureProductGroups ();
		}

		public IList<ProductGroup> getProductGroupsByProviderID(long providerID) {
			return dao.getProductGroupsByProviderID (providerID);
		}

		public IList<ProductGroup> getProductGroupsByName (long providerID, string name, bool? IsRegularOrder)
		{
			return dao.getProductGroupsByName (providerID, name, IsRegularOrder);
		}

		public IList<ProductGroup> getProductGroupsByName (string name)
		{
			return dao.getProductGroupsByName (name);
		}

		public IList<ProductGroup> getProductGroupsByProductCode (long providerID, string productCode, bool? IsRegularOrder)
		{
			return dao.getProductGroupsByProductCode (providerID, productCode, IsRegularOrder);
		}

		public IList<ProductGroup> getProductGroupsByProductCode (string productCode)
		{
			return dao.getProductGroupsByProductCode (productCode);
		}

		public IList<ProductGroup> getProductGroupsByProductName (long providerID, string productName, bool? IsRegularOrder)
		{
			return dao.getProductGroupsByProductName (providerID, productName, IsRegularOrder);
		}

		public IList<ProductGroup> getProductGroupsByProductName (string productName)
		{
			return dao.getProductGroupsByProductName (productName);
		}

		/// <summary>
		/// Returns the list of products under the given product group.
		/// </summary>
		/// <returns>The product list by product group.</returns>
		/// <param name="productGroup">Product group.</param>
		public IList<Product> getProductListByProductGroup (long providerID, string productGroup)
		{
			return dao.getProductListByProductGroup (providerID, productGroup);
		}

		public IList<Product> getProductListByProductGroupAndProviderID(long providerID, string productGroup, bool? IsRegularOrder, DateTime DateCriteria) 
		{
			return dao.getProductListByProductGroupAndProviderID (providerID, productGroup, IsRegularOrder, DateCriteria);
		}

		public IList<Product> getGroupedProductsByProductName (long providerID, string groupName, string productName, bool? IsRegularOrder, DateTime ReleaseDate)
		{
			return dao.getGroupedProductsByProductName (providerID, groupName, productName, IsRegularOrder, ReleaseDate);
		}

		public IList<Product> getGroupedProductsByProductCode (long providerID, string groupName, string productCode, bool? IsRegularOrder)
		{
			var productList = dao.getGroupedProductsByProductCode (providerID, groupName, productCode, IsRegularOrder); 
			return productList;
		}
		// ------ Provider ------
		public IList<Provider> getProviderList ()
		{
			return dao.getProviderList ();
		}

		public IList<Provider> getDistinctProviderList ()
		{
			return dao.getDistinctProviderList ();
		}


		public IList<Provider> getProviderListByCustomerID(long customerID) {
			return dao.getProviderListByCustomerID(customerID);
		}


		public IList<Provider> getProviderListByCustomerIDFiltered(long customerID) {
			return dao.getProviderListByCustomerIDFiltered(customerID);
		}

		public Provider getProviderByID (long providerID)
		{
			return dao.getProviderByID (providerID);
		}

		public Provider getProviderByOldID (long oldID)
		{
			return dao.getProviderByOldID (oldID);
		}

		public Provider getProviderByCustomerAndProviderID (long customerID, long providerID)
		{
			return dao.getProviderByCustomerAndProviderID (customerID, providerID);
		}

		// ------ Provider Warehouse ------
		public IList<ProviderWarehouse> getWarehouseList ()
		{
			return dao.getProviderWarehouseList ();
		}

		public IList<ProviderWarehouse> getWarehouseListByProviderID (long providerID)
		{
			return dao.getProviderWarehouseListByProviderID (providerID);
		}

		public ProviderWarehouse getWarehouseByID (int warehouseID)
		{
			return dao.getProviderWarehouseByID (warehouseID);
		}

		public ProviderWarehouse getWarehouseByOldID (int oldID)
		{
			return dao.getProviderWarehouseByOldID (oldID);
		}

		// ------ CRUD Operations ------
		public int SaveCustomer (Customer customer)
		{
			return dao.saveCustomer (customer);
		}

		public void updateCustomerContactDetails (Customer customer)
		{
			dao.updateCustomerContactDetails (customer);
		}

		public void DeleteCustomer (Customer customer)
		{
			dao.deleteCustomer (customer);
		}

		public int SaveProduct (Product product)
		{
			return dao.saveProduct (product);
		}

		public void DeleteProduct (Product product)
		{
			dao.deleteProduct (product);
		}

		public int SaveOrder (Order order)
		{
			var orderPK = dao.saveOrder (order);
			return orderPK;
		}

		public void DeleteOrder (Order order)
		{
			dao.deleteOrder (order);
		}

		public void DeleteOldOrders(int purgePeriod){
			dao.deleteOldOrders (purgePeriod);
		}

		// ------ Record Counter ------

		/// <summary>
		/// Counts all customers.
		/// </summary>
		/// <returns>The all customers.</returns>
		public int CountAllCustomers() 
		{
			return dao.CountAllCustomers ();
		}

		/// <summary>
		/// Counts the products.
		/// </summary>
		/// <returns>The products.</returns>
		public int CountUniqueCustomers ()
		{
			return dao.CountUniqueCustomers ();
		}

		/// <summary>
		/// Counts the distributors.
		/// </summary>
		/// <returns>The distributors.</returns>
		public int CountDistributors ()
		{
			return dao.CountDistributors ();
		}

		/// <summary>
		/// Counts the total number of orders.
		/// </summary>
		/// <returns>The total number of orders.</returns>
		public int CountOrders (int salesRepAccountID)
		{
			return dao.CountOrders (salesRepAccountID);
		}

		/// <summary>
		/// Counts the orders that are pending processing.
		/// </summary>
		/// <returns>The pending orders.</returns>
		public int CountPendingOrders (int salesRepAccountID)
		{
			return dao.CountOrdersByStatus (Order.STATUS.Pending, salesRepAccountID);
		}

		/// <summary>
		/// Counts the orders that are currently being processed.
		/// </summary>
		/// <returns>The orders that are being processed.</returns>
		public int CountProcessingOrders (int salesRepAccountID)
		{
			return dao.CountOrdersByStatus (Order.STATUS.Processing, salesRepAccountID);
		}

		/// <summary>
		/// Counts the number of partially completed orders.
		/// Partially completed orders are orders that still have to upload the signature image.
		/// </summary>
		/// <returns>The partial orders.</returns>
		public int CountPartialOrders (int salesRepAccountID)
		{
			return dao.CountOrdersByStatus (Order.STATUS.Partial, salesRepAccountID);
		}

		/// <summary>
		/// Counts the providers.
		/// </summary>
		/// <returns>The providers.</returns>
		public int CountProviders() 
		{
			return dao.CountProviders ();
		}

		/// <summary>
		/// Counts the orders that have been completely uploaded.
		/// </summary>
		/// <returns>The completed orders.</returns>
		public int CountCompletedOrders (int salesRepAccountID)
		{
			return dao.CountOrdersByStatus (Order.STATUS.Completed, salesRepAccountID);
		}

		/// <summary>
		/// Counts the order lines.
		/// </summary>
		/// <returns>The order lines.</returns>
		public int CountOrderLines (int salesRepAccountID)
		{
			return dao.CountOrderLines (salesRepAccountID);
		}

		/// <summary>
		/// Counts the payment terms.
		/// </summary>
		/// <returns>The payment terms.</returns>
		public int CountPaymentTerms ()
		{
			return dao.CountPaymentTerms ();
		}

		/// <summary>
		/// Counts the product categories.
		/// </summary>
		/// <returns>The product categories.</returns>
		public int CountProductCategories ()
		{
			return dao.CountProductCategories ();
		}

		/// <summary>
		/// Counts the products.
		/// </summary>
		/// <returns>The products.</returns>
		public int CountUniqueProducts ()
		{
			return dao.CountUniqueProducts ();
		}

		/// <summary>
		/// Counts the products.
		/// </summary>
		/// <returns>The products.</returns>
		public int CountFutureProducts ()
		{
			return dao.CountFutureProducts ();
		}

		/// <summary>
		/// Counts all warehouse.
		/// </summary>
		/// <returns>The all warehouse.</returns>
		public int CountAllWarehouse ()
		{
			return dao.CountAllWarehouse ();
		}

		/// <summary>
		/// Counts the unique warehouse.
		/// </summary>
		/// <returns>The unique warehouse.</returns>
		public int CountUniqueWarehouse() 
		{
			return dao.CountUniqueWarehouse ();
		}

		/// <summary>
		/// Counts the warehouse by provider.
		/// </summary>
		/// <returns>The warehouse by provider.</returns>
		/// <param name="provider">Provider.</param>
		public int CountWarehouseByProvider (Provider provider)
		{
			if (provider != null && provider.ID != 0)
				return dao.CountWarehouseByProvider (provider.ID);
			else
				return -1;
		}

		// ------ Log Entries ------
		/// <summary>
		/// Inserts a log entry in the Log table.
		/// </summary>
		/// <param name="log">Log.</param>
		public void saveLog (Log log)
		{
			dao.saveLog (log);
		}

		/// <summary>
		/// Returns all log entries from the beginning of time.
		/// </summary>
		/// <returns>The log all entries.</returns>
		public IList<Log> getLogAllEntries ()
		{
			return dao.getLogAllEntries ();
		}

		/// <summary>
		/// Returns all log entries under the given type.
		/// </summary>
		/// <returns>The log entries by type.</returns>
		/// <param name="logType">Log type.</param>
		public IList<Log> getLogEntriesByType (Log.LOG_TYPE logType)
		{
			return dao.getLogEntriesByType (logType);
		}

		/// <summary>
		/// Returns all log entries with the given severity.
		/// </summary>
		/// <returns>The log entries by severity.</returns>
		/// <param name="severity">Severity.</param>
		public IList<Log> getLogEntriesBySeverity (Log.SEVERITY severity)
		{
			return dao.getLogEntriesBySeverity (severity);
		}

		/// <summary>
		/// Returns all log entries for the given component
		/// </summary>
		/// <returns>The log entries by component.</returns>
		/// <param name="component">Component.</param>
		public IList<Log> getLogEntriesByComponent (string component)
		{
			return dao.getLogEntriesByComponent (component);
		}

		/// <summary>
		/// Returns the log entries for the given day
		/// </summary>
		/// <returns>The log entries by day.</returns>
		/// <param name="day">Day.</param>
		public IList<Log> getLogEntriesByDay (DateTime day)
		{
			return dao.getLogEntriesByDay (day);
		}

		/// <summary>
		/// Returns all the log entries under the month of the given date.
		/// </summary>
		/// <returns>The log entries by month.</returns>
		/// <param name="month">Month.</param>
		public IList<Log> getLogEntriesByMonth (DateTime month)
		{
			return dao.getLogEntriesByMonth (month);
		}

		/// <summary>
		/// Returns today's log entries.
		/// </summary>
		/// <returns>The log entries from today.</returns>
		public IList<Log> getLogEntriesFromToday ()
		{
			return dao.getLogEntriesFromToday ();
		}
		//  ------ Utility ------
		private void InitializeCountryList ()
		{
			CountryList = new List<Country> ();
			string[] countryArray = File.ReadAllLines ("Data/CountryCodes.csv");
			foreach (var c in countryArray) {
				var countryDef = c.Split (',');
				CountryList.Add (new Country (countryDef [1], countryDef [0]));
			}
		}

		public IList<Country> getCountryList ()
		{
			if (CountryList == null || CountryList.Count <= 0)
				InitializeCountryList ();
			return CountryList;
		}

		private void InitializeProductCategoryList ()
		{
			ProductCategoryList = new List<ProductCategory> ();
			IList<string> categories = File.ReadAllLines ("Data/ProductCategories.txt");
			foreach (var c in categories) {
				ProductCategory category = new ProductCategory ();
				category.Name = c;
				ProductCategoryList.Add (category);
			}
		}

		public IList<ProductCategory> getProductCategoryList ()
		{
			if (ProductCategoryList == null || ProductCategoryList.Count <= 0)
				InitializeProductCategoryList ();
			return ProductCategoryList;
		}

		private void InitializeUOMList ()
		{
			UOMList = new List<UOM> ();
			IList<string> uomList = File.ReadAllLines ("Data/UOM.txt");
			foreach (var u in uomList) {
				UOM uom = new UOM ();
				uom.Name = u;
				UOMList.Add (uom);
			}
		}

		public IList<UOM> getUOMList ()
		{
			if (UOMList == null || UOMList.Count <= 0)
				InitializeUOMList ();
			return UOMList;
		}
	}
}