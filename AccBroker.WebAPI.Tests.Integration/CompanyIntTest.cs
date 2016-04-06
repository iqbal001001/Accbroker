using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.SessionState;
using System.Security.Principal;
using Owin;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.Testing;
using System.IO;
using System.Threading;
using AccBroker.Domain;
using AccBroker.RepositoryInterface;
using AccBroker.WebAPI;

//reference : https://blogs.msdn.microsoft.com/webdev/2013/11/26/unit-testing-owin-applications-using-testserver/
//reference : http://www.strathweb.com/2012/06/asp-net-web-api-integration-testing-with-in-memory-hosting/
//reference : http://dotnetliberty.com/index.php/2015/12/17/asp-net-5-web-api-integration-testing/

namespace AccBroker.WebAPI.Tests.Integration
{
    [TestClass]
    public class CompanyIntTest
    {
        private IDisposable webApp;
        //private readonly TestServer _server;
        private string baseAddress = "http://localhost/";
        //private string baseAddress = "http://mbws:8123/";

        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //    try
        //    {
        //        //Start OWIN host
        //        webApp = WebApp.Start<MyStartup>(url: baseAddress);
        //        //webApp = TestServer.Create<MyStartup>();
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        //[TestCleanup]
        //public void MyTestCleanup()
        //{
        //    webApp.Dispose();
        //}

        [TestMethod]
        public void OwinAppTest()
        {
            using (var server = TestServer.Create<Startup>())
            {
                HttpResponseMessage response = server.HttpClient.GetAsync("/").Result;
                //await server.CreateRequest("/").AddHeader("header1", "headervalue1").GetAsync();
                //Execute necessary tests
                Assert.AreEqual<string>("AccBroker", response.Content.ReadAsStringAsync().Result);
            }
        }


        [TestMethod]
        public void Get_ShouldReturnAllCompanys()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                var client = server.HttpClient;
                //client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Accept
                //    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("api/Company").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<List<CompanyDTO>>().Result;
                Assert.AreEqual(3, result.Count());
            }
        }

        [TestMethod]
        public void Get_ShouldReturnAllCompanys_page_pageSize()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.CreateRequest("api/Company?page=1&pageSize=2").AddHeader("header1", "headervalue1").GetAsync().Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<List<CompanyDTO>>().Result;
                Assert.AreEqual(2, result.Count());

                HttpResponseMessage response2 = server.HttpClient.GetAsync("api/Company?page=2&pageSize=2").Result;

                Assert.IsTrue(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response2.StatusCode);

                var result2 = response2.Content.ReadAsAsync<List<CompanyDTO>>().Result;
                Assert.AreEqual(1, result2.Count());

                // TODO : HTTPContext test for Pagination Object
            }
        }

        [TestMethod]
        public void Get_ShouldReturnAllCompanys_sort()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.HttpClient.GetAsync("api/Company?sort=Name").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<List<CompanyDTO>>().Result;
                Assert.AreEqual("Company A ", result[0].Name.ToString());
                Assert.AreEqual("Company B ", result[1].Name.ToString());
                Assert.AreEqual("Company C ", result[2].Name.ToString());

                HttpResponseMessage response2 = server.HttpClient.GetAsync("api/Company?sort=-Name").Result;

                Assert.IsTrue(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response2.StatusCode);

                var result2 = response2.Content.ReadAsAsync<List<CompanyDTO>>().Result;
                Assert.AreEqual("Company C ", result2[0].Name.ToString());
                Assert.AreEqual("Company B ", result2[1].Name.ToString());
                Assert.AreEqual("Company A ", result2[2].Name.ToString());
            }
        }

        [TestMethod]
        public void Get_ShouldReturnAllCompanys_filter()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.HttpClient.GetAsync("api/Company?fields=ID,Name,ABN").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<List<CompanyDTO>>().Result;
                Assert.IsNotNull(result[0].Name);
                Assert.IsNotNull(result[0].ID);
                Assert.IsNotNull(result[0].ABN);
                Assert.IsNull(result[0].Addresses);
                Assert.IsNull(result[0].Concurrency);
                Assert.IsNull(result[0].ChangeDate);
                Assert.IsNull(result[0].CreateDate);      
            }
        }

        [TestMethod]
        public void Get_ShouldReturnAllCompanys_filter_address()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.HttpClient.GetAsync("api/Company?fields=ID,Name,ABN,addresses.AddressLine1,addresses.AddressLine2").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<List<CompanyDTO>>().Result;
                Assert.IsNotNull(result[0].Name);
                Assert.IsNotNull(result[0].ID);
                Assert.IsNotNull(result[0].ABN);
                Assert.IsNull(result[0].Concurrency);
                Assert.IsNull(result[0].ChangeDate);
                Assert.IsNull(result[0].CreateDate);

                var address = result[0].Addresses.ToList<AddressDTO>()[0];
                //Assert.IsNull(address.ID);
                Assert.IsNotNull(address.AddressLine1);
                Assert.IsNotNull(address.AddressLine2);
                Assert.IsNull(address.Suburb);
                Assert.IsNull(address.State);
                Assert.IsNull(address.PostCode);
                Assert.IsNull(address.Concurrency);
                Assert.IsNull(address.CreateDate);
                Assert.IsNull(address.ChangeDate);
                Assert.IsNull(address.CompanyID);

            }
        }


        [TestMethod]
        public void Get_Single_ShouldReturnCompanys()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.HttpClient.GetAsync("api/Company/1").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);

                var result = response.Content.ReadAsAsync<CompanyDTO>().Result;
                Assert.AreEqual(1, result.ID);
            }
        }

        [TestMethod]
        public void Post_ShouldInsertCompany()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                var data = new CompanyDTO { Name = "Company D", ABN = "209384" };
                HttpResponseMessage response = server.HttpClient.PostAsJsonAsync("api/Company", data).Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);

                var result = response.Content.ReadAsAsync<CompanyDTO>().Result;
                Assert.AreEqual(data.Name, result.Name);
            }
        }

        [TestMethod]
        public void Put_ShouldUpdateCompany()
        {
            using (var server = TestServer.Create<MyStartup>())
            {

                HttpResponseMessage response1 = server.HttpClient.GetAsync("api/Company/1").Result;

                Assert.IsTrue(response1.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.OK, response1.StatusCode);

                var data = response1.Content.ReadAsAsync<CompanyDTO>().Result;
                data.Name = "Company CC";

                HttpResponseMessage response = server.HttpClient.PutAsJsonAsync("api/Company/3", data).Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);

                var result = response.Content.ReadAsAsync<CompanyDTO>().Result;

                Assert.AreEqual(data.Name, result.Name);
            }
           
        }

        [TestMethod]
        public void Delete_ShouldRemoveCompany()
        {
            using (var server = TestServer.Create<MyStartup>())
            {
                HttpResponseMessage response = server.HttpClient.DeleteAsync("api/Company/3").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);

                var result = response.Content.ReadAsAsync<CompanyDTO>().Result;

                HttpResponseMessage response2 = server.HttpClient.GetAsync("api/Company/3").Result;

                Assert.IsFalse(response2.IsSuccessStatusCode);
                Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response2.StatusCode);

                var result2 = response2.Content.ReadAsAsync<CompanyDTO>().Result;

               // Assert.AreEqual(data.Name, result.Name);
            }
        }

        public HttpContext FakeHttpContext(Dictionary<string, object> sessionVariables, string path)
        {
            var httpRequest = new HttpRequest(string.Empty, path, string.Empty);
            var stringWriter = new StringWriter();
            var httpResponce = new HttpResponse(stringWriter);
            var httpContext = new HttpContext(httpRequest, httpResponce);
            httpContext.User = new GenericPrincipal(new GenericIdentity("username"), new string[0]);
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity("username"), new string[0]);
            var sessionContainer = new HttpSessionStateContainer(
              "id",
              new SessionStateItemCollection(),
              new HttpStaticObjectsCollection(),
              10,
              true,
              HttpCookieMode.AutoDetect,
              SessionStateMode.InProc,
              false);

            foreach (var var in sessionVariables)
            {
                sessionContainer.Add(var.Key, var.Value);
            }

            SessionStateUtility.AddHttpSessionStateToContext(httpContext, sessionContainer);
            return httpContext;
        }
    }
}
