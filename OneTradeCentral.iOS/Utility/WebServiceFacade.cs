using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Globalization;
using System.Net;
using System.IO;
using System.Text;
using UIKit;
using OneTradeCentral.iOS;
using OneTradeCentral.DTOs;
using OneTradeCentral.iOS.OrderLincRegistry;
using OneTradeCentral.iOS.OrdersWebService;


// New libs
using System.Threading.Tasks;
using Foundation;


namespace OneTradeCentral.iOS
{
	public class WebServiceFacade
	{
		private static DateTime MinimumDate = DateTime.Parse ("Jan 1, 1900", new CultureInfo ("en-US"));

		OrderLincRegistry.Service1 registryWS = new OrderLincRegistry.Service1(Constants.RegistryServiceURL);

		private static UserAccount _userAccount = null;

		// Original
		// private static OrderAppWebService.mobileservices _orderAppWS = null;

		// New Code
		private static OrdersWebService.mobileservices _orderAppWS = null;

		UserAccount userAccount {
			get {
				if (_userAccount == null || _userAccount.AccountID == 0) {
					DALFacade dal = new DALFacade ();
					_userAccount = dal.getUserAccount ();
				} 
				return _userAccount;
			}
		}

		// Original Code below:
//		OrderAppWebService.mobileservices orderAppWS {
//			get {
//				if (_orderAppWS == null) {
//					_orderAppWS = new OrderAppWebService.mobileservices(userAccount.ServerURL);
//				}
//				return _orderAppWS;
//			}
//		}


		// New Code using the enhanced web service
		OrdersWebService.mobileservices orderAppWS {
			get {
				if (_orderAppWS == null) {
					_orderAppWS = new OrdersWebService.mobileservices(userAccount.ServerURL);
				}
				return _orderAppWS;
			}
		}


		public WebServiceFacade () {
		}

		private void setUserAccount (OrderLincRegistry.DTOMobileAccount dtoAccount) {
			_userAccount = null;
			if (dtoAccount != null) {
				_userAccount = new UserAccount ();
				_userAccount.AccountID = dtoAccount.AccountID;
				_userAccount.RefID = dtoAccount.RefID;
				_userAccount.AccountTypeID = dtoAccount.AccountTypeID;
				_userAccount.FirstName = dtoAccount.FirstName;
				_userAccount.LastName = dtoAccount.LastName;
				// until the web service is updated to include SalesOrgID, do this
				_userAccount.SalesOrgID = dtoAccount.RefID;  
				_userAccount.OrgUnitID = dtoAccount.OrgUnitID;
//				_userAccount.Deleted = dtoAccount.Deleted;
				_userAccount.Email = dtoAccount.Email;
//				_userAccount.TotalRows = dtoAccount.TotalRows;
//				_userAccount.InActive = dtoAccount.InActive;
				_userAccount.Username = dtoAccount.Username;
				_userAccount.ServerURL = dtoAccount.ServerUrl;
				_userAccount.LogoURL = dtoAccount.Logo;
//				_userAccount.UploadImageURL = dtoAccount.UploadImageURL;
//				_userAccount.ServerID = dtoAccount.ServerID;
				_userAccount.AddressID = dtoAccount.AddressID;
				_userAccount.ContactID = dtoAccount.ContactID;
				 
			} 
		}

		public UserAccount login(string loginName, string password) {
			if (Identity.DeviceIdentifier() != Constants.UNREGISTERED) {
				var dtoAccount = registryWS.RegisterDevice (loginName, password, Identity.DeviceIdentifier());
				setUserAccount (dtoAccount);
			}
			return _userAccount;
		}

		public UserAccount login(string loginName, string password, string deviceID) {
			if (Identity.DeviceIdentifier() != Constants.UNREGISTERED) {
				setUserAccount (registryWS.RegisterDevice (loginName, password, deviceID));
			}
			return _userAccount;
		}

		public UserAccount registerDevice(string loginName, string password, string orderLincVersion, string iOSVersion) {
		
			if (Identity.DeviceIdentifier() != Constants.UNREGISTERED) {
				var dtoAccount = registryWS.RegisterDevice (loginName, password, Identity.DeviceIdentifier());

				if (dtoAccount != null && dtoAccount.AccountID > 0) {
					setUserAccount (dtoAccount);
				} else {
					throw new Exception ("Login error.");
				}
			}
			return _userAccount;
		}

		public UserAccount registerDevice(string loginName, string password, string deviceID) {
			if (deviceID != Constants.UNREGISTERED) {
				setUserAccount (registryWS.RegisterDevice (loginName, password, deviceID));
			}
			return _userAccount;
		}

		public DataSet getReferenceData(long salesOrgNo, long repAccountNo, string orderLincVersion, string iOSVersion) {
			return orderAppWS.GetReferenceData (salesOrgNo, repAccountNo, orderLincVersion, iOSVersion);
		}

		public DateTime? LastVersionReleaseDate(){

			string _sample = orderAppWS.LastReleaseDate ().ToString ();

			return DateTime.Parse (_sample);

		}

		public long sendOrder (Order order)
		{
			// TODO we shouldn't be logging in for each web service call
//			DateTime todate = DateTime.Now;
			DTOOrder purchaseOrder = new DTOOrder ();
			purchaseOrder.OrderNumber = order.OrderNumber;
			// #132, user editable store ID is now being set as customer ID
//			purchaseOrder.StoreID = order.StoreID;
			purchaseOrder.CustomerID = order.CustomerID;
			purchaseOrder.CustomerName = order.CustomerName;
			purchaseOrder.ProviderID = order.ProviderID;
			purchaseOrder.ProviderWarehouseID = order.WarehouseID;
			purchaseOrder.ProviderCustomerCode = order.StoreID;

			DTOContact contact = new DTOContact();
			// FIXME: Do we need to assign Contact ID?
//			contact.ContactID = oder.StoreMgrID;
			contact.FirstName = order.StoreMgrFirstName;
			contact.LastName = order.StoreMgrLastName;
			contact.Email = order.StoreMgrEmail;
			contact.Mobile = order.StoreMgrPhone;
//			purchaseOrder.FirstName = order.StoreMgrFirstName;
//			purchaseOrder.LastName = order.StoreMgrLastName;
//			purchaseOrder.Email = order.StoreMgrEmail;
//			purchaseOrder.Mobile = order.StoreMgrPhone;


			purchaseOrder.IsRegularOrder = order.IsRegularOrder;

			if (order.RequestedReleaseDate == MinimumDate) {
				purchaseOrder.RequestedReleaseDate = null;
			}
			else{
				purchaseOrder.RequestedReleaseDate = (DateTime) order.RequestedReleaseDate;
			}


			purchaseOrder.OrderDate = order.OrderDate;
//			purchaseOrder.TotalRows = order.OrderLineList.Count;
			purchaseOrder.SalesOrgID = order.SalesOrgID;
			purchaseOrder.SalesRepAccountID = order.SalesRepAccountID;
			purchaseOrder.CreatedByUserID = order.CreatedByUserID;
			purchaseOrder.UpdatedByUserID = order.UpdatedByUserID;
			purchaseOrder.DateCreated = order.DateCreated;
			purchaseOrder.DateUpdated = order.DateUpdated;
			purchaseOrder.InvoiceDate = MinimumDate;
			purchaseOrder.ReceivedDate = MinimumDate;
			purchaseOrder.ReleaseDate = MinimumDate;
			purchaseOrder.DeliveryDate = MinimumDate;
			purchaseOrder.CreatedByUserID = order.SalesRepAccountID;
			purchaseOrder.UpdatedByUserID = order.SalesRepAccountID;
//			purchaseOrder.FullName = order.StoreMgrFirstName.Trim() + " " + order.StoreMgrLastName.Trim();
			purchaseOrder.IsHeld = order.IsHeld;
			purchaseOrder.IsSent = order.IsSent;
			purchaseOrder.OrderGUID = order.GUID;
//			purchaseOrder.OrderStatusText = order.OrderStatusText;
			purchaseOrder.SYSOrderStatusText = order.SYSOrderStatusText;
			purchaseOrder.SYSOrderStatusID = order.SYSOrderStatusID;


			// sanity check
			if (order.OrderLineList != null && order.OrderLineList.Count > 0) {

				List<DTOOrderLine> orderLines = new List<DTOOrderLine>();
				int lineNumber = 1;
				foreach (var orderDetail in order.OrderLineList) {
					DTOOrderLine lineItem = new DTOOrderLine ();
					lineItem.LineNum = lineNumber++;
					lineItem.OrderLineID = lineNumber;
//					lineItem.ProductID = orderDetail.Product.ID;
//					lineItem.ProductCode = orderDetail.Product.Code;
//					lineItem.ProductName = orderDetail.Product.Name;
//					lineItem.GTINCode = orderDetail.Product.GTINCode;
//					lineItem.UOM = orderDetail.Product.UOM;
					lineItem.ProductID = orderDetail.ProductID;
					lineItem.ProductCode = orderDetail.ProductCode;
					lineItem.ProductName = orderDetail.ProductName;
					lineItem.GTINCode = orderDetail.ProductGTINCode;
					lineItem.UOM = orderDetail.ProductUOM;
					lineItem.OrderQty = orderDetail.Quantity;
					orderLines.Add (lineItem);
				}
				purchaseOrder.OrderLine = orderLines.ToArray ();

//			var poNumber = orderAppWS.SendOrder (purchaseOrder, orderLineArray, Identity.UserName, Identity.Password, Identity.DeviceID);

				// this is the new submit order web method.  2014-05-08
				var sigImageFilePath = Constants.GetImageFilePath (order.SignatureFilename);
				byte[] signatureByteArray;
				using (FileStream fs = new FileStream (sigImageFilePath, FileMode.Open)) {
					signatureByteArray = new byte[fs.Length];
					fs.Read (signatureByteArray, 0, signatureByteArray.Length);
				}

				String serverMessage;
   				var poNumber = orderAppWS.SubmitOrder (purchaseOrder, signatureByteArray, contact, 
					Identity.UserName, Identity.Password, Identity.DeviceIdentifier(), out serverMessage);

				return poNumber;
			} else {
				Logger.log ("Empty Orderlines, will not process order for " + purchaseOrder.CustomerName);
				return 0;
			}
		}

		public IList<Order> getPurchaseOrders() 
		{
			// TODO: Ask WebService team to implement a getPO method
			throw new NotImplementedException ();
		}

		public void downloadLogo(UserAccount userAccount) {
			var webClient = new WebClient ();
			var downloadURI = new Uri (userAccount.LogoURL);
			// Synchronous Approach
			var imageBytes = webClient.DownloadData (downloadURI);
			File.WriteAllBytes (Constants.LogoFilePath, imageBytes);
		}

		public bool IsCorrectOrdersWSVersion(string _majorVersion, string _minorVersion, out string _errorMessage, out string _errorHeader){

			bool _isCorrectVersion = false;
			_errorMessage = "";
			_errorHeader = "";

			try{
		
				_isCorrectVersion =  orderAppWS.VersionControl (_majorVersion, _minorVersion, out _errorMessage);



				if(!_isCorrectVersion){
					Logger.log("Web service connection rejected (Orders): wrong version");
					_errorHeader = "Connection Rejected";
				}
			}
			catch(Exception e){
				Logger.log (e.Message);//("Web Service Error (Orders): Please check connection.");
				_errorMessage = e.Message;
				_errorHeader = "Web Service Error";
			}

			return _isCorrectVersion ;
		}

		public bool IsCorrectRegistryWSVersion(string _majorVersion, string _minorVersion, out string _errorMessage, out string _errorHeader){

			bool _isCorrectVersion = false;

			_errorMessage = "";
			_errorHeader = "";

			try{
				_isCorrectVersion = registryWS.VersionControl (_majorVersion, _minorVersion, out _errorMessage);

				if(!_isCorrectVersion){
					Logger.log("Web service connection rejected (Registry): wrong version");
					_errorHeader = "Connection Rejected";
				}

			}
			catch(Exception e){

				Logger.log (e.Message);// ("Web Service Error (Registry): Please check connection.");
				_errorMessage = e.Message;
				_errorHeader = "Web Service Error";

			}

		
			return _isCorrectVersion ;
		}

			
	}
}

