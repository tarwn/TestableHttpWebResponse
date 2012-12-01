using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestableHttpWebRequest.Sample.Tests;

namespace TestableHttpWebResponse.Sample.Tests
{
	public class SampleService
	{
		private Uri _uri;

		public SampleService(Uri uri)
		{
			_uri = uri;
		}

		public ServiceResponse Execute(WebRequest request)
		{
			throw new NotImplementedException("not implemented");
		}

	}
}
