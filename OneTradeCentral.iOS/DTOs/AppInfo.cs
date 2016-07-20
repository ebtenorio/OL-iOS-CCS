using System;

using SQLite;

namespace OneTradeCentral.DTOs
{
	public class AppInfo
	{
		public static readonly string MAIN_TITLE = "OrderLinc";

		public AppInfo ()
		{
		}

		[PrimaryKey]
		public int PK { get; set; }

		public DateTime LastSyncDate { get; set; }
	}
}

