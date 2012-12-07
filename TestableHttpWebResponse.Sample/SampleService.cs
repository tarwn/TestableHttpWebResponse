using Microsoft.Practices.TransientFaultHandling;
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
		private RetryPolicy _retryPolicy;
		private Uri _baseUri;

		public SampleService(Uri baseUri)
		{
			_baseUri = baseUri;
			_retryPolicy = new RetryPolicy<AwesomeRetryStrategy>(5, TimeSpan.FromMilliseconds(1));
		}

		private ServiceResponse SendRequest(WebRequest request)
		{
			return _retryPolicy.ExecuteAction<ServiceResponse>(() =>
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
			});
		}

		private ServiceResponse SendRequestAsyncTPL(WebRequest request)
		{
			return _retryPolicy.ExecuteAction<ServiceResponse>(() =>
			{
				var task = Task.Factory.FromAsync(
					request.BeginGetResponse,
					asyncResult => request.EndGetResponse(asyncResult),
					null)
				.ContinueWith((t) =>
					{
						var response = (HttpWebResponse)t.Result;
						var reader = new StreamReader(response.GetResponseStream());
						var message = reader.ReadToEnd();
						return new ServiceResponse() { IsSuccess = true, Message = message };
					});

				try
				{
					return task.Result;
				}
				catch (AggregateException ae)
				{
					throw MappedAggregateException(ae);
				}

			});
		}

		private ServiceResponse SendRequestAsyncAwait(WebRequest request)
		{
			Func<Task<ServiceResponse>> getResponse = async () =>
			{
				var response = await request.GetResponseAsync();
				var reader = new StreamReader(response.GetResponseStream());
				var message = reader.ReadToEnd();
				return new ServiceResponse() { IsSuccess = true, Message = message };
			};

			return _retryPolicy.ExecuteAction<ServiceResponse>(() =>
			{
				try
				{
					return getResponse().Result;
				}
				catch (AggregateException ae)
				{
					throw MappedAggregateException(ae);
				}

			});
		}

		private ServiceResponse SendData(WebRequest request, byte[] data)
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

		private ServiceResponse SendDataAsyncTPL(WebRequest request, byte[] data)
		{
			try
			{
				Task.Factory.FromAsync(
					request.BeginGetRequestStream,
					asyncResult => request.EndGetRequestStream(asyncResult),
					null)
				.ContinueWith((t) =>
				{
					var stream = (Stream)t.Result;
					stream.Write(data, 0, data.Length);
				})
				.Wait();
			}
			catch (AggregateException ae)
			{
				throw MappedAggregateException(ae);
			}

			return SendRequestAsyncTPL(request);
		}

		private async Task<ServiceResponse> SendDataAsyncAwait(WebRequest request, byte[] data)
		{
			try
			{
				var stream = await request.GetRequestStreamAsync();
				stream.Write(data, 0, data.Length);
			}
			catch (AggregateException ae)
			{
				throw MappedAggregateException(ae);
			}

			return SendRequestAsyncAwait(request);
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

		private Exception MappedAggregateException(AggregateException ae)
		{
			Exception nonAggregate;

			if (ae.InnerExceptions.Any(ie => !(ie is AggregateException)))
			{
				nonAggregate = ae.InnerExceptions.Where(ie => !(ie is AggregateException))
												 .First();
			}
			else if (ae.InnerExceptions.Any(ie => ie.InnerException != null && !(ie.InnerException is AggregateException)))
			{
				nonAggregate = ae.InnerExceptions.Where(ie => ie.InnerException != null && !(ie.InnerException is AggregateException))
												 .Select(ie => ie.InnerException)
												 .First();
			}
			else
				return ae;

			if (nonAggregate is WebException)
				return MappedException((WebException)nonAggregate);
			else
				return nonAggregate;
		}

		#region Sample API Call Implementations

		public ServiceResponse ListRemoteStuff(string operation)
		{
			var uri = new Uri(_baseUri, operation);
			var request = WebRequest.Create(uri);
			request.Headers.Add("version", "123-awesome");
			return SendRequest(request);
		}

		public ServiceResponse ListRemoteStuffAsyncTPL(string operation)
		{
			var uri = new Uri(_baseUri, operation);
			var request = WebRequest.Create(uri);
			request.Headers.Add("version", "123-awesome");
			return SendRequestAsyncTPL(request);
		}

		public ServiceResponse ListRemoteStuffAsyncAwait(string operation)
		{
			var uri = new Uri(_baseUri, operation);
			var request = WebRequest.Create(uri);
			request.Headers.Add("version", "123-awesome");
			return SendRequestAsyncAwait(request);
		}

		public ServiceResponse UploadSomething(string operation, byte[] data)
		{
			var uri = new Uri(_baseUri, operation);
			var request = WebRequest.Create(uri);
			request.Headers.Add("version", "123-awesome");
			return SendData(request, data);
		}

		public ServiceResponse UploadSomethingAsyncTPL(string operation, byte[] data)
		{
			var uri = new Uri(_baseUri, operation);
			var request = WebRequest.Create(uri);
			request.Headers.Add("version", "123-awesome");
			return SendDataAsyncTPL(request, data);
		}

		public ServiceResponse UploadSomethingAsyncAwait(string operation, byte[] data)
		{
			var uri = new Uri(_baseUri, operation);
			var request = WebRequest.Create(uri);
			request.Headers.Add("version", "123-awesome");
			try
			{
				return SendDataAsyncAwait(request, data).Result;
			}
			catch (AggregateException ae)
			{
				throw MappedAggregateException(ae);
			}
		}

		public object BonusMethod(string operation, string username, string password)
		{
			var uri = new Uri(_baseUri, operation);
			var request = WebRequest.Create(uri);
			request.Credentials = new NetworkCredential(username, password).GetCredential(uri, "Basic");
			request.PreAuthenticate = true;
			
			return SendRequest(request);
		}

		#endregion


	}


}
