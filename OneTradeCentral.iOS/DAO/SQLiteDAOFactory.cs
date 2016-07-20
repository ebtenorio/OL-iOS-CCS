using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Foundation;
using UIKit;

using SQLite;

using OneTradeCentral.DTOs;
using OneTradeCentral.iOS;

namespace OneTradeCentral.DAO
{
	public class SQLiteDAOFactory : IDAOFactory
	{
		static object _lock = new object ();
		private readonly static SQLiteConnection db = 
			new SQLiteConnection (Path.Combine (getUserDirectory(), "OneTradeCentral.DB"));

		private static String getUserDirectory() {
			
			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				return NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User) [0].Path;
			} else {
				return Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			}
		}
			
//		Original
//		private readonly static string ProductGroupLookupSQL = 
//			"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
//			+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
//			+ "and p.ProviderID = ? and p.StartDate <=? and p.EndDate >= ? ";


//		private readonly static string ProductGroupLookupSQL = 
//			"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
//			+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
//			+ "and p.ProviderID = ? and p.EndDate >= ? ";
		
		private readonly static string ProductGroupLookupSQL = 
			"select distinct g.* from [ProductGroup] g";

		private readonly static string GroupedProductLookupSQLRegular = 
			"select distinct p.* from [Product] p join [ProductGroupLookup] l on p.ID = l.ProductID " 
			+ "join [ProductGroup] g on g.ID = l.ProductGroupID where p.Inactive = 0 and p.Deleted = 0 "
			+ "and p.ProviderID = ? and g.Name = ? and p.StartDate <=? and p.EndDate >= ? ";

		private readonly static string GroupedProductLookupSQLPresell = 
			"select distinct p.* from [Product] p join [ProductGroupLookup] l on p.ID = l.ProductID " 
			+ "join [ProductGroup] g on g.ID = l.ProductGroupID where p.Inactive = 0 and p.Deleted = 0 "
			+ "and p.ProviderID = ? and g.Name = ? and p.StartDate > ? and p.EndDate >= ? ";

		private readonly static string GroupedProductLookupSQLAll = 
			"select distinct p.* from [Product] p join [ProductGroupLookup] l on p.ID = l.ProductID " 
			+ "join [ProductGroup] g on g.ID = l.ProductGroupID where p.Inactive = 0 and p.Deleted = 0 "
			+ "and p.ProviderID = ? and g.Name = ? and p.EndDate >= ? ";

		public SQLiteDAOFactory ()
		{
			// Create Accounts Table - Test
			db.CreateTable<UserAccount> ();
			db.CreateTable<AppInfo> ();
			db.CreateTable<Customer> ();
			db.CreateTable<Product> ();
			db.CreateTable<ProductGroup> ();
			db.CreateTable<ProductGroupLookup> ();
			db.CreateTable<Order> ();
			db.CreateTable<OrderLine> ();
			db.CreateTable<Provider> ();
			db.CreateTable<ProviderWarehouse> ();
			db.CreateTable<Log> ();
		}

		// ------ User Account ------
		public UserAccount getUserAccount ()
		{
			try {
				// Used in Production below:
				//return db.Table<UserAccount>().First();   //.Where( g => g.Username == Identity.UserName).First();  //     .Where( t => t.Username == Identity.UserName).First();
	
				// As suggested by Xamarin (John Miller).
				lock(_lock){
					return db.Table<UserAccount>().Where( t => t.Username == Identity.UserName).First();
				}
			} catch{
				return null;
			}
		}

		public void saveUserAccount (UserAccount user)
		{
			try {

				// there should only be one user account at any point in time
				db.DropTable<UserAccount>();
				db.CreateTable<UserAccount>();
				db.Insert(user);
			} catch (Exception e) {
				Logger.log ("Error saving user account: " + e.Message);
			}
		}

		// ------ App Info ------
		public AppInfo getApplicationInfo ()
		{
			// this table should only contain one record
			try {
				return db.Table<AppInfo> ().First ();
			} catch (Exception e) {
				//Logger.log (e.Message);
				return new AppInfo ();
			}
		}

		public void updateApplicationInfo (AppInfo appInfo)
		{
			// update or insert - depends on the existence of the PK
			if (db.Table<AppInfo> ().Where (t => t.PK == appInfo.PK).Count () == 0)
				db.Insert (appInfo);
			else 
				db.Update (appInfo);
		}

		// ------  Customer ------ 

		public IList<Customer> getActiveUniqueCustomerList() {
			// appended: Comparison of today's date to that of the Customer's End Date - Eldon
			var customerList = db.Query<Customer> (
				"select distinct ID, Name, StateCode, StateName from Customer where Inactive = 0 and Deleted = 0 " + 
				"and StartDate <= ? and EndDate >= ? order by Name",
				DateTime.Now, DateTime.Now);
			if (customerList == null)
				customerList = new List<Customer> ();
			return customerList;
		}

		public IList<Customer> getActiveCustomerList ()
		{
			var customerList = db.Table<Customer> ().Where 
				(c => c.Deleted == false && c.InActive == false  && c.StartDate <= DateTime.Now && c.EndDate >= DateTime.Now).
				OrderBy (c => c.Name).ToList ();
			if (customerList == null)
				customerList = new List<Customer> ();
			return customerList;
		}

		public IList<Customer> getAllCustomerList ()
		{
			var customerList = db.Table<Customer> ().OrderBy (c => c.Name).ToList ();
			//			CustomerList customerList = db.Table<Customer> ().Where (
			//				t => (t.InActive == false && t.Deleted == false)).ToList () as CustomerList;
			if (customerList == null)
				customerList = new List<Customer> ();
			return customerList;
		}

		public Customer getCustomerByPK (int PK)
		{
			var query = db.Table<Customer> ().Where (t => t.PK == PK);
			if (query.Count () > 0)
				return query.First ();
			else 
				return null;
		}


		public Customer getCustomerByOldID (long OldID)
		{
			var query = db.Table<Customer> ().Where (t => t.ID == OldID);
			if (query.Count () > 0)
				return query.First ();
			else 
				return null;
		}

		public IList<Customer> getCustomerListByID (long ID)
		{
			var query = db.Table<Customer> ().Where (t => t.ID == ID);
			if (query.Count () > 0)
				return query.ToList ();
			else 
				return null;
		}

		public Customer getCustomerByProviderID (long providerID, long customerID)
		{
			var query = db.Table<Customer> ().Where (t => t.ProviderID == providerID && t.ID == customerID);
			if (query.Count () > 0)
				return query.First ();
			else 
				return null;
		}


		public List<Customer> getAllCustomerByProviderID (long providerID)
		{
			// Added Start and End Date
			var query = db.Table<Customer> ().Where (t => t.ProviderID == providerID && t.StartDate <= DateTime.Now && t.EndDate > DateTime.Now ).OrderBy (c => c.Name);
			if (query.Count () > 0)
				return query.ToList();
			else 
				return null;
		}

		public int saveCustomer (Customer customer)
		{
			if (getCustomerByPK (customer.PK) == null)
				db.Insert (customer);
			else
				db.Update (customer);
			return customer.PK;
		}

		public int updateCustomerContactDetails (Customer customer) {
			if (customer != null) {
				saveCustomer (customer); // save changes in this customer record, e.g. customer code
				var customerList = getCustomerListByID (customer.ID);
				if (customerList != null && customerList.Count > 0) { 
					// now save all contact information for similar customer records
					foreach (Customer c in customerList) {
						c.ContactFirstName = customer.ContactFirstName;
						c.ContactLastName = customer.ContactLastName;
						c.ContactEmail = customer.ContactEmail;
						c.ContactMobile = customer.ContactMobile;
						c.ContactNumber = customer.ContactNumber;
						c.ContactFax = customer.ContactFax;
						saveCustomer (c);
					}
				}
				return customerList.Count;
			} else {
				return -1;
			}
		}

		public void deleteCustomer (Customer customer)
		{
			customer.Deleted = true;
			db.Update (customer);
		}

		public void deleteCustomer (int PK)
		{
			var queryResult = db.Table<Customer> ().Where (t => t.PK == PK);
			if (queryResult.Count () > 0) {
				Customer customer = queryResult.First ();
				deleteCustomer (customer);
			}
		}

		public void purgeCustomer (Customer customer)
		{
			if (customer.PK > 0 && customer.Deleted == true)
				db.Delete (customer);
		}

		public void purgeCustomer (int PK)
		{
			throw new NotImplementedException ();
		}

		// ------ Distributor ------ 

		public IList<Distributor> getDistributorList ()
		{
			IList<Distributor> distributorList = db.Table<Distributor> ().Where (t => t.Deleted == false).ToList ();
			return distributorList;
		}

		public Distributor getDistributor (int PK)
		{
			throw new NotImplementedException ();
		}

		public int saveDistributor (Distributor distributor)
		{
			throw new NotImplementedException ();
		}

		public void deleteDistributor (Distributor distributor)
		{
			throw new NotImplementedException ();
		}

		public void deleteDistributor (int PK)
		{
			throw new NotImplementedException ();
		}

		public void purgeDistributor (Distributor distributor)
		{
			throw new NotImplementedException ();
		}

		public void purgeDistributor (int PK)
		{
			throw new NotImplementedException ();
		}

		// ------ Order ------ 

		public IList<Order> getOrderList (int salesRepAccountID)
		{


			// Commented-out codes below are the original codes
//			var orderQuery = db.Table<Order> ().Where (t => t.Deleted == false).OrderByDescending(t => t.OrderDate);
//			IList<Order> orderList = orderQuery.ToList ();
//			if (orderList == null)
//				orderList = new List<Order> ();
//			return orderList;
			// Test query
			//var orderTestQ = db.Table<Order> ().Where (t => t.Deleted == false && t.SalesRepAccountID == salesRepAccountID).OrderByDescending(t => t.OrderDate)


			// Codes below had been changed to accept a parameter SalesRepAccountID
			//var orderQuery = db.Table<Order> ().Where (t => t.Deleted == false && t.SalesRepAccountID == salesRepAccountID).OrderByDescending(t => t.OrderDate);

			// Code below is to get Orders, disregarding the SalesRepAccountID


			// salesRepAccountID will be of no value anymore.


			// THIS CODE HAS BEEN MODIFIED:
			var orderQuery = db.Table<Order> ().Where (t => t.Deleted ==false).OrderByDescending (t => t.OrderDate); // Modified for WRI/PEP/FRU
			IList<Order> orderList = orderQuery.ToList ();
			if (orderList == null)
				orderList = new List<Order> ();
			return orderList;
		}

		public int CountOrdersByStatus (Order.STATUS status, int salesRepAccountID)
		{
			var query = db.Table<Order> ().Where (t => t.UploadStatus == (int)status && t.Deleted == false); //&& t.SalesRepAccountID == salesRepAccountID);
			if (query != null)
				return query.Count ();
			else
				return 0;
		}

		public IList<Order> getAllPendingOrders (int salesRepAccountID)
		{
			var query = db.Table<Order> ().Where (t => t.UploadStatus != (int)Order.STATUS.Completed && t.SalesRepAccountID == salesRepAccountID).OrderByDescending(t => t.DateCreated);
			IList<Order> orderList;
			if (query != null && (orderList = query.ToList ()) != null ) {
				foreach(Order o in orderList) {

					// compatibility: check for empty or blank GUIDs
					if (o.GUID == null || o.GUID.Trim ().Length == 0) {
						o.GUID = Guid.NewGuid ().ToString();
					}

					populateOrder (o);
				}
				return orderList;
			}
			else
				return new List<Order> ();
		}

		public IList<Order> getOrderListByStoreName(string storeName) {
			//			var query = db.Table<Order> ().Where (t => t.CustomerName.ToUpper ().Contains (storeName)).OrderBy (t => t.CustomerName).OrderByDescending( t => t.OrderDate);
			//			var orderList = query.ToList ();
			storeName = "%" + storeName.ToUpper() + "%";
			//			var orderList = db.Query<Order> ("select o.* from [Order] as o join [Customer] as c on o.CustomerPK = c.PK where c.Name like ? order by c.Name, o.OrderDate desc", storeName);
			var orderList = db.Query<Order> ("select o.* from [Order] as o where o.CustomerName like ? order by o.CustomerName, o.OrderDate desc", storeName);
			if (orderList != null) {
				return orderList;
			}
			else
				return new List<Order>();
		}

		public IList<Order> getOrderListByStoreID(string storeID) {
			storeID = "%" + storeID.ToUpper() + "%";
			//			var orderList = db.Query<Order> ("select o.* from [Order] as o join [Customer] as c on o.CustomerPK = c.PK where c.Code like ? order by c.Name, o.OrderDate desc", storeID);
			var orderList = db.Query<Order> ("select o.* from [Order] as o where o.StoreID like ? order by o.StoreID, o.OrderDate desc", storeID);
			if (orderList != null) {
				return orderList;
			}
			else
				return new List<Order>();
		}

		public IList<Order> getOrderListByOrderNumber(string orderNumber) {
			orderNumber = "%" + orderNumber.ToUpper() + "%";
			var orderList = db.Query<Order> ("select * from [Order] where OrderNumber like ? order by OrderNumber desc", orderNumber);
			if (orderList != null) {
				return orderList;
			}
			else
				return new List<Order>();
		}

		public IList<Order> getOrderListByOrderDate(DateTime orderDate) {
			return null;
		}

		public void populateOrder (Order order)
		{
			// assign customer
			//			var customerRecords = db.Table<Customer> ().Where (t => t.PK == order.CustomerPK);


			var customerRecords = db.Table<Customer> ().Where (t => t.ID == order.CustomerID && t.ProviderID == order.ProviderID);

			Customer c;
			if (customerRecords.Count () > 0) {
				// c = db.Table<Customer> ().Where (t => t.PK == order.CustomerPK).First ();
				c = customerRecords.First ();
//				c = db.Table<Customer> ().Where (t => t.ID == order.CustomerID && t.ProviderID == order.ProviderID).First (); 
			} else {
				c = new Customer ();
			}
			order.Customer = c;

			//			getOrderLineListByOrderPK (order);
			order.OrderLineList = getOrderLineListByOrderPK (order.PK);

		}

		public void populateOrderWithOldIDs(Order order){
			// ORIGINAL CODE FROM ORDERAPP
			// assign customer

//			var customerRecords = db.Table<Customer> ().Where (t => t.PK == order.CustomerPK);
			var customerRecords = db.Table<Customer> ().Where (t => t.OldCustomerID == order.CustomerID);

			Customer c;
			if (customerRecords.Count () > 0)
				c = db.Table<Customer> ().Where (t => t.PK == order.CustomerPK).First ();
			else
				c = new Customer ();
			order.Customer = c;

			// assign associated orderlines
			IList<OrderLine> orderLineList = db.Table<OrderLine> ().Where (t => t.OrderPK == order.PK).ToList ();
			if (orderLineList != null) {
				foreach (var ol in orderLineList) {
					if (ol.ProductPK > 0) {
						// associate product for each orderline
						Product product = new Product ();
						try{
							product = db.Table<Product> ().Where (p => p.PK == ol.ProductPK  && p.OldProductID > 0).First ();
						}
						catch(Exception e){
							Logger.log (e.Message);
							product.Name = "<Product info not found>";
						}
						ol.Product = product;
					}
				}
				order.OrderLineList = orderLineList;
			} else {
				order.OrderLineList = new List<OrderLine> ();
			}
				
		}

		public IList<OrderLine> getOrderLineListByOrderPK (int orderPK)
		{
			// assign associated orderlines
			IList<OrderLine> orderLineList = db.Table<OrderLine> ().Where (t => t.OrderPK == orderPK).ToList ();

			if (orderLineList == null) {
				orderLineList = new List<OrderLine> ();
			} else {
				foreach (var ol in orderLineList) {
					try {

						// Here is the new change as well.
						Product product = new Product();
						long oldProductID = 0;

						if(this.hasProductOldID(ol.ProductID , out oldProductID)){
							product = db.Table<Product> ().Where (p => p.OldProductID == oldProductID).First ();
						}
						else{
							product = db.Table<Product> ().Where (p => p.ID == ol.ProductID).First ();
						}

						ol.Product = product;
					} catch (Exception e) {
						Logger.log ("Error retrieving product, ID = " + ol.ProductID + ".");
						Logger.log (e.Message);
					}
				}
			}

			return orderLineList;
		}

		public Order getOrder (int PK)
		{
			Order order = db.Table<Order> ().Where (t => t.PK == PK).First ();
			populateOrder (order);
			return order;
		}

		public void deleteOldOrders(int purgePeriod)
		{
//			var orderQuery = db.Table<Order> ().Where (t => t.Deleted == false).OrderByDescending(t => t.OrderDate);
			var orderQuery = db.Table<Order> ().OrderByDescending (t => t.OrderDate);
			IList<Order> orderList = orderQuery.ToList ();
			foreach (Order order in orderList) {
				if ((DateTime.Now - order.OrderDate).TotalDays > purgePeriod) {
					this.deleteOrder (order);
				}
			}
		}

		public int saveOrder (Order order)
		{
			// db.InsertOrReplace appears to be buggy, so do a manual insert or update
			bool insertMode = (order.PK == 0);
			try {
				db.BeginTransaction ();
				if (insertMode) {
					setDefaultValues (order);
					db.Insert (order);
				} else {
					db.Update (order);
				}

				if (order.OrderLineList != null && order.OrderLineList.Count > 0) {
					foreach (var ol in order.OrderLineList) {
						ol.OrderPK = order.PK;
						ol.ProductPK = ol.Product.PK;
						ol.ProductID = ol.Product.ID;
						ol.ProductCode = ol.Product.Code;
						ol.ProductName = ol.Product.Name;
						ol.ProductGTINCode = ol.Product.GTINCode;
						ol.ProductSKU = ol.Product.SKU;
						ol.ProductUOM = ol.Product.UOM;
						ol.ProductProviderID = ol.Product.ProviderID;
						if (insertMode) {
							db.Insert (ol);
						} else {
							db.Update (ol);
						}
					}
				} 
				db.Commit ();
			} catch (Exception e) {
				Logger.log ("Error Saving Order: " + e.Message);
				db.Rollback ();
				throw (e);
			}
			return order.PK;
		}

		// invoke this for new orders only
		private void setDefaultValues(Order order) {
			DateTime todateUTC = DateTime.Now.ToUniversalTime();
			// set customer information
			order.CustomerPK = order.Customer.PK;
			order.CustomerName = order.Customer.Name;
			// See ticket #132, store id is entered by sales reps
			//			order.CustomerCode = order.Customer.Code;
			// set date fields
			order.OrderDate = todateUTC; 
			order.DateCreated = todateUTC;
			order.DateUpdated = todateUTC;
			order.DatePosted = todateUTC;
			// these values should be auto populated by the server
			var userAccount = getUserAccount ();
			order.SalesRepAccountID = Convert.ToInt32(userAccount.AccountID);
			order.SalesOrgID = userAccount.RefID;
			order.CreatedByUserID = userAccount.AccountID;
			order.UpdatedByUserID = userAccount.AccountID;
			order.IsHeld = true;
			order.IsSent = false;
			order.OrderStatusText = Constants.DefaultOrderStatusText;
			order.SYSOrderStatusText = Constants.DefaultOrderStatusText;
			order.SYSOrderStatusID = Constants.DefaultSysOrderStatusID;
			//			order.ProviderID = Constants.DefaultProviderID;
			// See Order.STATUS for the values used here
			// public enum STATUS { Pending=1, Processing=2, Partial=4, Complete=8 }
			if (order.UploadStatus <= 0)
				order.UploadStatus = (int)Order.STATUS.Pending;
		}

		public void deleteOrder (Order order)
		{
			if (order.PK > 0) {
				IList<OrderLine> orderLineList = db.Table<OrderLine> ().Where (t => t.PK == order.PK).ToList ();
				if (orderLineList != null) {
					foreach (var ol in orderLineList) {
						ol.Deleted = true;
						db.Update (ol);
					}
				}
				order.Deleted = true;
				db.Update (order);
			}
		}

		public void deleteOrder (int PK)
		{
			if (PK > 0) {
				var order = db.Table<Order> ().Where (t => t.PK == PK).First ();
				deleteOrder (order);
			}
		}

		// "purge" operations should be called by the synch process after sending and confirming with the WS 
		// that server-side deletes are successful 
		public void purgeOrder (Order order)
		{
			// purge only records that have been previously marked for deletion
			if (order.PK > 0 && order.Deleted == true) {
				IList<OrderLine> orderLineList = db.Table<OrderLine> ().Where (t => t.ProductPK == order.PK).ToList ();
				if (orderLineList != null) {
					foreach (var ol in orderLineList) {
						db.Delete (ol);
					}
				}
				db.Delete (order);
			}
		}

		public void purgeOrder (int PK)
		{
			throw new NotImplementedException ();
		}

		// ------ Product ------ 

		public IList<Product> getProductList ()
		{
			// Modified: Added EndDate Comparison - Eldon
			IList<Product> productList = db.Table<Product> ().Where (t => t.Deleted == false && t.Inactive == false && 
				t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now).OrderBy (t => t.Name).ToList ();
			if (productList == null)
				productList = new List<Product> ();
			return productList;
		}

		// Modified: Added EndDate Comparison - Eldon
		public IList<Product> getProductListByProductName (long providerID, string productName, bool? IsRegularOrder)
		{
			IList<Product> productList = null;
			if ((bool)IsRegularOrder == true) {
				
				productList = db.Table<Product> ().Where (t => t.Deleted == false && t.Inactive == false && 
					t.ProviderID == providerID && t.Name.Contains (productName)  && t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now).
					OrderBy (t => t.Name).ToList ();
				
			} else if ((bool)IsRegularOrder == false){
				productList = db.Table<Product> ().Where (t => t.Deleted == false && t.Inactive == false && 
					t.ProviderID == providerID && t.Name.Contains (productName)  && t.StartDate > DateTime.Now && t.EndDate >= DateTime.Now).
					OrderBy (t => t.Name).ToList ();
			}
				
				


		
			if (productList == null)
				productList = new List<Product> ();
			return productList;
		}

		public IList<Product> getProductListByOldID (long oldID)
		{
			IList<Product> productList = db.Table<Product> ().Where (t => t.Deleted == false && t.Inactive == false && 
				t.OldProductID == oldID && t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now).
				OrderBy (t => t.Name).ToList ();
			if (productList == null)
				productList = new List<Product> ();
			return productList;
		}

		public IList<Product> getProductListByProductCode (long providerID, string productCode)
		{
			// Modified: Added EndDate Comparison - Eldon
			IList<Product> productList = db.Table<Product> ().Where (t => t.Deleted == false && t.Inactive == false && 
				t.ProviderID == providerID && t.Code.Contains (productCode) && t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now).
				OrderBy (t => t.Name).ToList (); 
			if (productList == null)
				productList = new List<Product> ();
			return productList;
		}

		public Product getProduct (int PK)
		{
			// Modified: Added Endate Comparison - Eldon
			var query = db.Table<Product> ().Where (t => t.PK == PK  && t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now);
			if (query.Count () >= 1)
				return query.First ();
			else
				return null;
		}

		public Product getProductByOldID (long OldID)
		{
			// USe of the current ID to compare with the Old ID inthe local DB
			var query = db.Table<Product> ().Where (t => t.OldProductID == OldID  && t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now);
			if (query.Count () >= 1)
				return query.First ();
			else
				return null;
		}

		public IList<Product> getProductListByProductGroup (long providerID, string productGroup)
		{

			// Appended: EndDate Comparison - Eldon
			try {
				//				productGroup = "%" + productGroup + "%";
				return db.Query<Product>( 
					"select distinct p.* from [Product] p join [ProductGroupLookup] g on p.Code = g.ProductCode where p.Inactive = 0 and p.Deleted = 0 " + 
					"and p.ProviderID = ? and g.GroupName = ? and p.StartDate <= ? and p.EndDate >= ? order by p.Name", 
					providerID, productGroup, DateTime.Now, DateTime.Now);
			} catch (Exception e) {
				Logger.log ("Error retrieving products by product group: " + e.Message);
				return null;
			}
		}

		public IList<Product> getProductListByProductGroupAndProviderID (long providerID, string productGroup, bool? IsRegularOrder, DateTime DateCriteria)
		{

			// Modified: Added EndDate comparison - Eldon
			// Modify the following SQL statements below to cater for the Pre-Sell and Regular Orders.
			// Note : DateCriteria is RequestedReleaseDate for REGULAR, and DateCriteria is OrderDate for PRE-SELL

			if (IsRegularOrder != null) {
				
				if (IsRegularOrder == true) {
				
					// REGULAR ORDERS : ProductStartDate <= RequestedReleaseDate <= ProductEndDate
					try {

//						// ORIGINAL CODES:
//						var result = db.Query<Product> (
//						"select distinct p.* from [Product] p join [ProductGroupLookup] l on p.ID = l.ProductID " +
//						"join [ProductGroup] g on g.ID = l.ProductGroupID where p.Inactive = 0 and p.Deleted = 0 " +
//						"and g.Name = ? and p.ProviderID = ? and p.StartDate <= ? and p.EndDate >= ? order by p.Name", 
//							productGroup, providerID, DateCriteria.ToLocalTime().Date, DateCriteria.ToLocalTime().Date);


		
						var result=  db.Query<Product> (
							"select distinct p.* from [Product] p join [ProductGroupLookup] l on p.ID = l.ProductID " +
							"join [ProductGroup] g on g.ID = l.ProductGroupID where p.Inactive = 0 and p.Deleted = 0 " +
							"and g.Name = ? and p.ProviderID = ? and p.StartDate <= ? and p.EndDate >= ? order by p.Name", 
							productGroup, providerID, DateCriteria.ToLocalTime().Date, DateCriteria.ToLocalTime().Date); // TEST: Changed from DateCriteria, = was removed at the EndDate condition


						return result;
					


					} catch (Exception e) {
						Logger.log ("Error retrieving products by product group: " + e.Message);
						return null;
					}
						
				} else  {
					// PRE-SELL ORDERS : ProductStartDate > Order Date And ProductEndDate >= Today
					try {

						var result = db.Query<Product> (
							             "select distinct p.* from [Product] p join [ProductGroupLookup] l on p.ID = l.ProductID " +
							             "join [ProductGroup] g on g.ID = l.ProductGroupID where p.Inactive = 0 and p.Deleted = 0 " +
							             "and g.Name = ? and p.ProviderID = ? and p.StartDate > ? and p.EndDate >= ? order by p.Name", 
										 productGroup, providerID, DateTime.Now.ToLocalTime().Date, DateTime.Now.ToLocalTime().Date);

		
						return result;

					} catch (Exception e) {
						Logger.log ("Error retrieving products by product group: " + e.Message);
						return null;
					}

				}
			} else {

				var result = db.Query<Product> (
					"select distinct p.* from [Product] p join [ProductGroupLookup] l on p.ID = l.ProductID " +
					"join [ProductGroup] g on g.ID = l.ProductGroupID where p.Inactive = 0 and p.Deleted = 0 " +
					"and g.Name = ? and p.ProviderID = ? and p.EndDate >= ? order by p.Name", 
					productGroup, providerID, DateTime.Now.ToLocalTime().Date);

				return result;
			}

		}


		public IList<Product> getProductListByProvider (long providerID)
		{
			// New method to get the Products by providerID
			try {
				return db.Query<Product>( 
					"select distinct p.* from [Product] p join [ProductGroupLookup] l on p.ID = l.ProductID " + 
					"join [ProductGroup] g on g.ID = l.ProductGroupID where p.Inactive = 0 and p.Deleted = 0 " + 
					"and p.ProviderID = ? and p.StartDate <= ? and p.EndDate >= ? order by p.Name", 
					 providerID, DateTime.Now, DateTime.Now);
			} catch (Exception e) {
				Logger.log ("Error retrieving products by product group: " + e.Message);
				return null;
			}
		}


		public int saveProduct (Product product)
		{
			if (getProduct (product.PK) == null)
				db.Insert (product);
			else
				db.Update (product);
			return product.PK;
		}

		public void deleteProduct (Product product)
		{
			product.Deleted = true;
			db.Update (product);
		}

		public void deleteProduct (int PK)
		{
			var query = db.Table<Product> ().Where (t => t.PK == PK);
			if (query.Count () > 0) {
				Product product = query.First ();
				deleteProduct (product);
			}
		}

		public void purgeProduct (Product product)
		{
			if (product.Deleted == true)
				db.Delete (product);
		}

		public void purgeProduct (int PK)
		{
			throw new NotImplementedException ();
		}

		// ------ Product Group ------

		public int saveProductGroup (ProductGroup productGroup)
		{
			if (productGroup.PK > 0)
				db.Update (productGroup);
			else
				db.Insert (productGroup);

			return productGroup.PK;
		}

		public int saveProductGroupLookup (ProductGroupLookup productGroupLookup)
		{
			if (productGroupLookup.PK > 0)
				db.Update (productGroupLookup);
			else
				db.Insert (productGroupLookup);

			return productGroupLookup.PK;
//			try {
//				return db.Insert(productGroupLookup);
//			} catch (Exception e) {
//				Logger.log ("Error saving product group: " + e.Message);
//				return 0;
//			}
		}

		public IList<ProductGroup> getAllProductGroups ()
		{
			try {
				return db.Table<ProductGroup>().Distinct().ToList();
//				return db.Query<ProductGroup>("select distinct GroupName as Name from [ProductGroupLookup]");
			} catch (Exception e) {
				Logger.log ("Error retrieving product groups: " + e.Message);
				return null;
			}
		}

		public int CountProductGroups() {
			var query = db.Query<ProductGroup>(
				"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
				+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
				+ "and p.StartDate <= ? and p.EndDate >= ? ", DateTime.Now.ToLocalTime().Date, DateTime.Now.ToLocalTime().Date);
			return query.Count;
		}

		public int CountFutureProductGroups() {
			var query = db.Query<ProductGroup>(
				"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
				+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
				+ "and p.StartDate > ? and p.EndDate >= ? ", DateTime.Now.ToLocalTime().Date, DateTime.Now.ToLocalTime().Date);
			return query.Count;
		}

		public IList<ProductGroup> getProductGroupsByProviderID (long providerID)
		{
			try {

//				// REVERT TO THIS
//				return db.Query<ProductGroup> (
//				ProductGroupLookupSQL, providerID, DateTime.Now, DateTime.Now
//				).OrderBy(t => t.Name).ToList();
//
//				return db.Query<ProductGroup> (
//					ProductGroupLookupSQL
//				).OrderBy(t => t.Name).ToList();


				// REVERT TO THIS
				return db.Query<ProductGroup> (ProductGroupLookupSQL, providerID, DateTime.Now
				).OrderBy(t => t.Name).ToList();


			} catch (Exception e) {
				Logger.log ("Error retrieving product groups: " + e.Message);
				return null;
			}
		}

		public IList<ProductGroup> getProductGroupsByName (long providerID, string groupName, bool? IsRegularOrder)
		{
			try {
				groupName = "%" + groupName + "%";

				string RegularOrderGroups = "select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
					+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
					+ "and p.ProviderID = ? and p.StartDate <=? and p.EndDate >= ? ";

				string PresellOrderGroups = "select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
					+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
					+ "and p.ProviderID = ? and p.StartDate > ? and p.EndDate >= ? ";

				string AllOrderGroups = "select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
					+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
					+ "and p.ProviderID = ? and p.EndDate >= ? ";

				if(IsRegularOrder != null){
					if(IsRegularOrder == true){
						return db.Query<ProductGroup>(
							RegularOrderGroups + " and g.Name like ? ", 
							providerID, DateTime.Now.ToLocalTime().Date, DateTime.Now.ToLocalTime().Date, groupName);
					}
					else {
						return db.Query<ProductGroup>(
							PresellOrderGroups + " and g.Name like ? ", 
							providerID, DateTime.Now.ToLocalTime().Date, DateTime.Now.ToLocalTime().Date, groupName);
					}
				}
				else{
					// Default at Regular and Presell
					return db.Query<ProductGroup>(
						AllOrderGroups + " and g.Name like ? ", 
						providerID, DateTime.Now.ToLocalTime().Date, groupName);
				}


			} catch (Exception e) {
				Logger.log ("Error retrieving product groups by GroupName: " + e.Message);
				return null;
			}
		}

		public IList<ProductGroup> getProductGroupsByName (string groupName)
		{
			try {
				groupName = "%" + groupName + "%";
				return db.Query<ProductGroup>("select distinct GroupName as Name from [ProductGroupLookup] where GroupName like ?", groupName);
			} catch (Exception e) {
				Logger.log ("Error retrieving product groups by GroupName: " + e.Message);
				return null;
			}
		}

		public IList<ProductGroup> getProductGroupsByProductCode (long providerID, string productCode, bool? IsRegularOrder)
		{
			try {

				string lookUp;

				lookUp =	"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
					+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
					+ "and p.ProviderID = ? and p.StartDate <=? and p.EndDate >= ? ";
				
				if(IsRegularOrder != null){
					if((bool)IsRegularOrder == true){
						lookUp =	"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
							+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
							+ "and p.ProviderID = ? and p.StartDate <=? and p.EndDate >= ? ";
						
					}
					else if((bool)IsRegularOrder == false){
						lookUp =	"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
							+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
							+ "and p.ProviderID = ? and p.StartDate > ? and p.EndDate > ? ";
						
					}
				}
			


				productCode = "%" + productCode + "%";
				var list = db.Query<ProductGroup> (
					lookUp + " and p.Code like ? ", 
					providerID, DateTime.Now.ToLocalTime().Date, DateTime.Now.ToLocalTime().Date, productCode).ToList();
				return list;
			} catch (Exception e) {
				Logger.log ("Error retrieving product groups by ProductCode: " + e.Message);
				return null;
			}
		}

		public IList<ProductGroup> getProductGroupsByProductCode (string productCode)
		{
			try {
				productCode = "%" + productCode + "%";
				return db.Query<ProductGroup>(
					"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
					+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 "
					+ "and p.StartDate <= ? and p.EndDate >= ? and p.Code like ?", 
					DateTime.Now.ToLocalTime().Date, DateTime.Now.ToLocalTime().Date, productCode);
			} catch (Exception e) {
				Logger.log ("Error retrieving product groups by ProductCode: " + e.Message);
				return null;
			}
		}

		public IList<ProductGroup> getProductGroupsByProductName (long providerID, string productName, bool? IsRegularOrder)
		{
			try {

				string ProductGroupLookupSQLRegular 
					= 	"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
					+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
					+ "and p.ProviderID = ? and p.StartDate <=? and p.EndDate >= ? ";


				string ProductGroupLookupSQLPresell
				= 	"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
					+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
					+ "and p.ProviderID = ? and p.StartDate > ? and p.EndDate >= ? ";

				string ProductGroupLookupSQLAll
				= 	"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
					+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
					+ "and p.ProviderID = ? and p.EndDate >= ? ";
				
				productName = "%" + productName + "%";

			
				if(IsRegularOrder != null){
					if((bool) IsRegularOrder == true){
						var result =  db.Query<ProductGroup>(ProductGroupLookupSQLRegular + " and p.Name like ? ", providerID, DateTime.Now.ToLocalTime().Date.ToLocalTime().Date, DateTime.Now.ToLocalTime().Date.ToLocalTime().Date, productName);
						return result;
					}
					else if (IsRegularOrder == false){
						var result =  db.Query<ProductGroup>(ProductGroupLookupSQLPresell + " and p.Name like ? ", providerID, DateTime.Now.ToLocalTime().Date, DateTime.Now.ToLocalTime().Date, productName);
						return result;
					}
					else{
						var result =  db.Query<ProductGroup>(ProductGroupLookupSQLAll + " and p.Name like ? ", providerID, DateTime.Now, DateTime.Now,productName);
						return result;
					}
				}
				else{
					var result =  db.Query<ProductGroup>(ProductGroupLookupSQLAll + " and p.Name like ? ", providerID, DateTime.Now,  productName);
					return result;

				}



			} catch (Exception e) {
				Logger.log ("Error retrieving product groups by ProductName: " + e.Message);
				return null;
			}
		}

		public IList<ProductGroup> getProductGroupsByProductName (string productName)
		{
			try {
				productName = "%" + productName + "%";
				return db.Query<ProductGroup>(
					"select distinct g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID " 
					+ "join [Product] p on l.ProductID = p.ID where p.Inactive = 0 and p.Deleted = 0 " 
					+ "and p.StartDate <= ? and p.EndDate >= ? and p.Name like ?", 
					DateTime.Now, DateTime.Now, productName);
//					"select distinct GroupName as Name from [ProductGroupLookup] where ProductName like ?", productName);
			} catch (Exception e) {
				Logger.log ("Error retrieving product groups by ProductName: " + e.Message);
				return null;
			}
		}

		public IList<ProductGroup> getProductGroupsByProduct (Product product)
		{
			try {
				return db.Query<ProductGroup>( 
					"select g.* from [ProductGroup] g join [ProductGroupLookup] l on g.ID = l.ProductGroupID "
					+ "join [Product] p on p.ID = l.ProductID where p.Inactive = 0 and p.Deleted = 0 "
					+ "and p.StartDate <= ? and p.EndDate >= ? and p.ID = ? ", 
					DateTime.Now, DateTime.Now, product.ID);
			} catch (Exception e) {
				Logger.log ("Error retrieving products groups by product: " + e.Message);
				return null;
			}
		}

		// Added new Parameter: DateTime ReleaseDate
		public IList<Product> getGroupedProductsByProductName (long providerID, string groupName, string productName, bool? IsRegularOrder, DateTime ReleaseDate)
		{
			// Modified: Added EndDate comparison - Eldon
			try {


				if(IsRegularOrder != null){

					if(IsRegularOrder == true){
						productName = "%" + productName + "%";
						var result =  db.Query<Product>(GroupedProductLookupSQLRegular + " and p.Name like ? ",
							providerID, groupName, DateTime.Now, ReleaseDate, productName); // use ReleaseDate here 
						return result;
					}
					else{
						productName = "%" + productName + "%";
						var result =  db.Query<Product>(GroupedProductLookupSQLPresell + " and p.Name like ? ",
							providerID, groupName, DateTime.Now, DateTime.Now, productName);
						return result;
					}


				}
				else{
					productName = "%" + productName + "%";
					var result =  db.Query<Product>(GroupedProductLookupSQLAll + " and p.Name like ? ",
						providerID, groupName, DateTime.Now, productName);
					return result;
				}


			} catch (Exception e) {
				Logger.log ("Error retrieving products by product group: " + e.Message);
				return null;
			}
		}

		public IList<Product> getGroupedProductsByProductCode (long providerID, string groupName, string productCode, bool? IsRegularOrder)
		{
			try {
				productCode = "%" + productCode + "%";
				if(IsRegularOrder != null){
				if((bool)IsRegularOrder){
						var result =  db.Query<Product>(GroupedProductLookupSQLRegular + " and p.Code like ? ",
						providerID, groupName, DateTime.Now, DateTime.Now, productCode);
						return result;
				}
				else{
						var result =  db.Query<Product>(GroupedProductLookupSQLPresell + " and p.Code like ? ",
							providerID, groupName, DateTime.Now, DateTime.Now, productCode);
						return result;
				}
				}
				else{
					var result = db.Query<Product>(GroupedProductLookupSQLAll + " and p.Code like ? ",
						providerID, groupName, DateTime.Now, productCode);
					return result;
				}

			} catch (Exception e) {
				Logger.log ("Error retrieving products by product group: " + e.Message);
				return null;
			}
		}

		// ------ Payment Terms ------ 

		public IList<PaymentTerms> getPaymentTermsList ()
		{
			var tableQuery = db.Table<PaymentTerms> ();
			if (tableQuery.Count () > 0)
				return tableQuery.ToList ();
			else
				return new List<PaymentTerms> ();
		}

		public PaymentTerms getPaymentTerms (int PK)
		{
			var query = db.Table<PaymentTerms> ().Where (t => t.PK == PK);
			if (query.Count () >= 1)
				return query.First ();
			else
				return null;
		}

		public int savePaymentTerms (PaymentTerms terms)
		{
			if (getPaymentTerms (terms.PK) == null) {
				db.Insert (terms);
			} else {
				db.Update (terms);
			}
			return terms.PK;
		}

		public void deletePaymentTerms (PaymentTerms terms)
		{
			terms.Deleted = true;
			savePaymentTerms (terms);
		}

		public void deletePaymentTerms (int PK)
		{
			PaymentTerms terms = getPaymentTerms (PK);
			deletePaymentTerms (terms);
		}

		public void purgePaymentTerms (PaymentTerms terms)
		{
			db.Delete (terms);
		}

		public void purgePaymentTerms (int PK)
		{
			PaymentTerms terms = getPaymentTerms (PK);
			purgePaymentTerms (terms);
		}

		// ------ Provider ------

		public void saveProvider (Provider provider)
		{
			if (provider.PK <= 0)
				db.Insert (provider);
			else
				db.Update (provider);
		}

		public Provider getProviderByID (long providerID)
		{
			var query = db.Table<Provider> ().Where (t => t.ID == providerID);
			if (query != null && query.Count() > 0)
				return query.First ();
			else
				return new Provider();
		}

		public Provider getProviderByOldID (long oldID)
		{
			var query = db.Table<Provider> ().Where (t => t.OldProviderID == oldID);
			if (query != null && query.Count() > 0)
				return query.First ();
			else
				return new Provider();
		}

		public IList<Provider> getProviderList ()
		{
			var query = db.Table<Provider> ().Where (t => t.Deleted == false && t.InActive == false);
			if (query != null && query.Count() > 0)
				return query.ToList ();
			else
				return new List<Provider> ();
		}

		public IList<Provider> getDistinctProviderList ()
		{
			var query = db.Query<Provider> (
				            "select distinct p.* from [Provider] p where p.Inactive = 0 and p.Deleted = 0 ");

			if (query != null && query.Count() > 0)
				return query.ToList ();
			else
				return new List<Provider> ();
		}

		public IList<Provider> getProviderListByCustomerID (long customerID)
		{
			var query = db.Query<Provider> (
				"select distinct p.* from [Provider] p join [Customer] c on p.ID = c.ProviderID where p.Inactive = 0 and p.Deleted = 0 "
				+ "and c.ID = ? and c.StartDate <= ? and c.EndDate >= ?", customerID, DateTime.Now, DateTime.Now);
			if (query != null && query.Count() > 0)
				return query.ToList ();
			else
				return new List<Provider> ();
		}

		public IList<Provider> getProviderListByCustomerIDFiltered (long customerID)
		{

			IList<Provider> _providerListFiltered = new List<Provider>();

			IList<Provider> _providerList = new List<Provider>();

			var queryProvider = db.Query<Provider> (
				                    "select distinct p.* from [Provider] p join [Customer] c on p.ID = c.ProviderID where p.Inactive = 0 and p.Deleted = 0 "
				                    + "and c.ID = ? and c.StartDate <= ? and c.EndDate >= ?", customerID, DateTime.Now, DateTime.Now);

			_providerList = queryProvider.ToList ();


			var queryProviderWarehouse = db.Query<ProviderWarehouse> (
				                             "select distinct pw.* from [ProviderWarehouse] pw where pw.Inactive = 0 and pw.Deleted = 0 "
				                             + "and pw.StartDate <= ? and pw.EndDate >= ?", DateTime.Now, DateTime.Now);

			var queryProviderProduct = db.Query<Product> (
				                           "select distinct p.* from [Product] p where p.Inactive = 0 and p.Deleted = 0 "
				                           + "and p.StartDate <= ? and p.EndDate >= ?", DateTime.Now, DateTime.Now);


			foreach (Provider _provider in _providerList) {

				int countWareHouse = 0;
				int countProduct = 0;

				foreach (Product _product in queryProviderProduct.ToList()) {
					if (_provider.ID == _product.ProviderID)
						countProduct++;
				}

				foreach (ProviderWarehouse _providerWarehouse in queryProviderWarehouse.ToList()) {
					if ( _provider.ID == _providerWarehouse.ProviderID)
						countWareHouse++;
				}


				if (countProduct > 0 && countWareHouse > 0)
					_providerListFiltered.Add (_provider);
					
			}


			return _providerListFiltered;

		
//			if (query != null && query.Count() > 0)
//				return query.ToList ();
//			else
//				return new List<Provider> ();

		}


		public Provider getProviderByCustomerAndProviderID (long customerID, long providerID)
		{
			var query = db.Query<Provider> (
				"select p.* from [Provider] p join [Customer] c on p.ID = c.ProviderID where p.Inactive = 0 and p.Deleted = 0 and "
				+ "c.ID = ? and p.ID = ? and c.StartDate <= ? and c.EndDate >= ?", 
				customerID, providerID, DateTime.Now, DateTime.Now);
			if (query != null && query.Count () > 0)
				return query.FirstOrDefault ();
			else
				return new Provider ();
		}

		// ------ Provider Warehouse ------

		public void saveProviderWarehouse(ProviderWarehouse warehouse) {
			if (warehouse.PK <= 0)
				db.Insert (warehouse);
			else
				db.Update (warehouse);
		}

		public ProviderWarehouse getProviderWarehouseByID(int warehouseID) {
			var query = db.Table<ProviderWarehouse> ().Where (t => t.ID == warehouseID);
			try {
				if (query != null && query.Count() > 0) 
					return query.First ();
				else 
					return new ProviderWarehouse ();
			} catch (Exception e) {
				Logger.log ("Error retrieving warehouse, ID = " + warehouseID + ". ");
				Logger.log ("Warehouse ID: " + warehouseID + ", " + e.Message);
				return new ProviderWarehouse ();
			}
		}

		public ProviderWarehouse getProviderWarehouseByOldID(long oldID) {
			var query = db.Table<ProviderWarehouse> ().Where (t => t.OldProviderWarehouseID == oldID);
			try {
				if (query != null && query.Count() > 0) 
					return query.First ();
				else 
					return new ProviderWarehouse ();
			} catch (Exception e) {
				Logger.log ("Error retrieving warehouse, Old ID = " + oldID + ". ");
				Logger.log ("Warehouse Old ID: " + oldID + ", " + e.Message);
				return new ProviderWarehouse ();
			}
		}

		public IList<ProviderWarehouse> getProviderWarehouseList() {
			var query = db.Table<ProviderWarehouse>().Where ( t => t.Deleted == false && t.InActive == false && 
				t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now);
			if (query != null && query.Count() > 0)
				return query.ToList();
			else
				return new List<ProviderWarehouse>();
		}

		public IList<ProviderWarehouse> getProviderWarehouseListByProviderID (long providerID)
		{
			var query = db.Table<ProviderWarehouse>().Where ( t => t.Deleted == false && t.InActive == false &&
				t.ProviderID == providerID && t.StartDate <= DateTime.Now && t.EndDate >= DateTime.Now);
			if (query != null && query.Count() > 0)
				return query.ToList();
			else
				return new List<ProviderWarehouse>();
		}

		// ------ Log Entries ------

		public void saveLog (Log log)
		{
			try {
				db.Insert (log);
			} catch (Exception e) {
				Console.WriteLine("Error saving log: " + e.Message);
			}
		}

		public IList<Log> getLogAllEntries ()
		{
			var logEntries = db.Table<Log> ().OrderByDescending(t=> t.logTimeStamp);
			if (logEntries != null && logEntries.Count() > 0)
				return logEntries.ToList();
			else
				return new List<Log> ();
		}

		public IList<Log> getLogEntriesByType (Log.LOG_TYPE logType)
		{
			var logEntries = db.Table<Log> ().Where( t => t.logType == logType);
			if (logEntries != null && logEntries.Count() > 0)
				return logEntries.ToList();
			else
				return new List<Log> ();
		}

		public IList<Log> getLogEntriesBySeverity (Log.SEVERITY severity)
		{
			var logEntries = db.Table<Log> ().Where( t => t.severity == severity);
			if (logEntries != null)
				return logEntries.ToList();
			else
				return new List<Log> ();
		}

		public IList<Log> getLogEntriesByComponent (string component)
		{
			var logEntries = db.Table<Log> ().Where( t => t.component == component);
			if (logEntries != null)
				return logEntries.ToList();
			else
				return new List<Log> ();
		}

		public IList<Log> getLogEntriesByDay (DateTime date)
		{
			var logEntries = db.Table<Log> ().Where( t => t.logTimeStamp.Month == date.Month 
				&& t.logTimeStamp.Year == date.Year
				&& t.logTimeStamp.Day == date.Day);
			if (logEntries != null)
				return logEntries.ToList();
			else
				return new List<Log> ();
		}

		public IList<Log> getLogEntriesByMonth (DateTime date)
		{
			var logEntries = db.Table<Log> ().Where( t => t.logTimeStamp.Month == date.Month 
				&& t.logTimeStamp.Year == date.Year);
			if (logEntries != null)
				return logEntries.ToList();
			else
				return new List<Log> ();
		}

		public IList<Log> getLogEntriesFromToday ()
		{
			var todate = DateTime.Now;
			return getLogEntriesByDay (todate);
		}

		// ------ Record Counter ------


		public int CountAllCustomers ()
		{
			return db.Table<Customer> ().Count ();
		}

		public int CountUniqueCustomers ()
		{
			var query = db.Query<Customer> ("select distinct ID from Customer where Inactive = 0 and Deleted = 0 and StartDate <= ? and EndDate >= ?", DateTime.Now, DateTime.Now);
			return query.Count();
		}

		public int CountDistributors ()
		{
			return db.Table<Distributor> ().Count ();
		}

		public int CountOrders (int salesRepAccountID)
		{
			//return db.Table<Order> ().Count ();
			return db.Table<Order> ().Where(t => t.Deleted == false).ToList ().Count;    //.Where (t => t.SalesRepAccountID); //== salesRepAccountID).Count();
		}

		public int CountOrderLines (int salesRepAccountID)
		{
			IList<Order> _orders = db.Table<Order> ().Where(t => t.Deleted ==false).ToList ();  //.Where (t => t.SalesRepAccountID == salesRepAccountID).ToList ();
			IList<OrderLine> _orderlines = db.Table<OrderLine> ().ToList ();

			int countOrderLines = 0;

			for (int iOrder = 0; iOrder < _orders.Count; iOrder++) {
				for (int iOrderLine = 0; iOrderLine < _orderlines.Count; iOrderLine++) {
					if (_orderlines [iOrderLine].OrderPK == _orders [iOrder].PK) {
						countOrderLines++;
					}
				}
			}
				
			return countOrderLines;
			//return db.Table<OrderLine> ().Where(t=> t
		}

		public int CountPaymentTerms ()
		{
			return db.Table<PaymentTerms> ().Count ();
		}

		public int CountProductCategories ()
		{
			return db.Table<ProductCategory> ().Count ();
		}

		public int CountAllProducts ()
		{
			return db.Table<Product> ().Count ();
		}

		// CURRENT PRODUCTS WHERE StartDate <= TODAY
		public int CountUniqueProducts ()
		{
			var query = db.Query<Product> (
				"select distinct p.ID from [Product] p where Inactive = 0 and Deleted = 0 and StartDate <= ? and EndDate >= ?", 
				DateTime.Now.ToLocalTime().Date, DateTime.Now.ToLocalTime().Date);
			return query.Count();
		}


		// FUTURE DATED ORDERS WHERE StartDate > TODAY
		public int CountFutureProducts ()
		{
			var query = db.Query<Product> (
				"select distinct p.ID from [Product] p where Inactive = 0 and Deleted = 0 and StartDate > ? and EndDate >= ?", DateTime.Now.ToLocalTime(), DateTime.Now.ToLocalTime().Date);

//			var query = db.Query<Product> (
//				"select distinct p.ID from [Product] p where Inactive = 0 and Deleted = 0 and StartDate > ? and EndDate >= ?", DateTime.Now, DateTime.Now);
			


			return query.Count();
		}

		public int CountProviders ()
		{
			return db.Table<Provider> ().Where(t => t.InActive == false && t.Deleted == false). Count ();
			// query below returns the same value as the one above
//			var query = db.Query<Customer> ("select distinct Name from Provider");
//			return query.Count();
		}

		public int CountWarehouseByProvider (long providerId) {
			return db.Table<ProviderWarehouse> ().Where (t => t.ProviderID == providerId && t.StartDate <= DateTime.Now 
				&& t.EndDate >= DateTime.Now && t.InActive == false && t.Deleted == false).Count();
		}

		public int CountAllWarehouse ()
		{
			return db.Table<ProviderWarehouse> ().Count ();
		}

		public int CountUniqueWarehouse ()
		{
			var query = db.Query<ProviderWarehouse> (
				"select distinct ID from ProviderWarehouse where Inactive = 0 and Deleted = 0 and StartDate <= ? and EndDate >= ?", 
				DateTime.Now, DateTime.Now);
			return query.Count ();
		}

		// ------ DB Maintenance ------
		/// <summary>
		/// Recreates the local tables, intended for one-way synchronization
		/// </summary>
		public void resetReferenceTables ()
		{
			DateTime lastMonth = DateTime.Now.AddMonths(-1);
			var logEntriesForDeletion = db.Table<Log> ().Where (t => t.logTimeStamp <= lastMonth);
			foreach (var l in logEntriesForDeletion) {
				db.Delete<Log> (l.PK);
			}
			db.DropTable<Customer> ();
			db.CreateTable<Customer> ();
			db.DropTable<Product> ();
			db.CreateTable<Product> ();
			db.DropTable<Provider> ();
			db.CreateTable<Provider> ();
			db.DropTable<ProductGroup> ();
			db.CreateTable<ProductGroup> ();
			db.DropTable<ProductGroupLookup> ();
			db.CreateTable<ProductGroupLookup> ();
			db.DropTable<ProviderWarehouse> ();
			db.CreateTable<ProviderWarehouse> ();
		}

		/// <summary>
		/// Recreates the order tables.
		/// </summary>
		public void resetOrderTables ()
		{
			db.DropTable<Order> ();
			db.CreateTable<Order> ();
			db.DropTable<OrderLine> ();
			db.CreateTable<OrderLine> ();
			db.DropTable<Log> ();
			db.CreateTable<Log> ();
		}



		public bool hasCustomerOldID(Order order){

			IList<Customer> _customer = db.Table<Customer> ().Where (t => t.ID == order.CustomerID || t.OldCustomerID == order.CustomerID).ToList ();
			if (_customer.Count > 0) {
				return _customer [0].OldCustomerID > 0;
			}

			return false;


		}

		
		public bool hasProductOldID(long productID, out long oldProductID){
	
			IList<Product> _product = db.Table<Product> ().Where (t => t.ID == productID || t.OldProductID == productID).ToList ();
			if (_product.Count > 0) {
				oldProductID = _product [0].OldProductID;
				return _product [0].OldProductID > 0;
			}
			oldProductID = 0;
			return false;

		}
	}
}