using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestableHttpWebResponse.Sample.Exceptions;


namespace TestableHttpWebResponse.Sample.Tests
{
	[TestFixture]
	public class SampleServiceTests
	{
		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			WebRequest.RegisterPrefix("test", TestableWebRequestCreateFactory.GetFactory());
		}

		[Test]
		public void SendRequest_ValidRequest_ReturnsSuccessfulResponse()
		{
			var expectedUri = "test://mydomain.com/SendRequest_ValidRequest_ReturnsSuccessfulResponse";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendRequest(request);

			Assert.IsTrue(response.IsSuccess);
		}

		[Test]
		public void SendRequest_ValidRequest_ReturnsResponseContent()
		{
			var expectedUri = "test://mydomain.com/SendRequest_ValidRequest_ReturnsResponseContent";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendRequest(request);

			Assert.AreEqual("Even More Success", response.Message);
		}

		[Test]
		[ExpectedException(typeof(DohickyNotFoundException))]
		public void SendRequest_404DohickeyNotFound_ThrowsDohickeyNotFoundException()
		{
			var expectedUri = "test://mydomain.com/SendRequest_404DohickeyNotFound_ThrowsDohickeyNotFoundException";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "Dohicky not found", "I couldn't find your dohicky because I don't like you", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendRequest(request);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(GenericNotFoundException))]
		public void SendRequest_404SomeOtherObjectNotFound_ThrowsGenericNotFoundException()
		{
			var expectedUri = "test://mydomain.com/SendRequest_404SomeOtherObjectNotFound_ThrowsGenericNotFoundException";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "OtherObjectType not found", "I couldn't find yuor other object because the name was unimaginative", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendRequest(request);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(ExampleOfAnotherUsefulException))]
		public void SendRequest_403NoneShallPass_ThrowsExampleOfAnotherUsefulException()
		{
			var expectedUri = "test://mydomain.com/SendRequest_403NoneShallPass_ThrowsExampleOfAnotherUsefulException";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(HttpStatusCode.Forbidden, "None shall pass", "Somethign else amusing", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendRequest(request);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void SendRequest_TimeoutOccurs_ThrowsRawTimeoutException()
		{
			var expectedUri = "test://mydomain.com/SendRequest_TimeoutOccurs_ThrowsRawTimeoutException";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(new TimeoutException("took too long, so sorry"));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendRequest(request);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(SampleServiceOutageException))]
		public void SendRequest_ServiceOutage_ThrowsSampleServiceOutage()
		{
			var expectedUri = "test://mydomain.com/SendRequest_ServiceOutage_ThrowsSampleServiceOutage";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(new WebException("I'm broke!", WebExceptionStatus.ConnectFailure));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendRequest(request);

			// expect exception
		}

		[Test]
		public void SendData_ValidRequest_ReturnsSuccessfulResponse()
		{
			var expectedUri = "test://mydomain.com/SendData_ValidRequest_ReturnsSuccessfulResponse";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendData(request, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.IsTrue(response.IsSuccess);
		}
		
		[Test]
		public void SendData_ValidRequest_ReturnsResponseContent()
		{
			var expectedUri = "test://mydomain.com/SendData_ValidRequest_ReturnsResponseContent";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendData(request, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.AreEqual("Even More Success", response.Message);
		}

		[Test]
		public void SendData_ValidRequest_UploadsMyAwesomeDataCorrectly()
		{
			var expectedUri = "test://mydomain.com/SendData_ValidRequest_ReturnsResponseContent";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendData(request, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			var uploadedData = ((TestableWebRequest)request).GetContent();
			Assert.AreEqual("My awesome data payload!", System.Text.Encoding.UTF8.GetString(uploadedData));
		}
		
		[Test]
		[ExpectedException(typeof(DohickyNotFoundException))]
		public void SendData_404DohickeyNotFound_ThrowsDohickeyNotFoundException()
		{
			var expectedUri = "test://mydomain.com/SendData_404DohickeyNotFound_ThrowsDohickeyNotFoundException";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "Dohicky not found", "I couldn't find your dohicky because I don't like you", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendData(request, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(GenericNotFoundException))]
		public void SendData_404SomeOtherObjectNotFound_ThrowsGenericNotFoundException()
		{
			var expectedUri = "test://mydomain.com/SendData_404SomeOtherObjectNotFound_ThrowsGenericNotFoundException";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "OtherObjectType not found", "I couldn't find yuor other object because the name was unimaginative", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendData(request, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(ExampleOfAnotherUsefulException))]
		public void SendData_403NoneShallPass_ThrowsExampleOfAnotherUsefulException()
		{
			var expectedUri = "test://mydomain.com/SendData_403NoneShallPass_ThrowsExampleOfAnotherUsefulException";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(HttpStatusCode.Forbidden, "None shall pass", "Somethign else amusing", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendData(request, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void SendData_TimeoutOccurs_ThrowsRawTimeoutException()
		{
			var expectedUri = "test://mydomain.com/SendData_TimeoutOccurs_ThrowsRawTimeoutException";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(new TimeoutException("took too long, so sorry"));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendData(request, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(SampleServiceOutageException))]
		public void SendData_ServiceOutage_ThrowsSampleServiceOutage()
		{
			var expectedUri = "test://mydomain.com/SendData_ServiceOutage_ThrowsSampleServiceOutage";
			var expectedRequest = new TestableWebRequest(new Uri(expectedUri));
			expectedRequest.EnqueueResponse(new WebException("I'm broke!", WebExceptionStatus.ConnectFailure));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedUri, expectedRequest);
			var service = new SampleService();
			var request = WebRequest.Create(expectedUri);

			var response = service.SendData(request, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}
	}
}
