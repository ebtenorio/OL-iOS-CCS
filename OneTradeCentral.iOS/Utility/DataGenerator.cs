using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using OneTradeCentral.DTOs;
using OneTradeCentral.DAO;

namespace OneTradeCentral.Utility
{
	public class DataGenerator : IDAOFactory
	{
		public DataGenerator ()
		{
		}

		// ------ User Account ------
		public UserAccount getUserAccount ()
		{
			throw new NotImplementedException ();
		}

		public bool hasCustomerOldID (Order order){
			throw new NotImplementedException ();
		}

		public void saveUserAccount (UserAccount userAccount)
		{
			throw new NotImplementedException ();
		}
		
		// ------ App Info ------
		public AppInfo getApplicationInfo ()
		{
			throw new NotImplementedException ();
		}
		
		public void updateApplicationInfo (AppInfo appInfo)
		{
			throw new NotImplementedException ();
		}

		// ------ Order ------

		public Order getOrder (int PK)
		{
			throw new NotImplementedException ();
		}

		public IList<Order> getAllPendingOrders (int salesRepAccountID)
		{
			throw new NotImplementedException ();
		}

		public IList<Order> getOrderList (int salesRepAccountID)
		{
			IList<Order> orderList = new List<Order> ();
			for (int i=0; i<15; i++) {
//				CustomerDTO customer = new CustomerDTO ();
//				customer.Name = "ACE Hardware " + i.ToString ();
				var c = generateCustomer ("ACE Hardware " + i.ToString ());
				Order order = new Order ();
				order.Customer = c;
				orderList.Add (order);
			}
			return orderList;
		}

		public IList<OrderLine> getOrderLineListByOrderPK (int orderPK)
		{
			throw new NotImplementedException ();
		}

		public void populateOrder (Order order)
		{
			throw new NotImplementedException ();
		}

		public void populateOrderWithOldIDs (Order order)
		{
			throw new NotImplementedException ();
		}

		public int CountOrdersByStatus (Order.STATUS pending, int salesRepAccountID)
		{
			throw new NotImplementedException ();
		}

		public IList<Order> getOrderListByStoreName (string storeName)
		{
			throw new NotImplementedException ();
		}

		public IList<Order> getOrderListByStoreID (string storeID)
		{
			throw new NotImplementedException ();
		}

		public IList<Order> getOrderListByOrderNumber (string orderNumber)
		{
			throw new NotImplementedException ();
		}

		public IList<Order> getOrderListByOrderDate (DateTime orderDate)
		{
			throw new NotImplementedException ();
		}

		public IList<Product> getProductList ()
		{
			IList<Product> productList = new List<Product> ();
			for (int i=0; i<15; i++) {
				Product product = new Product ();
				product.Name = "iPad X" + i.ToString ();
				product.VendorName = "Apple, Inc.";
//				product.UnitPrice = 50.00m;
				product.SKU = "8013200";
				product.Category = "Tablet";
				product.ID = i;
				productList.Add (product);
			}
			return productList;
		}

		public Product getProduct (BarCodeType type, string barcode)
		{
			var productList = getProductList ();
			var r = new Random ();
			var i = r.Next (0, productList.Count-1);
			return productList [i];
		}

		public IList<Product> getProductListByProductName (long providerID, string productName, bool? IsRegularOrder)
		{
			throw new NotImplementedException ();
		}


		public IList<Product> getProductListByOldID (long oldID)
		{
			throw new NotImplementedException ();
		}

		public IList<Product> getProductListByProductCode (long providerID, string productCode)
		{
			throw new NotImplementedException ();
		}

		public IList<Distributor> getDistributorList ()
		{
			IList<Distributor> distributorList = new List<Distributor> ();
			for (int i=0; i< 10; i++) {
				Distributor distributor = new Distributor ();
				distributor.Company = "Distributor " + i.ToString ();
				
				distributor.Street1 = "Unit 506, 50F Corporate Tower Plaza";
				distributor.Street2 = "Broadway Lane";
				distributor.City = "Canberra";
				distributor.Country = "AU";
			}
			return distributorList;
		}

		Customer generateCustomer (string name)
		{
			Customer c = new Customer ();
			c.Name = name;
			
			c.BillToStreet1 = "Unit 506, 50F Corporate Tower Plaza";
			c.BillToStreet2 = "Broadway Lane";
			c.BillToCity = "Canberra";
			c.BillToCountry = "AU";

			c.ShipToStreet1 = "562 Parktown Mall";
			c.ShipToStreet2 = "32nd Avenue";
			c.ShipToCity = "New South Wales";
			c.ShipToCountry = "AU";

			return c;
		}

		public IList<Customer> getActiveCustomerList ()
		{
			IList<Customer> customerList = new List<Customer> ();
			IList<string> customerNames = File.ReadAllLines ("Data/CompanyNames.txt").ToList ();
			foreach (var name in customerNames) {
				var c = generateCustomer (name);
				customerList.Add (c);
			}
			return customerList;
		}

		public IList<Customer> getActiveUniqueCustomerList ()
		{
			throw new NotImplementedException ();
		}

		public IList<Customer> getAllCustomerList () 
		{
			throw new NotImplementedException ();
		}

		public Customer getCustomerByPK (int PK)
		{
			throw new NotImplementedException ();
		}

		public Customer getCustomerByOldID (long Old)
		{
			throw new NotImplementedException ();
		}

		public Customer getCustomerByProviderID (long providerID, long customerID)
		{
			throw new NotImplementedException ();
		}

		public List<Customer> getAllCustomerByProviderID (long providerID)
		{
			throw new NotImplementedException ();
		}

		public int saveCustomer (Customer customer)
		{
			throw new NotImplementedException ();
		}

		public int updateCustomerContactDetails (Customer customer)
		{
			throw new NotImplementedException ();
		}

		public void deleteCustomer (Customer customer)
		{
			throw new NotImplementedException ();
		}

		public void deleteCustomer (int PK)
		{
			throw new NotImplementedException ();
		}

		public void purgeCustomer (Customer customer)
		{
			throw new NotImplementedException ();
		}

		public void purgeCustomer (int PK)
		{
			throw new NotImplementedException ();
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

		public int saveOrder (Order order)
		{
			throw new NotImplementedException ();
		}

		public void deleteOrder (Order order)
		{
			throw new NotImplementedException ();
		}

		public void deleteOldOrders (int purgePeriod)
		{
			throw new NotImplementedException ();
		}

		public void deleteOrder (int PK)
		{
			throw new NotImplementedException ();
		}

		public void purgeOrder (Order order)
		{
			throw new NotImplementedException ();
		}

		public void purgeOrder (int PK)
		{
			throw new NotImplementedException ();
		}

		// ------ Product ------

		public Product getProduct (int PK)
		{
			throw new NotImplementedException ();
		}

		public int saveProduct (Product product)
		{
			throw new NotImplementedException ();
		}

		public void deleteProduct (Product product)
		{
			throw new NotImplementedException ();
		}

		public void deleteProduct (int PK)
		{
			throw new NotImplementedException ();
		}

		public void purgeProduct (Product product)
		{
			throw new NotImplementedException ();
		}

		public void purgeProduct (int PK)
		{
			throw new NotImplementedException ();
		}

		// ------ Product Group ------

		public int saveProductGroup (ProductGroup productGroup)
		{
			throw new NotImplementedException ();
		}

		public int saveProductGroupLookup (ProductGroupLookup productGroupLookup)
		{
			throw new NotImplementedException ();
		}

		public IList<ProductGroup> getAllProductGroups ()
		{
			throw new NotImplementedException ();
		}

		public int CountProductGroups ()
		{
			throw new NotImplementedException ();
		}


		public int CountFutureProductGroups ()
		{
			throw new NotImplementedException ();
		}

		public IList<ProductGroup> getProductGroupsByProviderID (long providerID)
		{
			throw new NotImplementedException ();
		}

		public IList<ProductGroup> getProductGroupsByName (long providerID, string groupName, bool? IsRegularOrder)
		{
			throw new NotImplementedException ();
		}

		public IList<ProductGroup> getProductGroupsByName (string name)
		{
			throw new NotImplementedException ();
		}

		public IList<ProductGroup> getProductGroupsByProductCode (long providerID, string productCode, bool? IsRegularOrder)
		{
			throw new NotImplementedException ();
		}

		public IList<ProductGroup> getProductGroupsByProductCode (string productCode)
		{
			throw new NotImplementedException ();
		}

		public IList<ProductGroup> getProductGroupsByProductName (long providerID, string productName, bool? IsRegularOrder)
		{
			throw new NotImplementedException ();
		}

		public IList<ProductGroup> getProductGroupsByProductName (string productName)
		{
			throw new NotImplementedException ();
		}

		public IList<Product> getProductListByProductGroup (long providerID, string productGroup)
		{
			throw new NotImplementedException ();
		}

		public IList<Product> getProductListByProvider (long providerID)
		{
			throw new NotImplementedException ();
		}

		public IList<ProductGroup> getAllProductGroupsByProviderID (long providerID)
		{
			throw new NotImplementedException ();
		}

		public IList<Product> getProductListByProductGroupAndProviderID (long providerID, string productGroup, bool? IsRegularOrder, DateTime DateCriteria)
		{
			throw new NotImplementedException ();
		}

		public IList<ProductGroup> getProductGroupsByProduct (Product product)
		{
			throw new NotImplementedException ();
		}

		public IList<Product> getGroupedProductsByProductName (long providerID, string groupName, string productName, bool? IsRegularOrder, DateTime ReleaseDate)
		{
			throw new NotImplementedException ();
		}

		public IList<Product> getGroupedProductsByProductCode (long providerID, string groupName, string productCode, bool? IsRegularOrder)
		{
			throw new NotImplementedException ();
		}
		
		// ------ Payment Terms ------ 
		
		public IList<PaymentTerms> getPaymentTermsList ()
		{
			throw new NotImplementedException ();
		}
		
		public PaymentTerms getPaymentTerms (int PK)
		{
			throw new NotImplementedException ();
		}
		
		public int savePaymentTerms (PaymentTerms terms)
		{
			throw new NotImplementedException ();
		}
		
		public void deletePaymentTerms (PaymentTerms terms)
		{
			throw new NotImplementedException ();
		}
		
		public void deletePaymentTerms (int PK)
		{
			throw new NotImplementedException ();
		}
		
		public void purgePaymentTerms (PaymentTerms terms)
		{
			throw new NotImplementedException ();
		}
		
		public void purgePaymentTerms (int PK)
		{
			throw new NotImplementedException ();
		}
		
		// ------ Record Counter ------

		public int CountAllCustomers() {
			throw new NotImplementedException ();
		}

		public int CountUniqueCustomers ()
		{
			throw new NotImplementedException ();
		}

		public int CountDistributors ()
		{
			throw new NotImplementedException ();
		}

		public int CountOrders (int salesRepAccountID)
		{
			throw new NotImplementedException ();
		}

		public int CountOrderLines (int salesRepAccountID)
		{
			throw new NotImplementedException ();
		}

		public int CountPaymentTerms ()
		{
			throw new NotImplementedException ();
		}

		public int CountProductCategories ()
		{
			throw new NotImplementedException ();
		}

		public int CountUniqueProducts ()
		{
			throw new NotImplementedException ();
		}

		public int CountFutureProducts ()
		{
			throw new NotImplementedException ();
		}

		public int CountProviders ()
		{
			throw new NotImplementedException ();
		}

		public int CountWarehouseByProvider (long providerId)
		{
			throw new NotImplementedException ();
		}

		public int CountAllWarehouse ()
		{
			throw new NotImplementedException ();
		}

		public int CountUniqueWarehouse ()
		{
			throw new NotImplementedException ();
		}

		// ------ Provider -------

		public void saveProvider (Provider provider)
		{
			throw new NotImplementedException ();
		}

		public Provider getProviderByID (long providerID)
		{
			throw new NotImplementedException ();
		}

		public Provider getProviderByOldID (long oldID)
		{
			throw new NotImplementedException ();
		}

		public IList<Provider> getProviderList ()
		{
			throw new NotImplementedException ();
		}

		public IList<Provider> getDistinctProviderList ()
		{
			throw new NotImplementedException ();
		}


		public IList<Provider> getProviderListByCustomerID (long customerID)
		{
			throw new NotImplementedException ();
		}		

		public IList<Provider> getProviderListByCustomerIDFiltered (long customerID)
		{
			throw new NotImplementedException ();
		}

		public Provider getProviderByCustomerAndProviderID (long customerID, long providerID)
		{
			throw new NotImplementedException ();
		}

		// ------ Provider Warehouse -------

		public void saveProviderWarehouse (ProviderWarehouse warehouse)
		{
			throw new NotImplementedException ();
		}

		public ProviderWarehouse getProviderWarehouseByID (int warehouseID)
		{
			throw new NotImplementedException ();
		}

		public ProviderWarehouse getProviderWarehouseByOldID (long oldID)
		{
			throw new NotImplementedException ();
		}

		public IList<ProviderWarehouse> getProviderWarehouseList ()
		{
			throw new NotImplementedException ();
		}

		public IList<ProviderWarehouse> getProviderWarehouseListByProviderID (long providerID)
		{
			throw new NotImplementedException ();
		}

		// ------ Log Entries ------

		public void saveLog (Log log)
		{
			throw new NotImplementedException ();
		}

		public IList<Log> getLogAllEntries ()
		{
			throw new NotImplementedException ();
		}

		public IList<Log> getLogEntriesByType (Log.LOG_TYPE logType)
		{
			throw new NotImplementedException ();
		}

		public IList<Log> getLogEntriesBySeverity (Log.SEVERITY severity)
		{
			throw new NotImplementedException ();
		}

		public IList<Log> getLogEntriesByComponent (string component)
		{
			throw new NotImplementedException ();
		}

		public IList<Log> getLogEntriesByDay (DateTime day)
		{
			throw new NotImplementedException ();
		}

		public IList<Log> getLogEntriesByMonth (DateTime month)
		{
			throw new NotImplementedException ();
		}

		public IList<Log> getLogEntriesFromToday ()
		{
			throw new NotImplementedException ();
		}
		// ------ DB Maintenance ------
		public void resetReferenceTables ()
		{
			throw new NotImplementedException ();
		}

		public void resetOrderTables ()
		{
			throw new NotImplementedException ();
		}
	}
}

