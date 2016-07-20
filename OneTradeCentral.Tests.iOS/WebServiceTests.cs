
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

using OneTradeCentral.iOS;
using OneTradeCentral.DTOs;
using OneTradeCentral.iOS.OrderAppWebService;

namespace OneTradeCentral.Tests.iOS
{
	[TestFixture]
	public class WebServiceTests
	{

		static string testUserName1 = "pepsico";
		static string testUserName2 = "wrigley";
		static string testUserPassword = "1234";

		[Test]
		public void Pass ()
		{
			Assert.True (true);
		}

//		[Test]
//		public void Fail ()
//		{
//			Assert.False (true);
//		}

//		[Test]
//		[Ignore ("another time")]
//		public void Ignore ()
//		{
//			Assert.True (false);
//		}

		[Test]
		public void TestLoginBlankDeviceID() {
			WebServiceFacade wsf = new WebServiceFacade ();
			var userAccount = wsf.login (testUserName1, testUserPassword, "");
			Assert.Null (userAccount, "Unable to login with no device ID.");
			userAccount = wsf.login ("invalidUser", "invalidPassword", "");
			Assert.Null (userAccount, "Was able to login with incorrect password and no device ID.");
//			Assert.IsTrue (repID == 0, "Was able to login with incorrect password and no device ID.");
		}

		[Test]
		public void TestLoginWithDeviceID() {
			WebServiceFacade wsf = new WebServiceFacade ();
			var userAccount1 = wsf.login (testUserName1, testUserPassword);
			Assert.NotNull (userAccount1, "Unable to login with system generated device ID.");
			userAccount1 = wsf.login ("invalidUser", "invalidPassword");
			Assert.Null (userAccount1, "Was able to login with incorrect password but correct device ID.");

			var userAccount2 = wsf.login (testUserName2, testUserPassword);
			Assert.NotNull (userAccount2, "Unable to login with system generated device ID.");
			userAccount2 = wsf.login ("invalidUser", "invalidPassword");
			Assert.Null (userAccount2, "Was able to login with incorrect password but correct device ID.");
		}

		[Test]
		public void TestRegisterDevice() {
			WebServiceFacade wsf = new WebServiceFacade ();
			var userAccount = wsf.registerDevice (testUserName1, testUserPassword);
			Assert.IsNotNull (userAccount);
			Assert.IsTrue (userAccount.AccountID > 0, "Unable to register system generated device ID.");
		}

		[Test]
		public void TestRegisterWrongDevice() {
			WebServiceFacade wsf = new WebServiceFacade ();
			var userAccount = wsf.registerDevice (testUserName1, testUserPassword, "");
			Assert.IsFalse (userAccount.AccountID > 0, "Was able to register a blank device ID.");
			userAccount = wsf.registerDevice (testUserName1, testUserPassword, "234123312dfwfs34513");
			Assert.IsFalse (userAccount.AccountID > 0, "Was able to re-register a different device ID.");
		}

		[Test]
		public void TestGetCustomerList() {
			// TODO User credentials should be stored in the keychain

			WebServiceFacade wsf = new WebServiceFacade();
			Assert.NotNull (wsf, "Unable to instantiate a WebService Façade.");
//			IList<Customer> customers = wsf.downloadCustomerList();
//			Assert.NotNull (customers, "WebService should return a list of customers.");
//			Assert.That( customers.Count > 0, "WebService should return at least one customer record.");
		}

		// deprecated in OrderLinc
//		[Test]
//		public void TestGetDistributorList() {
//			WebServiceFacade wsf = new WebServiceFacade();
//			Assert.NotNull (wsf, "Unable to instantiate a WebService Façade.");
//			IList<Distributor> distributors = wsf.getDistributorList();
//			Assert.NotNull (distributors, "WebService should return a list of distributors.");
//			Assert.That ( distributors.Count > 0, "WebService should return at least one distributor.");
//		}

		[Test]
		public void TestGetProductList() {
			// TODO User credentials should be stored in the keychain

			WebServiceFacade wsf = new WebServiceFacade();
			Assert.NotNull (wsf, "Unable to instantiate a WebService Façade.");
//			IList<Product> products = wsf.downloadProductList();
//			Assert.NotNull (products, "WebService should return a list of products.");
//			Assert.That (products.Count > 0, "WebService should return at least one product.");
		}
		
		[Test]
		public void TestSendPO() {
			WebServiceFacade wsf = new WebServiceFacade();
			Assert.NotNull (wsf, "Unable to instantiate a WebService Façade.");
			Order order = new Order();
			OrderLine orderDetail = new OrderLine();
			order.OrderLineList.Add(orderDetail);
			long poNumber = wsf.sendOrder(order);
//			long poNumber = wsf.sendPOEX(order);
			Assert.NotNull(poNumber, "WebService should return a PO number.");
			Assert.That (poNumber > 0, "WebService should return a positive integer as the PO number.");
			
		}

		// deprecated in OrderLinc
//		[Test]
//		public void TestSendPOLine() {
//			WebServiceFacade wsf = new WebServiceFacade();
//			Assert.NotNull (wsf, "Unable to instantiate a WebService Façade.");
//			OrderLine orderDetail = new OrderLine();
//			wsf.sendPOLine(orderDetail);
//		}

//		[Test]
//		public void TestSendPOEX() {
//			WebServiceFacade wsf = new WebServiceFacade();
//			Assert.NotNull (wsf, "Unable to instantiate a WebService Façade.");
//			Order order = new Order();
//			OrderLine orderDetail = new OrderLine();
//			order.OrderLineList.Add(orderDetail);
//			long poNumber = wsf.sendPOEX(order);
//			Assert.NotNull(poNumber, "WebService should return a PO Number.");
//			Assert.That (poNumber > 0, "PO Number should be greater than zero.");
//
//		}

		[Test]
		[Ignore ("Web Method Not Yet Available")]
		public void getPurchaseOrders() {
		}
	}
}
