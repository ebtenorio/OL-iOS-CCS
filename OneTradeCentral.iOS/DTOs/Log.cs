using System;

using SQLite;

namespace OneTradeCentral.DTOs
{
	public class Log
	{
		public Log ()
		{
		}

		public enum LOG_TYPE { INFO, WARNING, ERROR };
		public enum SEVERITY { NA, TRIVIAL, MINOR, MAJOR, CRITICAL }

		[PrimaryKey, AutoIncrement]
		public int PK { get; set; }

		public DateTime logTimeStamp { get; set; }
		public LOG_TYPE logType { get; set; }
		public SEVERITY severity { get; set; }
		public string component { get; set; }
		public string message { get; set; }
	}
}

