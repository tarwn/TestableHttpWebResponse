using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.TransientFaultHandling;

namespace TestableHttpWebResponse.Sample
{
	public class AwesomeRetryStrategy : ITransientErrorDetectionStrategy
	{
		public bool IsTransient(Exception ex)
		{
			return ex is TimeoutException;
		}
	}
}
