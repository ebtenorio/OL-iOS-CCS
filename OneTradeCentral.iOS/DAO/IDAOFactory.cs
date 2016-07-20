using System;
using System.Collections.Generic;

using OneTradeCentral.DTOs;

namespace OneTradeCentral.DAO
{
	public interface IDAOFactory
	{
		// ------ User Account ------

		/// <summary>
		/// Returns the current user account.
		/// </summary>
		/// <returns>The user account.</returns>
		UserAccount getUserAccount();

		/// <summary>
		/// Determines whether an order contains an Old ID.
		/// </summary>
		/// <returns>The user account.</returns>
		bool hasCustomerOldID (Order order);

		/// <summary>
		/// Saves the current user account.
		/// </summary>
		void saveUserAccount(UserAccount userAccount);

		// ------ App Info ------ 

		/// <summary>
		/// Gets the application info.
		/// </summary>
		/// <returns>The application info.</returns>
		AppInfo getApplicationInfo();

		/// <summary>
		/// Updates the application info.
		/// </summary>
		/// <param name="appInfo">App info.</param>
		void updateApplicationInfo(AppInfo appInfo);

		// ------ Customers ------ 
		/// <summary>
		/// Gets the list of all active customers.
		/// </summary>
		/// <returns>The customer list.</returns>
		IList<Customer> getActiveCustomerList();

		/// <summary>
		/// Gets the list of all active customers regardless of Provider.
		/// </summary>
		/// <returns>The active unique customer list.</returns>
		/// <param name="">.</param>
		IList<Customer> getActiveUniqueCustomerList ();

		/// <summary>
		/// Gets all customer list.
		/// </summary>
		/// <returns>The all customer list.</returns>
		IList<Customer> getAllCustomerList();

		/// <summary>
		/// Gets the customer.
		/// </summary>
		/// <returns>The customer.</returns>
		/// <param name="PK">Primary Key.</param>
		Customer getCustomerByPK(int PK);

		/// <summary>
		/// Gets the customer by Old Customer ID.
		/// </summary>
		/// <returns>The customer.</returns>
		/// <param name="OldID">Primary Key.</param>
		Customer getCustomerByOldID(long OldID);


		/// <summary>
		/// Gets the customer by provider I.
		/// </summary>
		/// <returns>The customer by provider I.</returns>
		/// <param name="providerID">Provider I.</param>
		/// <param name="customerID">Customer I.</param>
		Customer getCustomerByProviderID (long providerID, long customerID);

		/// <summary>
		/// Gets the customer by provider I.
		/// </summary>
		/// <returns>The customer by provider I.</returns>
		/// <param name="providerID">Provider I.</param>
		List<Customer> getAllCustomerByProviderID (long providerID);

		/// <summary>
		/// Saves the customer.
		/// </summary>
		/// <returns>The customer's primary key.</returns>
		/// <param name="customer">Customer.</param>
		int saveCustomer(Customer customer);

		/// <summary>
		/// Updates all customer contact details based on the customer parameter.
		/// </summary>
		/// <returns>The number of records updated.</returns>
		/// <param name="customer">Customer.</param>
		int updateCustomerContactDetails (Customer customer);

		/// <summary>
		/// Deletes the customer.
		/// </summary>
		/// <param name="customer">Customer.</param>
		void deleteCustomer(Customer customer);

		/// <summary>
		/// Deletes the customer.
		/// </summary>
		/// <param name="PK">P.</param>
		void deleteCustomer(int PK);

		/// <summary>
		/// Purges the customer.
		/// </summary>
		/// <param name="customer">Customer.</param>
		void purgeCustomer(Customer customer);

		/// <summary>
		/// Purges the customer.
		/// </summary>
		/// <param name="PK">P.</param>
		void purgeCustomer(int PK);

		// ------ Distributor ------ 

		/// <summary>
		/// Gets the distributor list.
		/// </summary>
		/// <returns>The distributor list.</returns>
		IList<Distributor> getDistributorList();

		/// <summary>
		/// Gets the distributor.
		/// </summary>
		/// <returns>The distributor.</returns>
		/// <param name="PK">P.</param>
		Distributor getDistributor(int PK);

		/// <summary>
		/// Saves the distributor.
		/// </summary>
		/// <returns>The distributor.</returns>
		/// <param name="distributor">Distributor.</param>
		int saveDistributor(Distributor distributor);

		/// <summary>
		/// Deletes the distributor.
		/// </summary>
		/// <param name="distributor">Distributor.</param>
		void deleteDistributor(Distributor distributor);

		/// <summary>
		/// Deletes the distributor.
		/// </summary>
		/// <param name="PK">P.</param>
		void deleteDistributor(int PK);

		/// <summary>
		/// Purges the distributor.
		/// </summary>
		/// <param name="distributor">Distributor.</param>
		void purgeDistributor(Distributor distributor);

		/// <summary>
		/// Purges the distributor.
		/// </summary>
		/// <param name="PK">P.</param>
		void purgeDistributor(int PK);

		// ------ Orders ------ 

//		/// <summary>
//		/// Gets the order list.
//		/// </summary>
//		/// <returns>The order list.</returns>
//		IList<Order> getOrderList();
//
		/// <summary>
		/// Gets the order list.
		/// </summary>
		/// <returns>The order list.</returns>
		IList<Order> getOrderList(int salesRepAccountID);

		/// <summary>
		/// Populates the order child objects with values.
		/// </summary>
		/// <param name="order">Order.</param>
		void populateOrder(Order order);

		/// <summary>
		/// Populates the order child objects with values, and using the OldIDs
		/// </summary>
		/// <param name="order">Order.</param>
		void populateOrderWithOldIDs (Order order);

		/// <summary>
		/// Gets the order line list by order PK.
		/// </summary>
		/// <returns>The order line list by order P.</returns>
		/// <param name="orderPK">Order PK.</param>
		IList<OrderLine> getOrderLineListByOrderPK (int orderPK);

		/// <summary>
		/// Counts the orders by status.
		/// </summary>
		/// <returns>The number of orders by status.</returns>
		/// <param name="pending">Pending.</param>
		int CountOrdersByStatus (Order.STATUS pending, int salesRepAccountID);

		/// <summary>
		/// Gets all Orders that have not yet completed processing.
		/// </summary>
		/// <returns>The all pending orders.</returns>
		IList<Order> getAllPendingOrders (int salesRepAccountID);

		/// <summary>
		/// Gets the name of the order list by store name.
		/// </summary>
		/// <returns>The order list by store name.</returns>
		/// <param name="storeName">Store name.</param>
		IList<Order> getOrderListByStoreName(string storeName);

		/// <summary>
		/// Gets the order list by store ID.
		/// </summary>
		/// <returns>The order list by store ID.</returns>
		/// <param name="storeID">Store ID.</param>
		IList<Order> getOrderListByStoreID(string storeID);

		/// <summary>
		/// Gets the order list by order number.
		/// </summary>
		/// <returns>The order list by order number.</returns>
		/// <param name="orderNumber">Order number.</param>
		IList<Order> getOrderListByOrderNumber(string orderNumber);

		/// <summary>
		/// Gets the order list by order date.
		/// </summary>
		/// <returns>The order list by order date.</returns>
		/// <param name="orderDate">Order date.</param>
		IList<Order> getOrderListByOrderDate(DateTime orderDate);

		/// <summary>
		/// Gets the order.
		/// </summary>
		/// <returns>The order.</returns>
		/// <param name="PK">P.</param>
		Order getOrder(int PK);

		/// <summary>
		/// Saves the order.
		/// </summary>
		/// <returns>The order.</returns>
		/// <param name="order">Order.</param>
		int saveOrder(Order order);

		/// <summary>
		/// Deletes the order.
		/// </summary>
		/// <param name="order">Order.</param>
		void deleteOrder(Order order);

		/// <summary>
		/// Deletes all orders more than 90 days old.
		/// </summary>
		void deleteOldOrders(int purgePeriod);


		/// <summary>
		/// Deletes the order.
		/// </summary>
		/// <param name="PK">P.</param>
		void deleteOrder(int PK);

		/// <summary>
		/// Purges the order.
		/// </summary>
		/// <param name="order">Order.</param>
		void purgeOrder(Order order);

		/// <summary>
		/// Purges the order.
		/// </summary>
		/// <param name="PK">P.</param>
		void purgeOrder(int PK);

		// ------ Product ------ 

		/// <summary>
		/// Gets the product list.
		/// </summary>
		/// <returns>The product list.</returns>
		IList<Product> getProductList();

		/// <summary>
		/// Gets the name of the product list by product.
		/// </summary>
		/// <returns>The product list by product name.</returns>
		/// <param name="productName">Product name.</param>
		IList<Product> getProductListByProductName (long providerID, string productName, bool? IsRegularOrder);

		/// <summary>
		/// Gets the name of the product list by product.
		/// </summary>
		/// <returns>The product list by product name.</returns>
		/// <param name="productName">Product name.</param>
		IList<Product> getProductListByOldID (long oldID);


		/// <summary>
		/// Gets the product list by product code.
		/// </summary>
		/// <returns>The product list by product code.</returns>
		/// <param name="productCode">Product code.</param>
		IList<Product> getProductListByProductCode (long providerID, string productCode);

		/// <summary>
		/// Gets the product.
		/// </summary>
		/// <returns>The product.</returns>
		/// <param name="PK">P.</param>
		Product getProduct(int PK);
		
//		/// <summary>
//		/// Gets a product based on its barcode
//		/// </summary>
//		/// <returns>The product.</returns>
//		/// <param name="type">Type.</param>
//		/// <param name="barcode">Barcode.</param>
//		Product getProduct (BarCodeType type, string barcode);

		/// <summary>
		/// Saves the product.
		/// </summary>
		/// <returns>The product.</returns>
		/// <param name="product">Product.</param>
		int saveProduct(Product product);

		/// <summary>
		/// Deletes the product.
		/// </summary>
		/// <param name="product">Product.</param>
		void deleteProduct(Product product);

		/// <summary>
		/// Deletes the product.
		/// </summary>
		/// <param name="PK">P.</param>
		void deleteProduct(int PK);

		/// <summary>
		/// Purges the product.
		/// </summary>
		/// <param name="product">Product.</param>
		void purgeProduct(Product product);

		/// <summary>
		/// Purges the product.
		/// </summary>
		/// <param name="PK">P.</param>
		void purgeProduct(int PK);

		// ------ Product Group ------

		/// <summary>
		/// Saves the product group.
		/// </summary>
		/// <returns>The product group.</returns>
		/// <param name="productGroup">Product group.</param>
		int saveProductGroup (ProductGroup productGroup);

		/// <summary>
		/// Saves the product group lookup.
		/// </summary>
		/// <returns>The product group lookup.</returns>
		/// <param name="productGroupLookup">Product group lookup.</param>
		int saveProductGroupLookup(ProductGroupLookup productGroupLookup);

		/// <summary>
		/// Gets all product groups.
		/// </summary>
		/// <returns>The all product groups.</returns>
		IList<ProductGroup> getAllProductGroups();

		/// <summary>
		/// Gets the product group count.
		/// </summary>
		/// <returns>The product group count.</returns>
		int CountProductGroups ();

		/// <summary>
		/// Gets the product group count.
		/// </summary>
		/// <returns>The product group count.</returns>
		int CountFutureProductGroups ();

		/// <summary>
		/// Gets all product groups by provider I.
		/// </summary>
		/// <returns>The all product groups by provider I.</returns>
		IList<ProductGroup> getProductGroupsByProviderID (long providerID);

		/// <summary>
		/// Gets the name of the product groups by.
		/// </summary>
		/// <returns>The product groups by name.</returns>
		/// <param name="providerID">Provider I.</param>
		/// <param name="groupName">Group name.</param>
		IList<ProductGroup> getProductGroupsByName(long providerID, string groupName, bool? IsRegularOrder);

		/// <summary>
		/// Gets the name of the product groups by.
		/// </summary>
		/// <returns>The product groups by name.</returns>
		/// <param name="name">Name.</param>
		IList<ProductGroup> getProductGroupsByName(string groupName);

		/// <summary>
		/// Gets the product groups by product code.
		/// </summary>
		/// <returns>The product groups by product code.</returns>
		/// <param name="providerID">Provider I.</param>
		/// <param name="productCode">Product code.</param>
		IList<ProductGroup> getProductGroupsByProductCode (long providerID, string productCode, bool? IsRegularOrder);

		/// <summary>
		/// Gets the product groups by product code.
		/// </summary>
		/// <returns>The product groups by product code.</returns>
		/// <param name="productCode">Product code.</param>
		IList<ProductGroup> getProductGroupsByProductCode (string productCode);

		/// <summary>
		/// Gets the name of the product groups by product.
		/// </summary>
		/// <returns>The product groups by product name.</returns>
		/// <param name="providerID">Provider I.</param>
		/// <param name="productName">Product name.</param>
		IList<ProductGroup> getProductGroupsByProductName (long providerID, string productName, bool? IsRegularOrder);

		/// <summary>
		/// Gets the name of the product groups by product.
		/// </summary>
		/// <returns>The product groups by product name.</returns>
		/// <param name="productName">Product name.</param>
		IList<ProductGroup> getProductGroupsByProductName (string productName);

		/// <summary>
		/// Gets a list of products by product group.
		/// </summary>
		/// <returns>The product by product group.</returns>
		/// <param name="productGroup">Product group.</param>
		IList<Product> getProductListByProductGroup (long providerID, string productGroup);

		/// <summary>
		/// Gets the product list by product group and provider ID.
		/// </summary>
		/// <returns>The product list by product group and provider I.</returns>
		/// <param name="productGroup">Product Group.</param>
		/// <param name="providerID">Provider ID.</param>
		IList<Product> getProductListByProductGroupAndProviderID (long providerID, string productGroup, bool? IsRegularOrder, DateTime DateCriteria);

		/// <summary>
		/// Gets the product list by product group and provider ID.
		/// </summary>
		/// <returns>The product list by product group and provider I.</returns>
		/// <param name="productGroup">Product Group.</param>
		/// <param name="providerID">Provider ID.</param>
		IList<Product> getProductListByProvider (long providerID);

		/// <summary>
		/// Gets the list of products under the specified group that matches given the product name.
		/// </summary>
		/// <returns>The grouped products by product name.</returns>
		/// <param name="groupName">Group name.</param>
		/// <param name="productName">Product name.</param>
		IList<Product> getGroupedProductsByProductName (long providerID, string groupName, string productName, bool? IsRegularOrder, DateTime ReleaseDate);

		/// <summary>
		/// Gets the list of products under the specified group that matches given the product code.
		/// </summary>
		/// <returns>The grouped products by product code.</returns>
		/// <param name="groupName">Group name.</param>
		/// <param name="productCode">Product code.</param>
		IList<Product> getGroupedProductsByProductCode (long providerID, string groupName, string productCode, bool? IsRegularOrder);

		/// <summary>
		/// Gets a product's product group.
		/// </summary>
		/// <returns>The product group by product.</returns>
		/// <param name="product">Product.</param>
		IList<ProductGroup> getProductGroupsByProduct (Product product);

		// ------ Payment Terms ------ 

		/// <summary>
		/// Gets the payment terms list.
		/// </summary>
		/// <returns>The payment terms list.</returns>
		IList<PaymentTerms> getPaymentTermsList();

		/// <summary>
		/// Gets the payment terms.
		/// </summary>
		/// <returns>The payment terms.</returns>
		/// <param name="PK">P.</param>
		PaymentTerms getPaymentTerms(int PK);

		/// <summary>
		/// Saves the payment terms.
		/// </summary>
		/// <returns>The payment terms.</returns>
		/// <param name="terms">Terms.</param>
		int savePaymentTerms(PaymentTerms terms);

		/// <summary>
		/// Deletes the payment terms.
		/// </summary>
		/// <param name="terms">Terms.</param>
		void deletePaymentTerms(PaymentTerms terms);

		/// <summary>
		/// Deletes the payment terms.
		/// </summary>
		/// <param name="PK">P.</param>
		void deletePaymentTerms(int PK);

		/// <summary>
		/// Purges the payment terms.
		/// </summary>
		/// <param name="terms">Terms.</param>
		void purgePaymentTerms(PaymentTerms terms);

		/// <summary>
		/// Purges the payment terms.
		/// </summary>
		/// <param name="PK">P.</param>
		void purgePaymentTerms(int PK);

		// ------ Record Counter ------

		/// <summary>
		/// Counts all customers.
		/// </summary>
		/// <returns>The all customers.</returns>
		int CountAllCustomers();

		/// <summary>
		/// Counts the products.
		/// </summary>
		/// <returns>The products.</returns>
		int CountUniqueCustomers();

		/// <summary>
		/// Counts the distributors.
		/// </summary>
		/// <returns>The distributors.</returns>
		int CountDistributors();

		/// <summary>
		/// Counts the orders.
		/// </summary>
		/// <returns>The orders.</returns>
		int CountOrders(int salesRepAccountID);

		/// <summary>
		/// Counts the order lines.
		/// </summary>
		/// <returns>The order lines.</returns>
		int CountOrderLines(int salesRepAccountID);

		/// <summary>
		/// Counts the payment terms.
		/// </summary>
		/// <returns>The payment terms.</returns>
		int CountPaymentTerms();

		/// <summary>
		/// Counts the product categories.
		/// </summary>
		/// <returns>The product categories.</returns>
		int CountProductCategories();
		
		/// <summary>
		/// Counts the products.
		/// </summary>
		/// <returns>The products.</returns>
		int CountUniqueProducts();

		/// <summary>
		/// Counts the products.
		/// </summary>
		/// <returns>The products.</returns>
		int CountFutureProducts();

		/// <summary>
		/// Counts the providers.
		/// </summary>
		/// <returns>The number of providers.</returns>
		int CountProviders();

		/// <summary>
		/// Counts the warehouses by provider.
		/// </summary>
		/// <returns>The warehouse by provider.</returns>
		/// <param name="providerId">Provider identifier.</param>
		int CountWarehouseByProvider (long providerId);

		/// <summary>
		/// Counts all warehouse.
		/// </summary>
		/// <returns>The all warehouse.</returns>
		int CountAllWarehouse ();

		/// <summary>
		/// Counts the unique warehouse.
		/// </summary>
		/// <returns>The unique warehouse.</returns>
		int CountUniqueWarehouse();

		// ------ Provider ------

		void saveProvider (Provider provider);

		Provider getProviderByID (long providerID);

		Provider getProviderByOldID (long oldID);

		IList<Provider> getProviderList ();

		IList<Provider> getDistinctProviderList ();

		IList<Provider> getProviderListByCustomerID (long customerID);

		IList<Provider> getProviderListByCustomerIDFiltered (long customerID);

		Provider getProviderByCustomerAndProviderID (long customerID, long providerID);

		// ------ Provider Warehouse ------

		void saveProviderWarehouse (ProviderWarehouse warehouse);

		ProviderWarehouse getProviderWarehouseByID(int warehouseID);

		ProviderWarehouse getProviderWarehouseByOldID(long oldID);

		IList<ProviderWarehouse> getProviderWarehouseList ();

		IList<ProviderWarehouse> getProviderWarehouseListByProviderID (long providerID);

		// ------ Log Entries ------

		/// <summary>
		/// Inserts a log entry in the Log table.
		/// </summary>
		/// <param name="log">Log.</param>
		void saveLog(Log log);


		/// <summary>
		/// Returns all log entries from the beginning of time.
		/// </summary>
		/// <returns>The log all entries.</returns>
		IList<Log> getLogAllEntries ();

		/// <summary>
		/// Returns all log entries under the given type.
		/// </summary>
		/// <returns>The log entries by type.</returns>
		/// <param name="logType">Log type.</param>
		IList<Log> getLogEntriesByType (Log.LOG_TYPE logType);

		/// <summary>
		/// Returns all log entries with the given severity.
		/// </summary>
		/// <returns>The log entries by severity.</returns>
		/// <param name="severity">Severity.</param>
		IList<Log> getLogEntriesBySeverity (Log.SEVERITY severity);

		/// <summary>
		/// Returns all log entries for the given component
		/// </summary>
		/// <returns>The log entries by component.</returns>
		/// <param name="component">Component.</param>
		IList<Log> getLogEntriesByComponent (string component);

		/// <summary>
		/// Returns the log entries for the given day
		/// </summary>
		/// <returns>The log entries by day.</returns>
		/// <param name="day">Day.</param>
		IList<Log> getLogEntriesByDay (DateTime day);

		/// <summary>
		/// Returns all the log entries under the month of the given date.
		/// </summary>
		/// <returns>The log entries by month.</returns>
		/// <param name="month">Month.</param>
		IList<Log> getLogEntriesByMonth (DateTime month);


		/// <summary>
		/// Returns today's log entries.
		/// </summary>
		/// <returns>The log entries from today.</returns>
		IList<Log> getLogEntriesFromToday ();

		// ------ DB Maintenance ------
		/// <summary>
		/// Recreates the local reference tables.
		/// </summary>
		void resetReferenceTables();

		/// <summary>
		/// Recreates the order tables.
		/// </summary>
		void resetOrderTables();
	}
}

