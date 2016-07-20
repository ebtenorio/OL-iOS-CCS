using System;
using SQLite;

namespace OneTradeCentral.DTOs
{
	public class ProductGroup
	{
		public ProductGroup ()
		{
		}

		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		[Indexed]
		public int ID { get; set; }

		public long SalesOrgID { get; set; }

		[Indexed]
		public string Name { get; set; }
	}
}

