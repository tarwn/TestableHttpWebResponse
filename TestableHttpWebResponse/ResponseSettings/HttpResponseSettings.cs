using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace TestableHttpWebResponse.ResponseSettings
{
	public class HttpResponseSettings : BaseResponseSettings
	{
		public HttpStatusCode StatusCode { get; set; }
		public string StatusDescription { get; set; }
		public string ResponseContent { get; set; }

		public HttpResponseSettings(HttpStatusCode httpStatusCode, string statusDescription, string responseContent, bool expectWebExceptionToBeThrown)
		{
			StatusCode = httpStatusCode;
			StatusDescription = statusDescription;
			ResponseContent = responseContent;
			ExpectException = expectWebExceptionToBeThrown;
		}


		public System.IO.Stream ResponseStream { get; set; }
	}
}
