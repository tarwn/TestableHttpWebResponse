using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TestableHttpWebResponse.Sample.Exceptions
{
	public class SampleServiceOutageException : Exception
	{
		public SampleServiceOutageException(WebException we) : base("Sample service unavailable: " + we.Message, we) { }
	}
}
