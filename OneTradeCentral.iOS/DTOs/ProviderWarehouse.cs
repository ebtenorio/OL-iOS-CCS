using System;

using SQLite;

namespace OneTradeCentral.DTOs
{
	public class ProviderWarehouse
	{
		public ProviderWarehouse ()
		{
		}
		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }
		public int ID { get; set; } // ProviderWarehouseID
		public long OldProviderWarehouseID { get; set; } // Old ID

		public long ProviderID { get; set; }
		public string Code { get; set; } // ProviderWarehouseCode
		public string Name { get; set; } // ProviderWarehouseName
		public long AddressID { get; set; }
		public float Longitude { get; set; }
		public float Latitude { get; set; }
		public bool Deleted { get; set; }
		public bool InActive { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
	}
}

