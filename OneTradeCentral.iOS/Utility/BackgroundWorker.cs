using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using OneTradeCentral.DAO;
using OneTradeCentral.DTOs;

namespace OneTradeCentral.iOS
{
	/// <summary>
	/// Singleton instance that is responsible for performning synchronization tasks.
	/// Eliminates race condition risks brought by multiple background tasks uploading the same data.
	/// </summary>
	public class BackgroundWorker
	{
		private static volatile BackgroundWorker _instance;
		private static object _syncRoot = new object ();

//		[Flags]
//		public enum ORDER_STATUS { Pending=1, Processing=2, Partial=4, Complete=8 }
		public enum STATUS { Running, Idle };
		public enum REQUEST { Started, Ignored, Queued };

		// make sure only one upload is running at a given time
		private static volatile STATUS _uploadStatus = STATUS.Idle;

		private BackgroundWorker ()
		{
		}

		/// <summary>
		/// Thread-safe method for returning and, if necessary, initializing an instance of SynchronizationWorker.
		/// </summary>
		/// <value>An instance of SynchronizationWorker.</value>
		public static BackgroundWorker Instance {
			get {
				if (_instance == null) {
					lock (_syncRoot) {
						if (_instance == null) {
							_instance = new BackgroundWorker ();
						}
					}
				}
				return _instance;
			}
		}

		public STATUS UploadStatus {
			get {
				return _uploadStatus;
			}
		}

		public bool UploadRunning {
			get {
				return (_uploadStatus == STATUS.Running);
			}
		}

		public REQUEST UploadOrder (Order order)
		{
			if (UploadRunning) {
//				Logger.log ("UploadOrder already running, ignoring new request.");
				return REQUEST.Ignored;
			} else {
				nint taskID = UIApplication.SharedApplication.BeginBackgroundTask (() => {
					lock(_syncRoot) {
						_uploadStatus = STATUS.Idle;
					}
				});
				new Task (() => {
					_uploadStatus = STATUS.Running;
					WebServiceFacade ws = new WebServiceFacade ();
					DALFacade dal = new DALFacade ();
					try {
						processOrder (order, ws, dal);
					} catch (Exception e) {
						Logger.log ("Error Sending Order: " + e.Message);
					} finally {
						_uploadStatus = STATUS.Idle;
						dal = null;
						ws = null;
					}
					UIApplication.SharedApplication.EndBackgroundTask(taskID);
				}).Start ();
			}
//			Logger.log ("Upload Order Started new request.");
			return REQUEST.Started;
		}

//		private void updatePartialOrder (Order order, long orderNumber, DALFacade dal)
//		{
//			order.OrderNumber = orderNumber.ToString ();
//			order.ID = orderNumber;
//			order.UploadStatus = (int)Order.STATUS.Partial;
//			dal.SaveOrder (order);
//			Logger.log ("Order No: " + order.OrderNumber + ", " + order.CustomerName);
//		}

//		private void uploadSignature(Order order) {
//			WebServiceFacade ws = new WebServiceFacade ();
//			ws.uploadSignature (order);
//		}

		private void processOrder (Order order, WebServiceFacade ws, DALFacade dal)
		{
			// sanity check, make sure we process only PO's that have been persisted locally 
			// For MPE: also and do not process orders with existing order numbers (no more partial uploads)

			if (order.PK > 0  && (order.OrderNumber == null || order.OrderNumber.Trim().Length == 0)) {
				switch ((Order.STATUS) order.UploadStatus) {
				// we don't use Order.STATUS.Processing currently, so ignore this
//				case Order.STATUS.Processing:
//					break;
				case Order.STATUS.Pending:
					Logger.log (String.Format ("Sending Order for {0}", order.CustomerName));
					var orderNumber = ws.sendOrder (order);
					if (orderNumber > 0) {
						order.OrderNumber = orderNumber.ToString ();
						order.ID = orderNumber;
						order.UploadStatus = (int) Order.STATUS.Completed;
						dal.SaveOrder (order);

						_uploadStatus = STATUS.Idle; // October 1, 2015

						Logger.log ("Uploaded Order No: " + order.OrderNumber + "-" + order.CustomerName);


//						updatePartialOrder (order, orderNumber, dal);
//						uploadSignature (order);
					} else {
						Logger.log (String.Format ("Unable to send Order for {0}, sendOrder = {1}", 
						                                  order.CustomerName, orderNumber.ToString ()));
					}
					break;
//				case Order.STATUS.Partial:
//					if (order.OrderNumber != null && order.OrderNumber.Trim ().Length > 0)
//						uploadSignature (order);
//					break;
				default:
					Logger.log (String.Format("Unhandled Order ({0}) Status: {1}.", order.OrderNumber, 
					                   (Order.STATUS) order.UploadStatus));
					break;
				}
			}
			else {
				if (order.PK > 0)
					Logger.log (String.Format("Order for {0} not saved, Skipping Upload", order.CustomerName));
				if (order.ID <= 0)
					Logger.log (String.Format("Existing Order {0}-{1}, Skipping Upload", order.OrderNumber, order.CustomerName));
			}
		}

		public REQUEST UploadAllPending () {
			if (false) {
//				Logger.log ("Uploader already running, ignoring UploadAll request.");
				return REQUEST.Ignored;
			} else {
				nint taskID = UIApplication.SharedApplication.BeginBackgroundTask (() => {
					lock(_syncRoot) {
						_uploadStatus = STATUS.Idle;
					}
				});
				new Task (() => {
					_uploadStatus = STATUS.Running;
					WebServiceFacade ws = new WebServiceFacade();
					DALFacade dal = new DALFacade();
					try {
						if(dal.getUserAccount() != null){
						IList<Order> pendingOrders = dal.getAllPendingOrders((int)dal.getUserAccount().AccountID);
							foreach (var order in pendingOrders) {
								processOrder (order, ws, dal);
							}

						}
					} catch (Exception e) {

						if(e.Message == "Error: NameResolutionFailure"){
							Logger.log("Error UploadAllPending: Please check Internet connection");
						}
						else{
							Logger.log("Error UploadAllPending: " + e.Message);
						}

						_uploadStatus = STATUS.Idle;

					} finally {
						_uploadStatus = STATUS.Idle;
						dal = null;
						ws = null;
					}
					UIApplication.SharedApplication.EndBackgroundTask(taskID);
				}).Start();
//				Logger.log ("UploadAll request Started.");
				return REQUEST.Started;
			}
		}
	}
} 

