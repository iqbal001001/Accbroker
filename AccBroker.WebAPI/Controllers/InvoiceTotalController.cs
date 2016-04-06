using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Data.Entity;
using AccBroker.RepositoryInterface;
using AccBroker.Domain;
using AccBroker.WebAPI;
using AccBroker.WebAPI.Helper;
using System.Web.Http.Cors;
using System.Data.Entity.Validation;

namespace AccBroker.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class InvoiceTotalController : ApiController
    {
        private IInvoiceRepository _InvoiceRepo;
        private IUnitOfWork _uow;

        public InvoiceTotalController(IInvoiceRepository InvoiceRepo, IUnitOfWork uow)
        {
            _InvoiceRepo = InvoiceRepo;
            _uow = uow;
        }

        // GET: api/InvoiceTotal
        [Route("api/InvoiceTotal", Name = "InvoiceTotalList")]
        public IHttpActionResult Get(string sort = "ID", string fields = null,
            int page = 1, int pageSize = 5,
            int? companyID = null, int? clientID = null,
            DateTime? startInvoiceDate = null, DateTime? endInvoiceDate = null,
            DateTime? startDueDate = null, DateTime? endDueDate = null,
            string searchInvoiceNo = null, bool? Paid = null)
        {
            bool includeInvoiceItem = false;
            List<string> lstOfFields = new List<string>();

            if (fields != null)
            {
                lstOfFields = fields.ToLower().Split(',').ToList();
                includeInvoiceItem = lstOfFields.Any(f => f.Contains("invoiceItem"));
            }

            var invoices = _InvoiceRepo.Get();

            invoices = invoices.Include(i => i.PaymentItems);
            invoices = invoices.Include(i => i.Client);

            if (companyID != null)
                invoices = invoices.Where(i => i.CompanyID == companyID);

            if (clientID != null)
                invoices = invoices.Where(i => i.ClientID == clientID);

            if (startInvoiceDate != null && endInvoiceDate != null)
                invoices = invoices.Where(i => i.DueDate >= startInvoiceDate && i.DueDate <= endInvoiceDate);

            if (startDueDate != null && endDueDate != null)
                invoices = invoices.Where(i => i.DueDate >= startDueDate && i.DueDate <= endDueDate);

            if (Paid != null && Paid == false)
                invoices = invoices.Where(i => i.PaymentItems.Sum(p => p.Amount) < (i.Amount + i.GST));

            if (Paid != null && Paid == true)
                invoices = invoices.Where(i => i.PaymentItems.Sum(p => p.Amount) == (i.Amount + i.GST));

            var totalCount = invoices.Count();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var urlHelper = new UrlHelper(Request);
            var prevLink = page > 1 ? urlHelper.Link("InvoiceTotalList",
                new
                {
                    page = page - 1,
                    pageSize = pageSize,
                    fields = fields,
                    sort = sort
                }) : "";
            var nextLink = page < totalPages ? urlHelper.Link("InvoiceTotalList",
                new
                {
                    page = page + 1,
                    pageSize = pageSize,
                    fields = fields,
                    sort = sort
                }) : "";

            var paginationHeader = new
            {
                currentPage = page,
                pageSize = pageSize,
                totalCount = totalCount,
                totalPages = totalPages,
                previousPageLink = prevLink,
                nextPageLink = nextLink
            };

            HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers",
         "X-Pagination");

            HttpContext.Current.Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

            var query = invoices
           .ApplySort(sort)
           .Skip(pageSize * (page - 1))
           .Take(pageSize)
           .ToList()
           .Select(com => com.ToTotalDTO());

            return Ok(query);
        }

        // GET: api/InvoiceTotal/DueDate
        [Route("api/InvoiceTotal/DueDate")]
        public IHttpActionResult GetDueDate(DateTime startDueDate, DateTime endDueDate, bool? Paid = null)
        {

            var invoices = _InvoiceRepo.Get();

            invoices = invoices.Include(i => i.PaymentItems);

            invoices = invoices.Where(i => i.DueDate > startDueDate && i.DueDate < endDueDate);

            if (Paid != null && Paid == false)
                invoices = invoices.Where(i =>  i.PaymentItems.Count == 0 || i.PaymentItems.Sum(p => p.Amount) < (i.Amount + i.GST));

            if (Paid != null && Paid == true)
                invoices = invoices.Where(i => i.PaymentItems.Sum(p => p.Amount) == (i.Amount + i.GST));

            var query = invoices
                .ToList()
                .Sum(i => (i.Amount + i.GST) - i.PaymentItems.Sum(p => p.Amount)) ;
                //.Select( i => new  {
                //    invoiceAmount = (i.Amount + i.GST), 
                //    paymentAmount = i.PaymentItems.Sum(p => p.Amount)});

            return Ok(query);
        }

        // GET: api/InvoiceTotal/InvoiceDate
        [Route("api/InvoiceTotal/InvoiceDate")]
        public IHttpActionResult GetInvocieDate(DateTime startInvoiceDate, DateTime endInvoiceDate, bool? Paid = null)
        {
            var invoices = _InvoiceRepo.Get();

            invoices = invoices.Include(i => i.PaymentItems);

            invoices = invoices.Where(i => i.DueDate >= startInvoiceDate && i.DueDate <= endInvoiceDate);

            if (Paid != null && Paid == false)
                invoices = invoices.Where(i => i.PaymentItems.Sum(p => p.Amount) < (i.Amount + i.GST));

            if (Paid != null && Paid == true)
                invoices = invoices.Where(i => i.PaymentItems.Sum(p => p.Amount) == (i.Amount + i.GST));

            var query = invoices
                .ToList()
                .Select(i => i.Amount + i.GST);

            return Ok(query);
        }

        // GET: api/InvoiceTotal/Company
        [Route("api/InvoiceTotal/Company")]
        public IHttpActionResult GetCompany(int companyId, bool? Paid = null)
        {
            var invoices = _InvoiceRepo.Get();

            invoices = invoices.Include(i => i.PaymentItems);

            invoices = invoices.Where(i => i.CompanyID == companyId);

            if (Paid != null && Paid == false)
                invoices = invoices.Where(i => i.PaymentItems.Sum(p => p.Amount) < (i.Amount + i.GST));

            if (Paid != null && Paid == true)
                invoices = invoices.Where(i => i.PaymentItems.Sum(p => p.Amount) == (i.Amount + i.GST));

            var query = invoices
                .ToList()
                .Select(i => i.Amount + i.GST);

            return Ok(query);
        }

        // GET: api/InvoiceTotal/Client
        [Route("api/InvoiceTotal/Client")]
        public IHttpActionResult GetClient(int clientId, bool? Paid = null)
        {
            var invoices = _InvoiceRepo.Get();

            invoices = invoices.Include(i => i.PaymentItems);

            invoices = invoices.Where(i => i.ClientID == clientId);

            if (Paid != null && Paid == false)
                invoices = invoices.Where(i => i.PaymentItems.Sum(p => p.Amount) < (i.Amount + i.GST));

            if (Paid != null && Paid == true)
                invoices = invoices.Where(i => i.PaymentItems.Sum(p => p.Amount) == (i.Amount + i.GST));

            var query = invoices
                .ToList()
                .Select(i => i.Amount + i.GST);

            return Ok(query);
        }


    }
}
