using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestableHttpWebResponse
{
	/// <summary>
	/// Factory responsible for allowing us to setup requests for specific URIs in advance. Uses a singleton
	/// pattern to allow all tests to access the version that has been Registered with WebRequest
	/// </summary>
    public class TestableWebRequestCreateFactory : IWebRequestCreate
    {
		private Dictionary<string, TestableWebRequest> _preparedRequests;

		private TestableWebRequestCreateFactory()
		{
			_preparedRequests = new Dictionary<string, TestableWebRequest>();
		}

		public void AddRequest(string absoluteUri, TestableWebRequest request)
		{
			lock (_preparedRequests)
			{
				if (_preparedRequests.ContainsKey(absoluteUri))
					_preparedRequests.Remove(absoluteUri);
				_preparedRequests.Add(absoluteUri, request);
			}
		}

		#region Singleton

		private static TestableWebRequestCreateFactory _currentFactory = new TestableWebRequestCreateFactory();

		public static TestableWebRequestCreateFactory GetFactory()
		{
			return _currentFactory;
		}

		#endregion

		#region WebRequest Factory Implementation

		public WebRequest Create(Uri uri)
		{
			if (!_preparedRequests.ContainsKey(uri.AbsoluteUri))
				throw new ArgumentException(String.Format("The specified URI '{0}' does not have a request setup", uri.AbsoluteUri));

			return _preparedRequests[uri.AbsoluteUri];
		}

		#endregion
	}
}
