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
		public Uri BaseUri { get { return new Uri("test://mydomain.com/api/"); } }

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			WebRequest.RegisterPrefix("test", TestableWebRequestCreateFactory.GetFactory());
		}

		#region Synchronous Methods 

		[Test]
		public void ListRemoteStuff_ValidRequest_SetsVersionHeader()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuff(operation);

			Assert.AreEqual("123-awesome", expectedRequest.Headers["version"]);
		}

		[Test]
		public void ListRemoteStuff_ValidRequest_ReturnsSuccessfulResponse()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuff(operation);

			Assert.IsTrue(response.IsSuccess);
		}

		[Test]
		public void ListRemoteStuff_ValidRequest_ReturnsResponseContent()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuff(operation);

			Assert.AreEqual("Even More Success", response.Message);
		}

		[Test]
		[ExpectedException(typeof(DohickyNotFoundException))]
		public void ListRemoteStuff_404DohickeyNotFound_ThrowsDohickeyNotFoundException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "Dohicky not found", "I couldn't find your dohicky because I don't like you", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuff(operation);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(GenericNotFoundException))]
		public void ListRemoteStuff_404SomeOtherObjectNotFound_ThrowsGenericNotFoundException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "OtherObjectType not found", "I couldn't find yuor other object because the name was unimaginative", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuff(operation);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(ExampleOfAnotherUsefulException))]
		public void ListRemoteStuff_403NoneShallPass_ThrowsExampleOfAnotherUsefulException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.Forbidden, "None shall pass", "Somethign else amusing", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuff(operation);

			// expect exception
		}

		[Test]
		public void ListRemoteStuff_TimeoutOccurs_TruesASecondTime()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(HttpStatusCode.OK, "All Good", "Nothing to see, please move along", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuff(operation);

			Assert.AreEqual("Nothing to see, please move along", response.Message);
		}

		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void ListRemoteStuff_SixTimeoutsOccur_FinallyThrowsTheException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuff(operation);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(SampleServiceOutageException))]
		public void ListRemoteStuff_ServiceOutage_ThrowsSampleServiceOutage()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new WebException("I'm broke!", WebExceptionStatus.ConnectFailure));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuff(operation);

			// expect exception
		}

		[Test]
		public void UploadSomething_ValidRequest_SetsVersionHeader()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomething(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.AreEqual("123-awesome", expectedRequest.Headers["version"]);
		}

		[Test]
		public void UploadSomething_ValidRequest_ReturnsSuccessfulResponse()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomething(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.IsTrue(response.IsSuccess);
		}

		[Test]
		public void UploadSomething_ValidRequest_ReturnsResponseContent()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomething(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.AreEqual("Even More Success", response.Message);
		}

		[Test]
		public void UploadSomething_ValidRequest_UploadsMyAwesomeDataCorrectly()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomething(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			var uploadedData = ((TestableWebRequest)expectedRequest).GetContent();
			Assert.AreEqual("My awesome data payload!", System.Text.Encoding.UTF8.GetString(uploadedData));
		}

		[Test]
		[ExpectedException(typeof(DohickyNotFoundException))]
		public void UploadSomething_404DohickeyNotFound_ThrowsDohickeyNotFoundException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "Dohicky not found", "I couldn't find your dohicky because I don't like you", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomething(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(GenericNotFoundException))]
		public void UploadSomething_404SomeOtherObjectNotFound_ThrowsGenericNotFoundException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "OtherObjectType not found", "I couldn't find yuor other object because the name was unimaginative", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomething(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(ExampleOfAnotherUsefulException))]
		public void UploadSomething_403NoneShallPass_ThrowsExampleOfAnotherUsefulException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.Forbidden, "None shall pass", "Somethign else amusing", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomething(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		public void UploadSomething_TimeoutOccurs_ThrowsRawTimeoutException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(HttpStatusCode.OK, "All Good", "Nothing to see, please move along", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomething(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.AreEqual("Nothing to see, please move along", response.Message);
		}

		[Test]
		[ExpectedException(typeof(SampleServiceOutageException))]
		public void UploadSomething_ServiceOutage_ThrowsSampleServiceOutage()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new WebException("I'm broke!", WebExceptionStatus.ConnectFailure));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomething(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		#endregion

		#region Async TPL Methods

		[Test]
		public void ListRemoteStuffAsyncTPL_ValidRequest_SetsVersionHeader()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncTPL(operation);

			Assert.AreEqual("123-awesome", expectedRequest.Headers["version"]);
		}

		[Test]
		public void ListRemoteStuffAsyncTPL_ValidRequest_ReturnsSuccessfulResponse()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncTPL(operation);

			Assert.IsTrue(response.IsSuccess);
		}

		[Test]
		public void ListRemoteStuffAsyncTPL_ValidRequest_ReturnsResponseContent()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncTPL(operation);

			Assert.AreEqual("Even More Success", response.Message);
		}

		[Test]
		[ExpectedException(typeof(DohickyNotFoundException))]
		public void ListRemoteStuffAsyncTPL_404DohickeyNotFound_ThrowsDohickeyNotFoundException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "Dohicky not found", "I couldn't find your dohicky because I don't like you", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncTPL(operation);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(GenericNotFoundException))]
		public void ListRemoteStuffAsyncTPL_404SomeOtherObjectNotFound_ThrowsGenericNotFoundException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "OtherObjectType not found", "I couldn't find yuor other object because the name was unimaginative", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncTPL(operation);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(ExampleOfAnotherUsefulException))]
		public void ListRemoteStuffAsyncTPL_403NoneShallPass_ThrowsExampleOfAnotherUsefulException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.Forbidden, "None shall pass", "Somethign else amusing", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncTPL(operation);

			// expect exception
		}

		[Test]
		public void ListRemoteStuffAsyncTPL_TimeoutOccurs_TruesASecondTime()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(HttpStatusCode.OK, "All Good", "Nothing to see, please move along", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncTPL(operation);

			Assert.AreEqual("Nothing to see, please move along", response.Message);
		}

		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void ListRemoteStuffAsyncTPL_SixTimeoutsOccur_FinallyThrowsTheException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncTPL(operation);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(SampleServiceOutageException))]
		public void ListRemoteStuffAsyncTPL_ServiceOutage_ThrowsSampleServiceOutage()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new WebException("I'm broke!", WebExceptionStatus.ConnectFailure));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncTPL(operation);

			// expect exception
		}


		[Test]
		public void UploadSomethingAsyncTPL_ValidRequest_SetsVersionHeader()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncTPL(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.AreEqual("123-awesome", expectedRequest.Headers["version"]);
		}

		[Test]
		public void UploadSomethingAsyncTPL_ValidRequest_ReturnsSuccessfulResponse()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncTPL(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.IsTrue(response.IsSuccess);
		}

		[Test]
		public void UploadSomethingAsyncTPL_ValidRequest_ReturnsResponseContent()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncTPL(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.AreEqual("Even More Success", response.Message);
		}

		[Test]
		public void UploadSomethingAsyncTPL_ValidRequest_UploadsMyAwesomeDataCorrectly()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncTPL(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			var uploadedData = ((TestableWebRequest)expectedRequest).GetContent();
			Assert.AreEqual("My awesome data payload!", System.Text.Encoding.UTF8.GetString(uploadedData));
		}

		[Test]
		[ExpectedException(typeof(DohickyNotFoundException))]
		public void UploadSomethingAsyncTPL_404DohickeyNotFound_ThrowsDohickeyNotFoundException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "Dohicky not found", "I couldn't find your dohicky because I don't like you", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncTPL(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(GenericNotFoundException))]
		public void UploadSomethingAsyncTPL_404SomeOtherObjectNotFound_ThrowsGenericNotFoundException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "OtherObjectType not found", "I couldn't find yuor other object because the name was unimaginative", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncTPL(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(ExampleOfAnotherUsefulException))]
		public void UploadSomethingAsyncTPL_403NoneShallPass_ThrowsExampleOfAnotherUsefulException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.Forbidden, "None shall pass", "Somethign else amusing", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncTPL(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		public void UploadSomethingAsyncTPL_TimeoutOccurs_ThrowsRawTimeoutException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(HttpStatusCode.OK, "All Good", "Nothing to see, please move along", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncTPL(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.AreEqual("Nothing to see, please move along", response.Message);
		}

		[Test]
		[ExpectedException(typeof(SampleServiceOutageException))]
		public void UploadSomethingAsyncTPL_ServiceOutage_ThrowsSampleServiceOutage()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new WebException("I'm broke!", WebExceptionStatus.ConnectFailure));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncTPL(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		#endregion

		#region Async Await Methods

		[Test]
		public void ListRemoteStuffAsyncAwait_ValidRequest_SetsVersionHeader()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncAwait(operation);

			Assert.AreEqual("123-awesome", expectedRequest.Headers["version"]);
		}

		[Test]
		public void ListRemoteStuffAsyncAwait_ValidRequest_ReturnsSuccessfulResponse()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncAwait(operation);

			Assert.IsTrue(response.IsSuccess);
		}

		[Test]
		public void ListRemoteStuffAsyncAwait_ValidRequest_ReturnsResponseContent()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncAwait(operation);

			Assert.AreEqual("Even More Success", response.Message);
		}

		[Test]
		[ExpectedException(typeof(DohickyNotFoundException))]
		public void ListRemoteStuffAsyncAwait_404DohickeyNotFound_ThrowsDohickeyNotFoundException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "Dohicky not found", "I couldn't find your dohicky because I don't like you", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncAwait(operation);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(GenericNotFoundException))]
		public void ListRemoteStuffAsyncAwait_404SomeOtherObjectNotFound_ThrowsGenericNotFoundException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "OtherObjectType not found", "I couldn't find yuor other object because the name was unimaginative", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncAwait(operation);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(ExampleOfAnotherUsefulException))]
		public void ListRemoteStuffAsyncAwait_403NoneShallPass_ThrowsExampleOfAnotherUsefulException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.Forbidden, "None shall pass", "Somethign else amusing", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncAwait(operation);

			// expect exception
		}

		[Test]
		public void ListRemoteStuffAsyncAwait_TimeoutOccurs_TruesASecondTime()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(HttpStatusCode.OK, "All Good", "Nothing to see, please move along", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncAwait(operation);

			Assert.AreEqual("Nothing to see, please move along", response.Message);
		}

		[Test]
		[ExpectedException(typeof(TimeoutException))]
		public void ListRemoteStuffAsyncAwait_SixTimeoutsOccur_FinallyThrowsTheException()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(new TimeoutException("took too long, so sorry"));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncAwait(operation);

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(SampleServiceOutageException))]
		public void ListRemoteStuffAsyncAwait_ServiceOutage_ThrowsSampleServiceOutage()
		{
			var operation = "ListOfStuff";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new WebException("I'm broke!", WebExceptionStatus.ConnectFailure));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.ListRemoteStuffAsyncAwait(operation);

			// expect exception
		}


		[Test]
		public void UploadSomethingAsyncAwait_ValidRequest_SetsVersionHeader()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncAwait(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.AreEqual("123-awesome", expectedRequest.Headers["version"]);
		}

		[Test]
		public void UploadSomethingAsyncAwait_ValidRequest_ReturnsSuccessfulResponse()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncAwait(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.IsTrue(response.IsSuccess);
		}

		[Test]
		public void UploadSomethingAsyncAwait_ValidRequest_ReturnsResponseContent()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncAwait(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.AreEqual("Even More Success", response.Message);
		}

		[Test]
		public void UploadSomethingAsyncAwait_ValidRequest_UploadsMyAwesomeDataCorrectly()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.OK, "Success", "Even More Success", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncAwait(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			var uploadedData = ((TestableWebRequest)expectedRequest).GetContent();
			Assert.AreEqual("My awesome data payload!", System.Text.Encoding.UTF8.GetString(uploadedData));
		}

		[Test]
		[ExpectedException(typeof(DohickyNotFoundException))]
		public void UploadSomethingAsyncAwait_404DohickeyNotFound_ThrowsDohickeyNotFoundException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "Dohicky not found", "I couldn't find your dohicky because I don't like you", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncAwait(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(GenericNotFoundException))]
		public void UploadSomethingAsyncAwait_404SomeOtherObjectNotFound_ThrowsGenericNotFoundException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.NotFound, "OtherObjectType not found", "I couldn't find yuor other object because the name was unimaginative", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncAwait(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		[ExpectedException(typeof(ExampleOfAnotherUsefulException))]
		public void UploadSomethingAsyncAwait_403NoneShallPass_ThrowsExampleOfAnotherUsefulException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(HttpStatusCode.Forbidden, "None shall pass", "Somethign else amusing", true);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncAwait(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		[Test]
		public void UploadSomethingAsyncAwait_TimeoutOccurs_ThrowsRawTimeoutException()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new TimeoutException("took too long, so sorry"))
						   .EnqueueResponse(HttpStatusCode.OK, "All Good", "Nothing to see, please move along", false);
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncAwait(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			Assert.AreEqual("Nothing to see, please move along", response.Message);
		}

		[Test]
		[ExpectedException(typeof(SampleServiceOutageException))]
		public void UploadSomethingAsyncAwait_ServiceOutage_ThrowsSampleServiceOutage()
		{
			var operation = "UploadSomething";
			var expectedRequest = new TestableWebRequest(new Uri(BaseUri, operation));
			expectedRequest.EnqueueResponse(new WebException("I'm broke!", WebExceptionStatus.ConnectFailure));
			TestableWebRequestCreateFactory.GetFactory().AddRequest(expectedRequest);
			var service = new SampleService(BaseUri);

			var response = service.UploadSomethingAsyncAwait(operation, System.Text.Encoding.UTF8.GetBytes("My awesome data payload!"));

			// expect exception
		}

		#endregion
	}
}
