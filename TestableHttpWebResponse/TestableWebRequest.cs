using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using TestableHttpWebResponse.ResponseSettings;

namespace TestableHttpWebResponse
{
	/// <summary>
	/// A fake HttpWebRequest that allows us to set one or more responses to receive in order and contains some basic
	/// settable properties we need in a WebRequest
	/// </summary>
	public class TestableWebRequest : WebRequest
	{
		private Uri _uri;
		private MemoryStream _requestStream;

		private Queue<BaseResponseSettings> _expectedResponses;

		public TestableWebRequest(Uri uri)
		{
			_uri = uri;
			_expectedResponses = new Queue<BaseResponseSettings>();
		}

		public void EnqueueResponse(HttpStatusCode httpStatusCode, string statusDescription, string responseContent, bool expectWebExceptionToBeThrown)
		{
			_expectedResponses.Enqueue(new HttpResponseSettings(httpStatusCode, statusDescription, responseContent, expectWebExceptionToBeThrown) 
			{
				ResponseStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(responseContent))
			});
		}

		public void EnqueueResponse(Exception exception)
		{
			_expectedResponses.Enqueue(new ExceptionResponseSettings(exception));
		}

		#region Overrides for WebRequest

		public override Stream GetRequestStream()
		{
			_requestStream = new MemoryStream();
			return _requestStream;
		}

		public override IAsyncResult BeginGetRequestStream(AsyncCallback callback, object state)
		{
			return base.BeginGetRequestStream(callback, state);
		}

		public override Stream EndGetRequestStream(IAsyncResult asyncResult)
		{
			return GetRequestStream();
		}

		/// <summary>
		/// Builds an HttpWebResponse using a deprecated constructor and the next queued ResponseSettings for this request
		/// </summary>
		/// <remarks>This is based on the sample code here: http://stackoverflow.com/questions/87200/mocking-webresponses-from-a-webrequest </remarks>
		public override WebResponse GetResponse()
		{
			var responseSettings = _expectedResponses.Dequeue();

			if (responseSettings is HttpResponseSettings)
			{
				var httpResponseSettings = (HttpResponseSettings)responseSettings;

				SerializationInfo si = new SerializationInfo(typeof(HttpWebResponse), new System.Runtime.Serialization.FormatterConverter());
				StreamingContext sc = new StreamingContext();
				WebHeaderCollection headers = new WebHeaderCollection();
				si.AddValue("m_HttpResponseHeaders", headers);
				si.AddValue("m_Uri", _uri);
				si.AddValue("m_Certificate", null);
				si.AddValue("m_Version", HttpVersion.Version11);
				si.AddValue("m_StatusCode", httpResponseSettings.StatusCode);
				si.AddValue("m_ContentLength", 0);
				si.AddValue("m_Verb", "GET");
				si.AddValue("m_StatusDescription", httpResponseSettings.StatusDescription);
				si.AddValue("m_MediaType", null);

				var webResponse = new TestableHttpWebResponse(si, sc, httpResponseSettings.ResponseStream);
				if (httpResponseSettings.ExpectException)
					throw new WebException("This request failed", new Exception(httpResponseSettings.StatusDescription), WebExceptionStatus.ProtocolError, webResponse);
				else
					return webResponse;
			}
			else if (responseSettings is ExceptionResponseSettings)
			{
				throw ((ExceptionResponseSettings)responseSettings).ExceptionToThrow;
			}
			else
			{
				throw new ArgumentException(String.Format("No logic to handle a ResponseSettings object of type '{0}'", responseSettings.GetType().Name));
			}
		}

		public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
		{
			var function = new Func<object>(() => { return null; });
			return function.BeginInvoke(callback, state);
		}

		public override WebResponse EndGetResponse(IAsyncResult asyncResult)
		{
			return GetResponse();
		}

		#endregion

		/// <summary>
		/// Returns the contents written to the Request stream
		/// </summary>
		public byte[] GetContent()
		{
			return _requestStream.ToArray();
		}

	}
}
