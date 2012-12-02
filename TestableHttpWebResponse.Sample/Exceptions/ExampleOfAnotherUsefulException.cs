using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TestableHttpWebResponse.Sample.Exceptions
{
	public class ExampleOfAnotherUsefulException : Exception
	{
		public ExampleOfAnotherUsefulException(string message, WebException we) : base(message, we) { }
	}
}
