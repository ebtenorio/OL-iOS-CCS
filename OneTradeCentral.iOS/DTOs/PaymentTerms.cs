using System;

using  SQLite;

namespace OneTradeCentral.DTOs

{
	public class PaymentTerms
	{
		public PaymentTerms ()
		{
		}
		
		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }
		
		// marks the record for deletion, used in synchronizing with the server
		public bool Deleted { get; set; }

		/// <remarks/>
		public int PaymentTermID {get; set;}
		
		/// <remarks/>
		public string Title {get; set;}
	}
}

