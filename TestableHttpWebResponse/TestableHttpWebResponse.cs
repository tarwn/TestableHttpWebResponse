using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TestableHttpWebResponse
{
	public class TestableHttpWebResponse : HttpWebResponse
	{
		private Stream _responseStream;

		public TestableHttpWebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext, Stream responseStream)
			#pragma warning disable 0618
			/* this base ctor is deprecated */
			: base(serializationInfo, streamingContext)
			#pragma warning restore 0618
		{
			_responseStream = responseStream;
		}

		public override System.IO.Stream GetResponseStream()
		{
			return _responseStream;
		}
	}
}
