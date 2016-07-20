using System;

namespace OneTradeCentral.iOS
{
	public class Contact
	{
		public Contact ()
		{
		}

		/// <remarks/>
		public int ContactID { get; set; }

		/// <remarks/>
		public string Phone { get; set; }

		/// <remarks/>
		public string Fax { get; set; }

		/// <remarks/>
		public string Mobile { get; set; }

		/// <remarks/>
		public string Email { get; set; }

		/// <remarks/>
		public string LastName { get; set; }

		/// <remarks/>
		public string FirstName { get; set; }

		/// <remarks/>
		public long CreatedByUserID { get; set; }
	}
}

