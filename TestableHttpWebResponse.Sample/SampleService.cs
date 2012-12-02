using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestableHttpWebResponse.Sample.Exceptions;

namespace TestableHttpWebResponse.Sample
{
	public class SampleService
	{
		public SampleService()
		{ }

		public ServiceResponse SendRequest(WebRequest request)
		{
			try
			{
				var response = (HttpWebResponse)request.GetResponse();
				var reader = new StreamReader(response.GetResponseStream());
				var message = reader.ReadToEnd();
				return new ServiceResponse() { IsSuccess = true, Message = message };
			}
			catch (WebException we)
			{
				throw MappedException(we);
			}
		}

		public ServiceResponse SendData(WebRequest request, byte[] data)
		{
			try
			{
				var stream = request.GetRequestStream();
				stream.Write(data, 0, data.Length);

				return SendRequest(request);
			}
			catch (WebException we)
			{
				throw MappedException(we);
			}
		}

		private Exception MappedException(WebException we)
		{
			// map to custom exceptions
			if (we.Status == WebExceptionStatus.ProtocolError)
			{
				var reader = new StreamReader(we.Response.GetResponseStream());
				var message = reader.ReadToEnd();
				var httpResponse = (HttpWebResponse)we.Response;
				switch (httpResponse.StatusCode)
				{
					case HttpStatusCode.NotFound:
						if (httpResponse.StatusDescription.Contains("Dohicky"))
							return new DohickyNotFoundException(message, we);
						else
							return new GenericNotFoundException(message, we);
					default:
						return new ExampleOfAnotherUsefulException(message, we);
				}
			}
			else
				return new SampleServiceOutageException(we);
		}
	}


}
