using System;

using  SQLite;

namespace OneTradeCentral.DTOs
{
	public class OrderLine
	{
		public OrderLine ()
		{
		}
		
		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		public OrderLine (Product product, int quantity, decimal discount)
		{
			this.Product = product;
			this.Quantity = quantity;
			this.DiscountRate = discount;
		}

		[Ignore]
		public Product Product { get; set; }

		public int ProductPK { get; set; }

		// foreign key that links OrderLines with Products
		public long ProductID { get; set; }

		public string ProductCode {get; set; }
		public string ProductName {get; set; }
		public string ProductUOM {get; set; }
		public string ProductSKU {get; set; }
		public string ProductGTINCode {get; set; }
		public long ProductProviderID { get; set; }

		// foreign key that links OrderLines with Orders
		public int OrderPK { get; set; }
		public string OrderNumber { get; set; }

		public int Quantity { get; set; }

		// TODO: implement methods for computed fields
		public decimal DiscountRate { get; set; }
		public decimal DiscountAmount { get; set; }
		public decimal TaxRate { get; set; }

//		public decimal TotalPrice { 
//			get {
//				return Convert.ToDecimal (Quantity) * Product.UnitPrice;
//			} 
//		}
		
		// marks the record for deletion, used in synchronizing with the server
		public bool Deleted { get; set; }

		// the following fields are incorporated from the WS definition

		// used "ID" instead of POLineID for consistency
//		public long ID { get; set; }

		public long OrderID { get; set; }

		// Not needed, information is included in ProductDTO
//		public long ProductTypeID;
//		public long ProductID;
//		public decimal UnitPrice;

		public double PickedQty { get; set; }
		public int DiscountID { get; set; }
		public string Remarks { get; set; }
		public long CreatedUserID { get; set; }
		public bool IsPicked { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateUpdated { get; set; }
	}
}

