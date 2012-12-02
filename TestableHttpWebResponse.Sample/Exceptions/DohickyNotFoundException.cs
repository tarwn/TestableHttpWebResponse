using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestableHttpWebResponse.Sample.Exceptions
{
	public class DohickyNotFoundException : Exception
	{
		public DohickyNotFoundException(string message, System.Net.WebException we) : base(message, we){}
	}
}
