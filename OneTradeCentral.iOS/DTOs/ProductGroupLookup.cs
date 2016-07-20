using System;

using SQLite;

namespace OneTradeCentral.DTOs {
	public class ProductGroupLookup
	{
		public ProductGroupLookup ()
		{
		}

//		public ProductGroupLookup(int productPK, string productCode, string productName, string groupName) {
		public ProductGroupLookup(long productID, int productGroupID) {
//			ProductPK = productPK;
//			ProductCode = productCode;
//			ProductName = productName;
//			GroupName = groupName;
			ProductID = productID;
			ProductGroupID = productGroupID;
		}

		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

//		[Indexed]
//		public int ProductPK { get; set; }

		[Indexed]
		public long ProductID { get; set; }

		// uncommented
		[Indexed]
		public string ProductCode { get; set; }

		// uncommented
		[Indexed]
		public string ProductName { get; set; }

		[Indexed]
		public int ProductGroupID { get; set; }

		// uncommented
		[Indexed]
		public string GroupName { get; set; }
	}
}

