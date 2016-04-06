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
    public class PaymentController : ApiController
    {
        private IPaymentRepository _PaymentRepo;
        private IInvoiceRepository _InvoiceRepo; 
        private IUnitOfWork _uow;

        public PaymentController(IPaymentRepository PaymentRepo, IInvoiceRepository InvoiceRepo, IUnitOfWork uow)
        {
            _PaymentRepo = PaymentRepo;
            _InvoiceRepo = InvoiceRepo;
            _uow = uow;
        }

        // GET: api/Payment
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                var payments = _PaymentRepo.Get();
                var query = payments
                    .ToList()
                    .Select(p => p.ToDTO()).ToList();

                return Ok(query);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/payment", Name = "PaymentList")]
        public IHttpActionResult Get(string sort = "ID", string fields = null,
            int page = 1, int pageSize = 5, 
            string searchPaymentNo = null,
            int? searchCompanyId = null,
            int? searchClientId = null)
        {
            try
            {
                bool includePaymentItem = false;
                List<string> lstOfFields = new List<string>();

                if (fields != null)
                {
                    lstOfFields = fields.ToLower().Split(',').ToList();
                    includePaymentItem = lstOfFields.Any(f => f.Contains("paymentItem"));
                }
                var payments = _PaymentRepo.Get().Include(c => c.Client).Include(c => c.Company);

                if (includePaymentItem)
                {
                    payments = payments.Include(p => p.PaymentItems);
                }

                if (searchPaymentNo != null)
                {
                    payments = payments.Where(i => i.PaymentNo.Contains(searchPaymentNo));
                }

                if (searchCompanyId != null)
                {
                    payments = payments.Where(i => i.CompanyID == searchCompanyId);
                }

                if (searchClientId != null)
                {
                    payments = payments.Where(i => i.ClientID == searchClientId);
                }

                var totalCount = payments.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var urlHelper = new UrlHelper(Request);
                var prevLink = page > 1 ? urlHelper.Link("PaymentList",
                    new
                    {
                        page = page - 1,
                        pageSize = pageSize,
                        fields = fields,
                        sort = sort
                    }) : "";
                var nextLink = page < totalPages ? urlHelper.Link("PaymentList",
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


               

                var query = payments
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

        // GET: api/Payment/5
        [Route("api/payment/{id}")]
        [Route("api/payment/{id}/paymentItem")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var payment = _PaymentRepo.Get()
                    .Include(p => p.Client)
                    .Include(p => p.Company)
                    .Include(p => p.PaymentItems).First<Payment>(c => c.ID == id);
                if (payment == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(payment.ToDTO());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

        // POST: api/Comapny
        [Route("api/payment")]
       // [Route("api/payment/{id}")]
        public IHttpActionResult Post([FromBody]PaymentDTO value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest();
                }

                if (value.Amount != value.PaymentItems.Sum(it => it.Amount))
                {
                    return BadRequest("Invocie Amount Miss Match with total Invoice Item Amount");
                }

                foreach (var item in value.PaymentItems)
                {
                    var invoice = _InvoiceRepo.Get().FirstOrDefault<Invoice>(i => i.InvoiceNo == item.InvoiceNo);
                    if (invoice == null)
                    {
                        return BadRequest(item.InvoiceNo + "InvoiceNo not found");
                    }
                    item.InvoiceID = invoice.ID;
                }

                var payment = value.ToDomain();

                _PaymentRepo.Add(payment);

                _uow.SaveChanges();

                if (payment.ID > 0)
                {
                    return Created<PaymentDTO>(Request.RequestUri + "/" + payment.ID, payment.ToDTO());
                }

                return BadRequest();

            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        // PUT: api/Comapny/5
        [Route("api/payment/{id}")]
        public IHttpActionResult Put(int id, [FromBody]PaymentDTO value)
        {
            if (value == null)
            {
                return BadRequest();
            }

            if (value.Amount != value.PaymentItems.Sum(it => it.Amount))
            {
                return BadRequest("Invocie Amount Miss Match with total Invoice Item Amount");
            }

            var originalPayment = _PaymentRepo.Get().Include(p => p.PaymentItems).FirstOrDefault<Payment>(c => c.ID == id);
            if (originalPayment == null)
            {
                return NotFound();
            }

            foreach (var item in value.PaymentItems)
            {
                var invoice = _InvoiceRepo.Get().FirstOrDefault<Invoice>(i => i.InvoiceNo == item.InvoiceNo);
                if (invoice == null)
                {
                    return BadRequest(item.InvoiceNo + "InvoiceNo not found");
                }
                item.InvoiceID = invoice.ID;
            }

            var payment = value.ToDomain(originalPayment);
            payment.Concurrency = Guid.NewGuid();

            _PaymentRepo.Update(payment);
            try
            {
                _uow.SaveChanges();

                return Created<PaymentDTO>(Request.RequestUri + "/" + payment.ID, payment.ToDTO());
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
        [Route("api/payment/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (_PaymentRepo.Get().First<Payment>(c => c.ID == id) == null)
                {
                    return NotFound();
                }

                _PaymentRepo.Delete(id);

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
