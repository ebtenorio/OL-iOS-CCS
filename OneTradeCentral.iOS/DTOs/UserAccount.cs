using System;

using SQLite;

namespace OneTradeCentral.DTOs
{
	public class UserAccount
	{
		public UserAccount ()
		{
		}

		// use "PK" as the primary key for the internal DB in order to differentiate this from
		// the "ID" column from the web service
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		public long AccountID { get; set; }

		public long RefID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public int AccountTypeID { get; set; }

		public string AccountTypeText { get; set; }

		public long SalesOrgID { get; set; }

		public int OrgUnitID { get; set; }

		public string Username { get; set; }

		public string DeviceNo { get; set; }

		public string ServerURL { get; set; }

		public string LogoURL { get; set; }

		public long AddressID { get; set; }

		public long ContactID { get; set; }

		public string UploadImageURL { get; set; }

		public int ServerID { get; set; }

		/// <summary>
		/// The total number of orders submitted by the user?
		/// </summary>
		public int TotalRows { get; set; }

		public bool Deleted { get; set; }

		public bool InActive { get; set; }

		public bool Lockout { get; set; }

		public DateTime DateCreated { get; set; }

		public DateTime DateUpdated { get; set; }

		public int CreatedByUserID { get; set; }

		public int UpdatedByUserID { get; set; }
	}
}

