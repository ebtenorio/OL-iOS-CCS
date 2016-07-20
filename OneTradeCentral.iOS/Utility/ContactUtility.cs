using System;
using AddressBook;

namespace OneTradeCentral.iOS
{
	public class ContactUtility
	{
		public ContactUtility ()
		{
		}

		public static string getEmailEntry(ABPerson contact) {
			string emailAddress = "";
			var emails = contact.GetEmails ();

			if (emails != null && emails.Count > 0) {
				foreach (var email in emails) {
					emailAddress = email.Value;
					if (email.Label.ToString().ToLower() == "work") 
						break;
				}
			}
			return emailAddress;
		}

		public static string getPhoneEntry(ABPerson contact) {
			string phoneNumber = "";
			var phoneNumbers = contact.GetPhones ();
			if (phoneNumbers != null && phoneNumbers.Count > 0) {
				foreach (var number in phoneNumbers) {
					phoneNumber = number.Value;
					if (number.Label.ToString().ToLower () == "mobile")
						break;
				}
			}
			return phoneNumber;
		}
	}
}

