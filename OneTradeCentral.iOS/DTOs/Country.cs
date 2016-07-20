using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace OneTradeCentral.DTOs
{
	public class Country
	{
		public string Code { get; set; }
		public string Name { get; set; }

		public Country (string code, string name)
		{
			Code = code;
			Name = name;
		}
	}
}

