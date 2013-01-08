About
=======================================

Testing libraries that talk to external HTTP APIs in .Net is difficult to
do. The testable classes have been designed to allow you to setup expected
Request and Responses ahead of time so you can exercise API logic in unit tests
rather than by manual test or talking to a live, networked resource.

* TestableHttpWebResponse - The Testable classes
* TestableHttpWebResponse.Sample - Sample API consumer
* TestableHttpWebResponse.Sample.Tests - Tests of the SampleService using the Testable classes 

This work is licensed under the Creative Commons Attribution 3.0 Unported License. To view a copy of this license, visit http://creativecommons.org/licenses/by/3.0/.

**Blog Post on the project:** [Testing the Not-So-Testable HttpWebResponse](http://blogs.lessthandot.com/index.php/DesktopDev/MSTech/CSharp/testing-httpwebresponse)

Other Sources
=======================================

Two other sources provided inspiration and/or code and are commented in their
respective places in the source:

* [How to mock HttpWebRequest When Unit Testing - salamandersoft.co.uk](http://blog.salamandersoft.co.uk/index.php/2009/10/how-to-mock-httpwebrequest-when-unit-testing/)
* [Mocking WebResopnses from a WebRequest - StackOverflow](http://stackoverflow.com/questions/87200/mocking-webresponses-from-a-webrequest)

