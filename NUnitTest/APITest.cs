using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using NUnit.Framework;

namespace NUnitTest
{
    [TestFixture]
    public class APITest
    {
        private HttpClient client;
        private HttpResponseMessage response;
        private string _serviceBaseUri = "https://localhost:44328/";

        [SetUp]
        public void SetUP()
        {
            client = new HttpClient();

            client.BaseAddress = new Uri(_serviceBaseUri);
            response = client.GetAsync("api").Result;
        }

        [Test]
        public void Test_API_Satus()
        {
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void Test_API_Response_IsJson()
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
        }

    }
}