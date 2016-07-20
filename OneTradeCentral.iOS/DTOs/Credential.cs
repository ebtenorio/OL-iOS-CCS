using System;

using  SQLite;

namespace OneTradeCentral.DTOs
{
	public class Credential
	{
		public Credential ()
		{
		}

		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		public string Name { get; set; }
		public string Password { get; set; }
		public string DevideID { get; set; }
		public Int16 FailedLoginAttempts { get; set; }
		public DateTime LastLogin { get; set; }
		public DateTime UpdateDate { get; set; }
	}
}

