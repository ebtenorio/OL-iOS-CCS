using System;

using SQLite;

namespace OneTradeCentral.DTOs
{
	public class Address
	{
		public Address ()
		{
		}
		/// <summary>
		/// Gets or sets the local Primary Key.
		/// </summary>
		/// <value>The internal primary key.</value>
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		/// <summary>
		/// Represents the Server ID of the address.
		/// </summary>
		/// <value>The ID.</value>
		public long ID { get; set; }
		
		public string Street1 { get; set; }
		public string Street2 { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }
	}
}

