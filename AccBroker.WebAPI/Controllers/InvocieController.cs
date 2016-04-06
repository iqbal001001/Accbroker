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
    public class InvoiceController : ApiController
    {
        private IInvoiceRepository _InvoiceRepo;
        private IUnitOfWork _uow;

        public InvoiceController(IInvoiceRepository InvoiceRepo, IUnitOfWork uow)
        {
            _InvoiceRepo = InvoiceRepo;
            _uow = uow;
        }

        // GET: api/Invoice
        [Route("api/Invoice", Name = "InvoiceList")]
        public IHttpActionResult Get(string sort = "ID", string fields = null,
            int page = 1, int pageSize = 5, 
            string searchInvoiceNo = null, 
            int? searchCompanyId = null,
            int? searchClientId = null)
        {
            try
            {
                bool includeInvoiceItem = false;
                List<string> lstOfFields = new List<string>();

                if (fields != null)
                {
                    lstOfFields = fields.ToLower().Split(',').ToList();
                    includeInvoiceItem = lstOfFields.Any(f => f.Contains("invoiceItem"));
                }

                var invoices = _InvoiceRepo.Get().Include(c => c.Client).Include(c => c.Company);

                if (includeInvoiceItem)
                {
                    invoices = invoices.Include(i => i.InvoiceItems);
                }

                if (searchInvoiceNo != null)
                {
                    invoices = invoices.Where(i => i.InvoiceNo.Contains(searchInvoiceNo));
                }

                if (searchCompanyId != null)
                {
                    invoices = invoices.Where(i => i.CompanyID == searchCompanyId);
                }

                if (searchClientId != null)
                {
                    invoices = invoices.Where(i => i.ClientID == searchClientId);
                }

                var totalCount = invoices.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var urlHelper = new UrlHelper(Request);
                var prevLink = page > 1 ? urlHelper.Link("InvoiceList",
                    new
                    {
                        page = page - 1,
                        pageSize = pageSize,
                        fields = fields,
                        sort = sort
                    }) : "";
                var nextLink = page < totalPages ? urlHelper.Link("InvoiceList",
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
                    .Select(com => com.ToDTO().ToDataShapeObject(lstOfFields));

                return Ok(query);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
       

        // GET: api/Invoice/5
        [Route("api/invoice/{id}")]
        [Route("api/invoice/{id}/invoiceItems")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var invoice = _InvoiceRepo.Get()
                    .Include(i => i.Company)
                    .Include(i => i.Client)
                    .Include(i => i.PaymentItems.Select(pi => pi.Payment))
                    .Include(i => i.InvoiceItems.Select(ii => ii.ProductInvoiceItem))
                    .Include(i => i.InvoiceItems.Select(ii => ii.JsonInvoiceItem))
                    .First<Invoice>(c => c.ID == id).ToDTO();
                if (invoice == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(invoice);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

        // POST: api/Comapny
        [Route("api/invoice")]
        public IHttpActionResult Post([FromBody]InvoiceDTO value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest();
                }

                if (value.Amount != value.InvoiceItems.Sum(it => it.Amount))
                {
                    return BadRequest("Invocie Amount Miss Match with total Invoice Item Amount");
                }

                if (value.GST != value.InvoiceItems.Sum(it => it.GST))
                {
                    return BadRequest("Invocie GST Miss Match with total Invoice Item GST");
                }

                var invoice = value.ToDomain();

                _InvoiceRepo.Add(invoice);

                _uow.SaveChanges();

                if (invoice.ID > 0)
                {
                    return Created<InvoiceDTO>(Request.RequestUri + "/" + invoice.ID, invoice.ToDTO());
                }

                return BadRequest();

            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        // PUT: api/Comapny/5
        [Route("api/invoice/{id}")]
        public IHttpActionResult Put(int id, [FromBody]InvoiceDTO value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            if (value.Amount != value.InvoiceItems.Sum(it => it.Amount) )
            {
                return BadRequest("Invocie Amount Miss Match with total Invoice Item Amount");
            }

            if (value.GST != value.InvoiceItems.Sum(it => it.GST))
            {
                return BadRequest("Invocie GST Miss Match with total Invoice Item GST");
            }
            

            var originalInvoice = _InvoiceRepo.Get()
                 .Include(i => i.InvoiceItems.Select(ii => ii.ProductInvoiceItem))
                 .Include(i => i.InvoiceItems.Select(ii => ii.JsonInvoiceItem))
                .FirstOrDefault<Invoice>(c => c.ID == id);
            if (originalInvoice == null)
            {
                return NotFound();
            }

            var invoice = value.ToDomain(originalInvoice);
            invoice.Concurrency = Guid.NewGuid();

            _InvoiceRepo.Update(invoice);
            try
            {
                _uow.SaveChanges();

                return Created<InvoiceDTO>(Request.RequestUri + "/" + invoice.ID, invoice.ToDTO());
            }
            catch (DbEntityValidationException ex)
            {
                //StringBuilder sb = new StringBuilder();

                //foreach (var failure in ex.EntityValidationErrors)
                //{
                //    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                //    foreach (var error in failure.ValidationErrors)
                //    {
                //        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                //        sb.AppendLine();
                //    }
                //}
                //  throw new DbEntityValidationException(
                //    "Entity Validation Failed - errors follow:\n" +
                //    sb.ToString(), ex
                //); // Add the original exception as the innerException

                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        // DELETE: api/Comapny/5
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (_InvoiceRepo.Get().First<Invoice>(c => c.ID == id) == null)
                {
                    return NotFound();
                }

                _InvoiceRepo.Delete(id);

                _uow.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (DbEntityValidationException ex)
            {
                //StringBuilder sb = new StringBuilder();

                //foreach (var failure in ex.EntityValidationErrors)
                //{
                //    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                //    foreach (var error in failure.ValidationErrors)
                //    {
                //        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                //        sb.AppendLine();
                //    }
                //}

                //     throw new DbEntityValidationException(
                //"Entity Validation Failed - errors follow:\n" +
                //sb.ToString(), ex
                // ); // Add the original exception as the innerException
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }
    }
}
