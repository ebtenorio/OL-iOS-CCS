using System;
using SQLite;

namespace OneTradeCentral.DTOs
{
	public class Provider
	{
		public Provider ()
		{
		}
		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		public long ID { get; set; } // ProviderID
		public long OldProviderID  { get; set; }

		public string Code { get; set;} // ProviderCode
		public string Phone { get; set; } // BusinessNumber
		public string Name { get; set; } // ProviderName
		public long SalesOrgID { get; set; } 
		public long AddressID { get; set; }
		public bool Deleted { get; set; }
		public bool InActive { get; set; }
	}
}

