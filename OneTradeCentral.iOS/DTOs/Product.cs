using System;

using  SQLite;

namespace OneTradeCentral.DTOs
{
	public class Product
	{
		public Product ()
		{
		}
		
		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		public long ID { get; set; } // ProductID <-- web service field
		public long ProviderID { get; set; } // added to support multiple providers

		public long OldProductID { get; set; } // Used to handle old object IDs from the Database
		public string Code { get; set; } // ProviderProductCode <-- web service field
		public string Name { get; set; } // ProductDescription <-- web service field
		public string SKU { get; set; }
		public string UOM { get; set; } // ProductUOM <-- web service field
		public string VendorName { get; set; }
		public string Category { get; set; }

		public string GTINCode { get; set; }
		public DateTime StartDate { get; set; } 
		public DateTime EndDate { get; set; }

		public int ProductCategoryID { get; set; }
		public int SalesOrgID { get; set; }
		[Ignore]
		public string[] GroupNames { get; set; }

//		public decimal UnitPrice { get; set; }
//		public bool Available { get; set; }
//		public string BarcodeID { get; set; }
//		public string UpdateDate { get; set; }

		// For retrieving, storing and displayin product images - not needed by Wrigley's
//		public string ImageURL { get; set; }
//		public string ImageFileName { get; set; }
//		public long ImageFileSize { get; set; }
//		public DateTime ImageUpdateDate { get; set; }
//		public string ImageType { get; set; }

		public bool Inactive { get; set; }
		public bool Deleted { get; set; }

		public override string ToString ()
		{
			return string.Format ("{0}:{1}.", Code, Name);
		}
	}
}

