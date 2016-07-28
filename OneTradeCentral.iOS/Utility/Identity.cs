using System;
using Foundation;
using Security;
using AdSupport;
using UIKit;
using OneTradeCentral.iOS;


namespace OneTradeCentral.iOS
{
	public class Identity
	{
		public Identity ()
		{
			FindKeyChainEntry ();
		}

		// NOTE for the Keychain Groups: use $(AppIdentifierPrefix)$(CFBundleIdentifier)


// 		PROD
		//private static string APP_ID = "EJ24Q24DZP.com.orderlinc.mobile"; // App ID of the original OrderLinc
//		private static string APP_ID = "EJ24Q24DZP.com.orderlinc.mpe"; // App ID of OrderLinc-MPE
		//private static string APP_ID = "9TF8VNJ362.com.blueprintgroup.orderlinc.mpe"; // App ID of OrderLinc-MPE deployed internally by CCS

//		USED FOR THE MEANTIME
//		private static string APP_ID = "9TF8VNJ362.com.blueprintgroup.orderlinc.trg"; // App ID of OrderLinc-TRG deployed internally by CCS
//		private static string APP_ID = "EJ24Q24DZP.com.orderlinc.mobilestg";
		private static string APP_ID = "EJ24Q24DZP.com.orderlinc.mobiletst"; // App ID of the original OrderLinc STG
//		private static string APP_ID = "EJ24Q24DZP.com.orderlinc.reintegration.stg"; // App ID of OrderLinc Reintegration

		private static string _username = null;
		private static string _password = null;

		// IdentifierForVendor
		private static string DEV_KEY = "DeviceIdentifier";
		private static string VER_KEY = "VersionIdentifier";
		private static string LASTVERSIONRELEASEDATE_KEY = "LastVersionReleaseDate";

		// IdentifierForVendor 
		//private static string _deviceIdentifier = null;

		// IdentifierForVendor
		public static string DeviceIdentifier(){ //(string ApplicationID, out SecRecord outSecRecord) {
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Generic = NSData.FromString (DEV_KEY),
				Service = APP_ID,
				Account  = DEV_KEY,// Account and Service should be unique for each keychain entry
			};

			SecStatusCode statusCode;
			var result = SecKeyChain.QueryAsRecord (keyChainRecord, out statusCode);
			//outSecRecord = result;

			if (statusCode == SecStatusCode.Success) {
				return result.ValueData.ToString ();
			} else {
				return Constants.UNREGISTERED;
			}
		}

		// LastVersionReleaseDate
		public static string LastVersionReleaseDate(){ //(string ApplicationID, out SecRecord outSecRecord) {
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Generic = NSData.FromString (LASTVERSIONRELEASEDATE_KEY),
				Service = APP_ID,
				Account  = LASTVERSIONRELEASEDATE_KEY,// Account and Service should be unique for each keychain entry
			};

			SecStatusCode statusCode;
			var result = SecKeyChain.QueryAsRecord (keyChainRecord, out statusCode);
			//outSecRecord = result;

			if (statusCode == SecStatusCode.Success) {
				return result.ValueData.ToString ();
			} else {
				return null;
			}
		}


		// OrderLincVersion
		public static string OrderLincVersion(){ //(string ApplicationID, out SecRecord outSecRecord) {
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Generic = NSData.FromString (VER_KEY),
				Service = APP_ID,
				Account  = VER_KEY,// Account and Service should be unique for each keychain entry
			};

			SecStatusCode statusCode;
			var result = SecKeyChain.QueryAsRecord (keyChainRecord, out statusCode);
			//outSecRecord = result;

			if (statusCode == SecStatusCode.Success) {
				return result.ValueData.ToString ();
			} else {

				return Constants.DEFAULTORDERLINCVERSION;
			}
		}

		// IdentifierForVendor
		public static bool FindKeyChainEntryForDeviceIdentifier() {
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Generic = NSData.FromString (DEV_KEY),
				Service = APP_ID,
				Account = DEV_KEY,
			};
		
			SecStatusCode statusCode;
			var result = SecKeyChain.QueryAsRecord (keyChainRecord, out statusCode);
		
			if (statusCode == SecStatusCode.Success) {
				//_deviceIdentifier = result.ValueData.ToString ();
				return true;
			} else {
				//_deviceIdentifier = Constants.UNREGISTERED;
				return false;
			}
		}

		// LastVersionReleaseDate
		public static bool FindKeyChainEntryForLastVersionReleaseDate() {
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Generic = NSData.FromString (LASTVERSIONRELEASEDATE_KEY),
				Service = APP_ID,
				Account = LASTVERSIONRELEASEDATE_KEY,
			};

			SecStatusCode statusCode;
			var result = SecKeyChain.QueryAsRecord (keyChainRecord, out statusCode);

			if (statusCode == SecStatusCode.Success) {
				//_deviceIdentifier = result.ValueData.ToString ();
				return true;
			} else {
				//_deviceIdentifier = Constants.UNREGISTERED;
				return false;
			}
		}


		// OrderLincVersion
		public static bool FindKeyChainEntryForOrderLincVersion() {
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Generic = NSData.FromString (VER_KEY),
				Service = APP_ID,
				Account = VER_KEY,
			};

			SecStatusCode statusCode;
			var result =SecKeyChain.QueryAsRecord (keyChainRecord, out statusCode);

			if (statusCode == SecStatusCode.Success) {
				return true;
			} else {
				return false;
			}
		}

		// IdentifierForVendor
		 public static bool CreateKeyChainEntryForDeviceIdentifier(string identifierForVendor) {				
		
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Label = "DeviceIdentifier",
				Description = "Uniquely identifies the device.",
				Comment = "This is suppose to persist the IdentifierForVendor.",
				ValueData = NSData.FromString (identifierForVendor),
				Generic = NSData.FromString (DEV_KEY),
				Service = APP_ID, // Account and Service should be unique for each keychain entry
				Account = DEV_KEY, // Account and Service should be unique for each keychain entry
			};
		
			var statusCode = SecKeyChain.Add (keyChainRecord);
		
			if (statusCode != SecStatusCode.Success && statusCode != SecStatusCode.DuplicateItem) {
				Logger.log ("Error adding keychain record : " + statusCode.ToString ());
				return false;
			} else {
				//_deviceIdentifier = identifierForVendor;
				return true;
			}
		}


		// LastVersionReleaseDate
		public static bool CreateKeyChainEntryForLastVersionReleaseDate(string _lastReleaseDate) {				

			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Label = "Last Version Release Date",
				Description = "Uniquely identifies the device.",
				Comment = "This is suppose to persist the last version release date.",
				ValueData = NSData.FromString (_lastReleaseDate),
				Generic = NSData.FromString (LASTVERSIONRELEASEDATE_KEY),
				Service = APP_ID, // Account and Service should be unique for each keychain entry
				Account = LASTVERSIONRELEASEDATE_KEY, // Account and Service should be unique for each keychain entry
			};

			var statusCode = SecKeyChain.Add (keyChainRecord);

			if (statusCode != SecStatusCode.Success && statusCode != SecStatusCode.DuplicateItem) {
				Logger.log ("Error adding keychain record : " + statusCode.ToString ());
				return false;
			} else {
				//_deviceIdentifier = identifierForVendor;
				return true;
			}
		}

		// IdentifierForVendor
		public static bool CreateKeyChainEntryForOrderLincVersion() {				

			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Label = "OrderLincVersion",
				Description = "Identifies the current version of OrderLinc installed.",
				Comment = "This is used to store the current version of OrderLinc.",
				ValueData = NSData.FromString (OrderLincFullVersion()),
				Generic = NSData.FromString (VER_KEY),
				Service = APP_ID, // Account and Service should be unique for each keychain entry
				Account = VER_KEY, // Account and Service should be unique for each keychain entry
			};

			var statusCode = SecKeyChain.Add (keyChainRecord);

			if (statusCode != SecStatusCode.Success && statusCode != SecStatusCode.DuplicateItem) {
				Logger.log ("Error adding keychain record : " + statusCode.ToString ());
				return false;
			} else {
				return true;
			}
		}

		// IdentifierForVendor
		public static bool SaveKeyChainEntryForDeviceIdentifier(string identifierForVendor) {
			if (FindKeyChainEntryForDeviceIdentifier()) {
				DeleteKeyChainEntryForDeviceIdentifier ();
			}

			return CreateKeyChainEntryForDeviceIdentifier (identifierForVendor);
		}

		// Last Version Release Date
		public static bool SaveKeyChainEntryForLastVersionReleaseDate(string _lastReleaseDate) {
			if (FindKeyChainEntryForLastVersionReleaseDate() ) {
				DeleteKeyChainEntryForLastVersionReleaseDate ();
			}

			return CreateKeyChainEntryForLastVersionReleaseDate (_lastReleaseDate);
		}


		// OrderLincVersion
		public static bool SaveKeyChainEntryForOrderLincVersion() {
			if (FindKeyChainEntryForOrderLincVersion() ) {
				DeleteKeyChainEntryForOrderLincVersion ();
			}

			return CreateKeyChainEntryForOrderLincVersion ();
		}


		// IdentifierForVendor
		public static bool DeleteKeyChainEntryForDeviceIdentifier() {
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Generic = NSData.FromString(DEV_KEY)
			};

			var statusCode = SecKeyChain.Remove (keyChainRecord);

			if (statusCode != SecStatusCode.Success) {
				Logger.log ("Error deleting keychain entry: " + statusCode.ToString ());
				return false;
			} else {
				_username = null;
				_password = null;
				return true;
			}
		}


		// OrderLincVersion
		public static bool DeleteKeyChainEntryForLastVersionReleaseDate() {
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Generic = NSData.FromString(LASTVERSIONRELEASEDATE_KEY)
			};

			var statusCode = SecKeyChain.Remove (keyChainRecord);

			if (statusCode != SecStatusCode.Success) {
				Logger.log ("Error deleting keychain entry: " + statusCode.ToString ());
				return false;
			} else {
				return true;
			}
		}


		// OrderLincVersion
		public static bool DeleteKeyChainEntryForOrderLincVersion() {
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Generic = NSData.FromString(VER_KEY)
			};

			var statusCode = SecKeyChain.Remove (keyChainRecord);

			if (statusCode != SecStatusCode.Success) {
				Logger.log ("Error deleting keychain entry: " + statusCode.ToString ());
				return false;
			} else {
				return true;
			}
		}

		public static string UserName { 
			get {
				// attempt to retrieve keychain if username is not set
				if (_username == null || _username.Trim ().Length <= 0)
					FindKeyChainEntry();
				return _username;
			}
		}

		public static string Password {
			get {
				// attempt to retrieve keychain if password is not set
				if (_password == null || _password.Trim ().Length <= 0)
					FindKeyChainEntry();
				return _password;
			}
		}

		public static void Reset() {
			_username = null;
			_password = null;
		}

		public static bool FindKeyChainEntry() {
			var keyChainRecord = new SecRecord(SecKind.GenericPassword) {
				Generic = NSData.FromString(APP_ID),
				Service = APP_ID, // Account and Service should be unique for each keychain entry
			};
			SecStatusCode statusCode;
			var result = SecKeyChain.QueryAsRecord (keyChainRecord, out statusCode);
			if (statusCode == SecStatusCode.Success) {
				_username = result.Account;
				_password = result.ValueData.ToString();
				return true;
			} else {
				_username = null;
				_password = null;
				return false;
			}
		}

		public static bool CreateKeyChainEntry(string username, string password) {
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Label = "OrderLinc",
				Description = "OrderLinc Credentials",
				Comment = "You're not supposed to see this.",
				ValueData = NSData.FromString(password),
				Generic = NSData.FromString(APP_ID),
				Service = APP_ID, // Account and Service should be unique for each keychain entry
				Account = username, // Account and Service should be unique for each keychain entry

			};				

			var statusCode = SecKeyChain.Add (keyChainRecord);

			if (statusCode != SecStatusCode.Success && statusCode != SecStatusCode.DuplicateItem) {
				Logger.log ("Error adding keychain record : " + statusCode.ToString ());
				return false;
			} else {
				_username = username;
				_password = password;
				return true;
			}
		}

		public static bool DeleteKeyChainEntry() {
			var keyChainRecord = new SecRecord (SecKind.GenericPassword) {
				Generic = NSData.FromString(APP_ID)
			};

			var statusCode = SecKeyChain.Remove (keyChainRecord);

			if (statusCode != SecStatusCode.Success) {
				Logger.log ("Error deleting keychain entry: " + statusCode.ToString ());
				return false;
			} else {
				_username = null;
				_password = null;
				return true;
			}
		}

		public static bool SaveKeyChainEntry(string username, string password) {
			if (FindKeyChainEntry()) {
				DeleteKeyChainEntry ();
			}
			return CreateKeyChainEntry (username, password);
		}

		/// <summary>
		/// Returns an ID that is unique across all iOS devices.
		/// </summary>
		/// <returns>The devide ID.</returns>
		public static string DeviceID { 
			get {

				// TODO: Choose between the different methods below.

				// generate Unique ID using ASIdentifierManager
//				return ASIdentifierManager.SharedManager.AdvertisingIdentifier.AsString ();

				// get the UUID via UIDevice
				return	UIDevice.CurrentDevice.IdentifierForVendor.AsString ();

				// generage Unique ID using NSUUID.
				//			NSUuid nsuuid = new NSUuid ();
				//			return nsuuid.AsString ();
			}
		}



		public static string MinorVersion(){
			string _minorVersion = NSBundle.MainBundle.InfoDictionary ["CFBundleShortVersionString"].ToString ().Split ('.') [1].ToString ();
			return  _minorVersion;
		}

		public static string MajorVersion(){
			string _majorVersion = NSBundle.MainBundle.InfoDictionary ["CFBundleShortVersionString"].ToString ().Split ('.') [0].ToString ();
			return _majorVersion;
		}

		public static string OrderLincFullVersion(){
			return  NSBundle.MainBundle.InfoDictionary ["CFBundleShortVersionString"].ToString ();
		}
			

	}
}

