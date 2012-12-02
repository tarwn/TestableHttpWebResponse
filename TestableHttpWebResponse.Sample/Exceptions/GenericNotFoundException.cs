using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestableHttpWebResponse.Sample.Exceptions
{
	public class GenericNotFoundException : Exception
	{
		public GenericNotFoundException(string message, System.Net.WebException we) : base(message, we) { }
	}
}
