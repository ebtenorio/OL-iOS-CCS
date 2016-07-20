using System;

using  SQLite;

namespace OneTradeCentral.DTOs
{
	public class Customer
	{
		public Customer ()
		{
		}

		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		public long ID { get; set; } // CustomerID?
		public long ProviderID { get; set; } // ProviderID
		public long OldCustomerID { get; set; } // Old ProviderID



		// Customer code is Store ID for Wrigley's/MetCash
		public string Code { get; set; } // ProviderCustomerCode
		public string Name { get; set; } // CustomerName

		public string ContactFirstName { get; set; } // FirstName
		public string ContactLastName { get; set; } // LastName
		public string ContactEmail { get; set; } // Email
		public string ContactNumber { get; set; } // Phone
		public string ContactFax { get; set; } // Fax
		public string ContactMobile { get; set; } // Mobile
		public string StateName { get; set; }
		public string StateCode { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public long SalesRepAccountID {get; set; } // SalesRepAccountID
		public long SalesOrgID { get; set; }

		public DateTime DateCreated {get; set; }
		public System.DateTime DateUpdated  {get; set; }
		public int CreatedByUserID {get; set; }
		public int UpdatedByUserID {get; set; }
		public bool InActive {get; set; }
		public bool Deleted { get; set; }

//		[Ignore]
		//		public Address BillToAddress { get; set; }

		public string OfficeStreet1 { get; set; }
		public string OfficeStreet2 { get; set; }
		public string OfficeCity { get; set; }
		public string OfficeStateCode { get; set; }
		public string OfficeStateName { get; set; }
		public string OfficeCountry { get; set; }
		public string OfficeZipCode { get; set; }

		public string getOfficeAddress() {
			return formatAddress(BillToStreet1, BillToStreet2, BillToCity, BillToZipCode, BillToCountry);
		}

		public string BillToStreet1 { get; set; }
		public string BillToStreet2 { get; set; }
		public string BillToCity { get; set; }
		public string BillToStateCode { get; set; }
		public string BillToStateName { get; set; }
		public string BillToCountry { get; set; }
		public string BillToZipCode { get; set; }

		public string getBillToAddress() {
			return formatAddress(BillToStreet1, BillToStreet2, BillToCity, BillToZipCode, BillToCountry);
		}

		public bool SameShipToAddress { get; set; }

		public string ShipToStreet1 { get; set; }
		public string ShipToStreet2 { get; set; }
		public string ShipToCity { get; set; }
		public string ShipToStateCode { get; set; }
		public string ShipToStateName { get; set; }
		public string ShipToCountry { get; set; }
		public string ShipToZipCode { get; set; }

		public string getShipToAddress() {
			return formatAddress(ShipToStreet1, ShipToStreet2, ShipToCity, ShipToZipCode, ShipToCountry);
		}

		public string formatAddress(string street1, string street2, string city, string zipcode, string country) {
			string formattedAddress = "";
			if (street1 != null && street1.Trim() != "")
				formattedAddress +=  street1.Trim();
			if (street2 != null && street2.Trim() != "")
				formattedAddress += (formattedAddress == "" ? street2.Trim() : ", " + street2.Trim());
			if (city != null && city.Trim() != "")
				formattedAddress += (formattedAddress == "" ? city.Trim() : ", " + city.Trim());
			if (zipcode != null && zipcode.Trim() != "")
				formattedAddress += (formattedAddress == "" ? zipcode.Trim() : ", " + zipcode.Trim());
			if (country != null && country.Trim() != "")
				formattedAddress += (formattedAddress == "" ? country.Trim() : ", " + country.Trim() );
			return formattedAddress;
		}

		public string getDisplayAddress() {
			if (getShipToAddress() != "")
				return getShipToAddress();
			else
				return getBillToAddress();
		}

		public float Latitude {
			get;
			set;
		}

		public float Longitude {
			get;
			set;
		}

		// the following fields are incorporated from the WS definition
		
		/// <remarks/>
		public string TIN {get; set; }
		
		/// <remarks/>
		public long IndustryID {get; set; }
		
		/// <remarks/>
		// TODO: is this company name or company id.  Probably name since it is a string.  :-)
		public string Company {get; set; }
		
		/// <remarks/>
		public long CustomerTypeID {get; set; }
		
		/// <remarks/>
		public decimal CreditLimit {get; set; }
		
		/// <remarks/>
		public int CreditTerms {get; set; }
	}
}