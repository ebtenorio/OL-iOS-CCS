using System;

using  SQLite;

namespace OneTradeCentral.DTOs
{
	public class ProductCategory
	{
		public ProductCategory ()
		{
		}
		
		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		// used "ID" instead of "ProductCategoryID" for consistency
		public int ID { get; set; }

		public string Name { get; set; }

		public int ParentCategoryID { get; set; }

		public bool InActive { get; set; }

		// marks the record for deletion, used in synchronizing with the server
		public bool Deleted { get; set; }
	}
}

