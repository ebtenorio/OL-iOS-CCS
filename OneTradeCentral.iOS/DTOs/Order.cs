using System;
using System.Collections.Generic;

using  SQLite;

namespace OneTradeCentral.DTOs
{
	public class Order
	{
		public Order ()
		{
			OrderDate = DateTime.UtcNow;
			Customer = new Customer ();
			OrderLineList = new List<OrderLine>();
		}
		
		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }
		
		public long ID { get; set;}
		public string OrderNumber { get; set; }

		public int CustomerPK { get; set; }
		public string CustomerName { get; set; }
		public long CustomerID { get; set; }

		[Ignore]
		public Customer Customer { get; set; }

		[Ignore]
		public IList<OrderLine> OrderLineList { get; set; }

		// #132, let sales reps enter the store ID
		public string StoreID { get; set; }
		public String GUID { get; set; }
		// store manager information, he's the guy/gal that signs the PO
		public string StoreMgrFirstName { get; set; }
		public string StoreMgrLastName { get; set; }
		public string StoreMgrEmail { get; set; }
		public string StoreMgrPhone { get; set; }
		
		public DateTime OrderDate { get; set; }
		public DateTime HoldDate { get; set; }
		public DateTime DeliveryDate { get; set; }
		public DateTime InvoiceDate { get; set; }
		public DateTime ReceivedDate { get; set; }
		public DateTime ReleaseDate { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DatePosted { get; set; }
		public DateTime DateUpdated { get; set; }
		public long CreatedByUserID { get; set; }
		public long UpdatedByUserID { get; set; }

		public long SalesOrgID { get; set; }
		public int SalesRepAccountID { get; set; }
		public long ProviderID { get; set; }
		public string ProviderName { get; set; }
		public int WarehouseID { get; set; }
		public int ProviderWarehouseID { get; set; }
		public string WarehouseName { get; set; }
		public int SYSOrderStatusID { get; set; }
		public bool IsSent { get; set; }
		public bool IsHeld { get; set; }
		public string SYSOrderStatusText { get; set; }
		public int TotalRows { get; set; }
		public string OrderStatusText { get; set; }
		public string SignatureFilename { get; set; }

		// Two new additional fields 
		public bool? IsRegularOrder { get; set; } = null;
		public DateTime RequestedReleaseDate { get; set; }

		// marks the record for deletion, used in synchronizing with the server
		public bool Deleted { get; set; }

		// used internally by the iOS app for processing uploads, see Order.STATUS for the values used here
		[Flags]
		public enum STATUS { Pending=1, Processing=2, Partial=4, Completed=8 }
		public int UploadStatus { get; set; }


	}
}

