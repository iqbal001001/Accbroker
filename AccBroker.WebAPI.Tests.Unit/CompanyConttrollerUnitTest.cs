using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http.Results;
using System.Web.Routing;
using System.IO;
using System.Web.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcContrib;
using MvcContrib.TestHelper;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using AccBroker.Domain;
using AccBroker.RepositoryInterface;
using AccBroker.WebAPI.Controllers;
using System.Web.Http;


namespace AccBroker.WebAPI.Tests.Unit
{
    [TestClass]
    public class CompanyConttrollerUnitTest
    {
        private IQueryable<Company> _data;
        private Mock<DbSet<Company>> _mockSet;
        private Mock<IUnitOfWork> _mockUOW;
        private Mock<ICompanyRepository> _mocCR;
        private Mock<IUnitOfWork> _mockUW;
        private Mock<HttpContextBase> _mockHttpContext;
        private Mock<HttpRequestBase> _mockRequest;
        private Mock<HttpResponseBase> _mockResponse;
        private NameValueCollection _FormKeys;


        public CompanyConttrollerUnitTest()
        {
            _data = new List<Company> 
            { 
                new Company {ID = 1,  Name = "Company A", ABN = ""}, 
                new Company {ID = 2,  Name = "Company B", ABN = ""},
                new Company {ID = 3,  Name = "Company C", ABN = ""},
            }.AsQueryable();


            _mockSet = new Mock<DbSet<Company>>();
            _mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(_data.Provider);
            _mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(_data.Expression);
            _mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(_data.ElementType);
            _mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());

            _mockUOW = new Mock<IUnitOfWork>();
            _mockUOW.Setup(t => t.SaveChanges()).Verifiable();

            _mocCR = new Mock<ICompanyRepository>();
            _mocCR.Setup(t => t.Get()).Returns(_mockSet.Object);
            _mocCR.Setup(t => t.Get(x => x.ID == It.IsAny<int>())).Returns(_mockSet.Object);
            _mocCR.Setup(t => t.Add(It.IsAny<Company>())).Callback((Company company) =>
            {
                var newListEmployee = new List<Company> { company };
                _data = _data.Concat(newListEmployee);
            }).Verifiable();
            _mocCR.Setup(t => t.Delete(It.IsAny<Company>())).Callback((Company company) =>
            {
                //var newListCompany = new List<Company> { company };
                var list = new List<Company>();
                list = _data.ToList();
                list.Remove(company);
                _data = list.AsQueryable();
            }).Verifiable();
            //_mocCR.Verify(mr => mr.Update(It.IsAny<Employee>()), Times.Once());
            //_mocCR.Setup(t => t.Delete(It.IsAny<int>()));
            //_mocCR.Setup(t => t.Delete(It.IsAny<Employee>()));
            _mocCR.Setup(t => t.Update(It.IsAny<Company>()));

            _mockUW = new Mock<IUnitOfWork>();
            _mockUW.Setup(t => t.SaveChanges());

            //reference : http://justinchmura.com/2014/06/26/mock-httpcontext/
            _mockHttpContext = new Mock<HttpContextBase>();
            _mockRequest = new Mock<HttpRequestBase>();
            _mockResponse = new Mock<HttpResponseBase>();
            _mockResponse.SetupGet(req => req.Headers).Returns(new NameValueCollection());
            _FormKeys = new NameValueCollection();
            _mockHttpContext.Setup(ctxt => ctxt.Request).Returns(_mockRequest.Object);
            _mockHttpContext.Setup(ctxt => ctxt.Response).Returns(_mockResponse.Object);
            // _mockHttpContext.Setup(ctxt => ctxt.ApplicationInstance).Returns(_mockHttpContext.Object);
            //_mockHttpContext.Setup(ctxt => ctxt.current).Returns(_mockResponse.Object);
            _mockRequest.Setup(r => r.Form).Returns(_FormKeys);


        }

        [TestInitialize]
        public void TestSetup()
        {
            HttpContext.Current = null;
            //new HttpContext(_mockRequest.Object.RequestContext,_mockResponse.Object.c; // _mockHttpContext.Object.ApplicationInstance.Context;
    //             new HttpContext(
    //new HttpRequest("", "http://tempuri.org", ""),
    //new HttpResponse(new StringWriter())
            //);

           // SetHttpContextWithSimulatedRequest("http://tempuri.org", "MyBlog");

        }

        public static void SetHttpContextWithSimulatedRequest(string host,string application)
        {
            string appVirtualDir = "/";
            string appPhysicalDir = @"c:\\projects\\SubtextSystem\\Subtext.Web\\";
            string page = application.Replace("/", string.Empty) + "/default.aspx";
            string query = string.Empty;
            TextWriter output = null;

            SimulatedHttpRequest workerRequest = new SimulatedHttpRequest(appVirtualDir, appPhysicalDir, page, query, output, host);
            HttpContext.Current = new HttpContext(workerRequest);

            Console.WriteLine("Request.FilePath: " + HttpContext.Current.Request.FilePath);
            Console.WriteLine("Request.Path: " + HttpContext.Current.Request.Path);

            Console.WriteLine("Request.RawUrl: " + HttpContext.Current.Request.RawUrl);

            Console.WriteLine("Request.Url: " + HttpContext.Current.Request.Url);

            Console.WriteLine("Request.ApplicationPath: " + HttpContext.Current.Request.ApplicationPath);

            Console.WriteLine("Request.PhysicalPath: " + HttpContext.Current.Request.PhysicalPath);
        }


        [TestMethod]
        public void Get_ShouldReturnAllCompanys()
        {
           
            var controller = new CompanyController(_mocCR.Object, _mockUOW.Object);
            //controller.ControllerContext = new System.Web.Http.Controllers.HttpControllerContext(); //new HttpConfiguration(), new RouteData(), new HttpRequestMessage());

            controller.Request = new HttpRequestMessage();
            var routeData = new RouteData();

            controller.Configuration = new HttpConfiguration();

            var result = controller.Get() as IHttpActionResult;
            Assert.IsTrue(result is OkNegotiatedContentResult<IEnumerable<Object>>);
            var okResult = result as OkNegotiatedContentResult<IEnumerable<Object>>;
            Assert.AreEqual(_data.Count(), okResult.Content.Count());

        }

        [TestMethod]
        public void Get_byID_ShouldReturnOneCompanys()
        {
            var controller = new CompanyController(_mocCR.Object, _mockUOW.Object);

            var result = controller.Get(1) as IHttpActionResult;
            Assert.IsTrue(result is OkNegotiatedContentResult<CompanyDTO>);

            var okResult = result as OkNegotiatedContentResult<CompanyDTO>;
            Assert.AreEqual(1, okResult.Content.ID);
        }

        [TestMethod]
        public void Post_ShouldInsertCompany()
        {

            var controller = new CompanyController(_mocCR.Object, _mockUOW.Object);
            var newCompany = new CompanyDTO { ID = 4, Name = "Company D"};
            controller.Request = new HttpRequestMessage();
            controller.Request.RequestUri =  new Uri("http://test");

            var result = controller.Post(newCompany) as IHttpActionResult;

            _mocCR.Verify(
                cr => cr.Add(It.Is<Company>(c => c.ID == 4)),
                    Times.Once);
            _mockUOW.Verify(
                uow => uow.SaveChanges(),
                Times.Once);

            Assert.IsTrue(result is CreatedNegotiatedContentResult<CompanyDTO>);

            var createdResult = result as CreatedNegotiatedContentResult<CompanyDTO>;
            Assert.AreEqual(newCompany.ID, createdResult.Content.ID);
            Assert.AreEqual(controller.Request.RequestUri.ToString() + "/" + newCompany.ID, createdResult.Location.ToString());

        }

        [TestMethod]
        public void Put_ShouldUpdateCompany()
        {

            var controller = new CompanyController(_mocCR.Object, _mockUOW.Object);
            var company = new CompanyDTO { ID = 1, Name = "Company AA" };
            controller.Request = new HttpRequestMessage();
            controller.Request.RequestUri = new Uri("http://test");

            var result = controller.Put(1,company) as IHttpActionResult;

            _mocCR.Verify(
                cr => cr.Update(It.Is<Company>(c => c.ID == 1)),
                    Times.Once);
            _mockUOW.Verify(
                uow => uow.SaveChanges(),
                Times.Once);

            Assert.IsTrue(result is CreatedNegotiatedContentResult<CompanyDTO>);

            var createdResult = result as CreatedNegotiatedContentResult<CompanyDTO>;
            Assert.AreEqual(company.ID, createdResult.Content.ID);
            Assert.AreEqual(company.Name, createdResult.Content.Name);
            Assert.AreEqual(controller.Request.RequestUri.ToString() + "/" + company.ID, createdResult.Location.ToString());

        }

        [TestMethod]
        public void Delete_ShouldRemoveCompany()
        {

            var controller = new CompanyController(_mocCR.Object, _mockUOW.Object);
            var company = _data.First<Company>(c=>c.ID == 1);

            var result = controller.Delete(1) as IHttpActionResult;

            _mocCR.Verify(
                cr => cr.Delete(It.Is<int>(c=>c.Equals(1))),
                    Times.Once);
            _mockUOW.Verify(
                uow => uow.SaveChanges(),
                Times.Once);

            Assert.IsTrue(result is StatusCodeResult);

            var statusResult = result as StatusCodeResult;
            Assert.AreEqual(System.Net.HttpStatusCode.NoContent , statusResult.StatusCode);

        }
    }

    public class SimulatedHttpRequest : SimpleWorkerRequest
    {
        string _host;

        /// <summary>
        /// Creates a new <see cref="SimulatedHttpRequest"/> instance.
        /// </summary>
        /// <param name="appVirtualDir">App virtual dir.</param>
        /// <param name="appPhysicalDir">App physical dir.</param>
        /// <param name="page">Page.</param>
        /// <param name="query">Query.</param>
        /// <param name="output">Output.</param>
        /// <param name="host">Host.</param>
        public SimulatedHttpRequest(string appVirtualDir, string
    appPhysicalDir, string page, string query, TextWriter output, string
    host)
            : base(appVirtualDir, appPhysicalDir, page, query, output)
        {
            if (host == null || host.Length == 0)
                throw new ArgumentNullException("host", "Host cannot be null nor empty.");
            _host = host;
        }

        /// <summary>
        /// Gets the name of the server.
        /// </summary>
        /// <returns></returns>
        public override string GetServerName()
        {
            return _host;
        }

        /// <summary>
        /// Maps the path to a filesystem path.
        /// </summary>
        /// <param name="virtualPath">Virtual path.</param>
        /// <returns></returns>
        public override string MapPath(string virtualPath)
        {
            return Path.Combine(this.GetAppPath(), virtualPath);
        }
    }
}
