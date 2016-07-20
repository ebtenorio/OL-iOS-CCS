using System;
using System.Collections.Generic;

using OneTradeCentral.DTOs;

namespace OneTradeCentral.iOS
{
	public class Logger
	{
		private static volatile Logger _instance;
		private static object _syncRoot = new object();
		private static DALFacade dal = new DALFacade();

		private Logger ()
		{
		}

		public static Logger Instance {
			get {
				if (_instance == null) {
					lock (_syncRoot) {
						if (_instance == null)
							_instance = new Logger ();
					}
				}
				return _instance;
			}
		}

		public static void log (string message) {
			Log log = new Log();
			log.logTimeStamp = DateTime.Now;
			log.logType = Log.LOG_TYPE.INFO;
			log.severity = Log.SEVERITY.NA;
			log.component = "App";
			log.message = message;
			dal.saveLog (log);
		}

		public static void log (Log.LOG_TYPE logType, Log.SEVERITY severity, string message) {
			Log log = new Log();
			log.logTimeStamp = DateTime.Now;
			log.logType = logType;
			log.severity = severity;
			log.component = "App";
			log.message = message;
			dal.saveLog (log);
		}

		public static IList<Log> getAllLogs() {
			return dal.getLogAllEntries ();
		}
	}
}

