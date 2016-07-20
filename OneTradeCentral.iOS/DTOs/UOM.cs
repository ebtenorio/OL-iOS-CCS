using System;

using SQLite;

namespace OneTradeCentral.DTOs
{
	public class UOM
	{
		public UOM ()
		{
		}
		
		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		public string Code { get; set; }
		public string Name { get; set; }
	}
}

