using System;

using  SQLite;

namespace OneTradeCentral.DTOs
{
	public class Distributor
	{
		public Distributor ()
		{
		}
		
		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		public long ID {get; set; }

		public string Code {
			get;
			set;
		}

		public string TIN {
			get;
			set;
		}

//		public int AddressPK { get; set; }
//		[Ignore]
//		public Address BusinessAddress { get; set; }

		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }

		public float Latitude {
			get;
			set;
		}

		public float Longitude {
			get;
			set;
		}

		// marks the record for deletion, used in synchronizing with the server
		public bool Deleted { get; set; }

		// the following fields are incorporated from the WS definition

		// TODO: is this company name or company id?
		public string Company { get; set; }

		public long AddressID { get; set; }

		public bool InActive { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime DateUpdated { get; set; }

		public int CreatedByUserID { get; set; }

		public int UpdatedByUserID { get; set; }
	}
}

