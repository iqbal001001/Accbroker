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
using AccBroker.WebAPI.Helper;
using System.Web.Http.Cors;
using System.Data.Entity.Validation;
using System.Web.Http.Description;

namespace AccBroker.WebAPI.Controllers
{
    [Authorize]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    //[RoutePrefix("")]
    public class CompanyController : ApiController
    {
        private ICompanyRepository _CompanyRepo;
        private IUnitOfWork _uow;

        public CompanyController(ICompanyRepository CompanyRepo, IUnitOfWork uow)
        {
            _CompanyRepo = CompanyRepo;
            _uow = uow;
        }

        // GET: api/Comapny
        [Route("api/Company", Name = "CompanyList")]
        [ResponseType(typeof(List<CompanyDTO>))]
        public IHttpActionResult Get(string sort = "ID", string fields = null,
            int page = 1, int pageSize = 5)
        {
            try
            {
                string userName;

                bool includeAddress = false;
                List<string> lstOfFields = new List<string>();

                if (fields != null)
                {
                    lstOfFields = fields.ToLower().Split(',').ToList();
                    includeAddress = lstOfFields.Any(f => f.Contains("address"));
                }

                var totalCount = _CompanyRepo.Get().Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var urlHelper = new UrlHelper(Request);
                var prevLink = page > 1 ? urlHelper.Link("CompanyList",
                    new
                    {
                        page = page - 1,
                        pageSize = pageSize,
                        fields = fields,
                        sort = sort
                    }) : "";
                var nextLink = page < totalPages ? urlHelper.Link("CompanyList",
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

                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers",
                 "X-Pagination");

                    HttpContext.Current.Response.Headers.Add("X-Pagination",
                        Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

                    if (HttpContext.Current != null && HttpContext.Current.User != null 
                        && HttpContext.Current.User.Identity.Name != null)
                    {
                        userName = HttpContext.Current.User.Identity.Name;
                    }
                }

                var companys = _CompanyRepo.Get();

                if (includeAddress)
                {
                    companys = companys.Include(c => c.Addresses);
                }

                var query = companys
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

        // GET: api/Comapny/5
        [Route("api/company/{id}")]
        [ResponseType(typeof(CompanyDTO))]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var company = _CompanyRepo.Get()
                    .Include(c => c.Addresses)
                    .Include(c => c.Contacts)
                    .FirstOrDefault<Company>(c => c.ID == id);
                if (company == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(company.ToDTO());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

        // GET: api/Comapny/5
        [Route("api/company/{code}/code")]
        public IHttpActionResult GetByCode(string code)
        {
            try
            {
                
                var company = _CompanyRepo.Get().FirstOrDefault<Company>(c => c.Code == code);
                if (company == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(company.ToDTO());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

        // GET: api/Comapny/5
        [HttpGet]
        [Route("api/company/{id}/{code}/codeAvailable")]
        public IHttpActionResult CodeAvailable(int? id, string code)
        {
            try
            {
                var company = _CompanyRepo.Get();

                if (id != null)
                {
                    company = company.Where(c => c.ID != id);
                }

                company.FirstOrDefault<Company>(c => c.Code == code);
                if (company == null)
                {
                    return Ok(new { CodeAvailable = false });
                }
                else
                {
                    return Ok(new { CodeAvailable = true });
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

        // POST: api/Comapny

        [Route("api/company")]
        [ResponseType(typeof(CompanyDTO))] // Api Documentation
        public IHttpActionResult Post([FromBody]CompanyDTO value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest();
                }

                string userName = "";
                if (HttpContext.Current != null && HttpContext.Current.User != null
                       && HttpContext.Current.User.Identity.Name != null)
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                var company = value.ToDomain();
                company.CreateUser = userName;
                company.ChangeUser = userName;
                company.Concurrency = Guid.NewGuid();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _CompanyRepo.Add(company);

                _uow.SaveChanges();

                if (company.ID > 0)
                {
                    return Created<CompanyDTO>(Request.RequestUri + "/" + company.ID, company.ToDTO());
                }

                return BadRequest();

            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        // PUT: api/Comapny/5
        [Route("api/company/{id}")]
        [ResponseType(typeof(CompanyDTO))]
        public IHttpActionResult Put(int id, [FromBody]CompanyDTO value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var originalCompany = _CompanyRepo.Get()
                .Include(c => c.Addresses)
                .Include(c => c.Contacts)
                .FirstOrDefault<Company>(c => c.ID == id);
            if (originalCompany == null)
            {
                return NotFound();
            }

            string userName = "";
            if (HttpContext.Current != null && HttpContext.Current.User != null
                   && HttpContext.Current.User.Identity.Name != null)
            {
                userName = HttpContext.Current.User.Identity.Name;
            }

            var company = value.ToDomain(originalCompany);
            company.ChangeUser = userName;
            company.Concurrency = Guid.NewGuid();

            _CompanyRepo.Update(company);
            try
            {
                _uow.SaveChanges();

                return Created<CompanyDTO>(Request.RequestUri + "/" + company.ID, company.ToDTO());
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
        [Route("api/company/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (_CompanyRepo.Get().First<Company>(c => c.ID == id) == null)
                {
                    return NotFound();
                }

                string userName = "";
                if (HttpContext.Current != null && HttpContext.Current.User != null
                       && HttpContext.Current.User.Identity.Name != null)
                {
                    userName = HttpContext.Current.User.Identity.Name;
                }

                _CompanyRepo.Delete(id);

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
