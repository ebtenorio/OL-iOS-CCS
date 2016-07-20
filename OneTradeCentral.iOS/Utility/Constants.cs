using System;
using System.IO;

namespace OneTradeCentral.iOS
{
	public class Constants
	{

		// testwslogin.orderlinc.com:443
		// testwsorder.orderlinc.com:443
		// test-au.orderlinc.com:443

		public static String PRODUCTION_URL = "https://wslogin.orderlinc.com";
		public static String DEVELOPMENT_URL = "http://121.96.75.227:7000/loginws/";
		public static String TEST_URL ="https://testwslogin.orderlinc.com";
		public static String STAGING_URL ="https://stglogin.orderlinc.com";
		public static String AWS_URL ="http://ec2-52-11-71-83.us-west-2.compute.amazonaws.com/OrderLinc_Login";

		public static String UNREGISTERED = "Unregistered";
		public static String DEFAULTORDERLINCVERSION = "0";
		public static String REGULAR_ORDER = "Regular Order";
		public static String PRESELL_ORDER= "Pre-sell Order";

	

		public enum ORDERTYPE{
			REGULAR = 0,
			PRESELL = 1
		};


		public static string RegistryServiceURL {
			get {
				// FIXME: shouldn't we be using http
//				return "http://login.orderlinc.com/OrderLincRegistry.asmx"; // Original OrderLinc
//				return DEVELOPMENT_URL;
//				return STAGING_URL;
//				return TEST_URL;
				return PRODUCTION_URL; 
				//return AWS_URL;
				}
		}
			

//		public static string OrderLincWebServiceURL {
//			get {
//				return "http://app.orderlinc.com/";
//
//			}
//		}

//		public static string SignatureImageUploadURL {
//			get {
//				return "http://app.orderlinc.com/ReceivedSig.aspx";
//			}
//		}

		public static string LogoFilePath {
			get {
				string logoPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				string logoFileName = "logo.png";
				string localFilePath = Path.Combine (logoPath, logoFileName);
				return localFilePath;
			}
		}

//		public static long DefaultProviderID {
//			get {
//				return 1000;
//			}
//		}

		public static int PurgePeriodInDays{
			get{
				return 90;
			}
		}
			

		public static int DefaultSysOrderStatusID {
			get {
				return 100;
			}
		}

		public static string DefaultOrderStatusText {
			get {
				return "RECEIVED";
			}
		}

		public static string SigImageDirectory {
			get {
				var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
				var library = Path.Combine (documents, "..", "Library");
				var SigImageDirectory = Path.Combine (library, "SigImages");
				return SigImageDirectory;
			}
		}

		public static string GetImageFilePath(string filename) {
			var filepath = Path.Combine (SigImageDirectory, filename);
			return filepath;
		}

		private Constants ()
		{
		}
			
	}
}