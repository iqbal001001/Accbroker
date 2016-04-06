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

namespace AccBroker.WebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductController : ApiController
    {
        private IProductRepository _ProductRepo;
        private IUnitOfWork _uow;

        public ProductController(IProductRepository ProductRepo, IUnitOfWork uow)
        {
            _ProductRepo = ProductRepo;
            _uow = uow;
        }

        // GET: api/Product
        //public IHttpActionResult Get()
        //{
        //    try
        //    {
        //        var products = _ProductRepo.Get();
        //        var query = products
        //            .ToList()
        //            .Select(p => p.ToDTO());

        //        return Ok(query);

        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}

        [Route("api/product", Name = "ProductList")]
        public IHttpActionResult Get(string sort = "ID", string fields = null,
            int page = 1, int pageSize = 5, string searchCode = null)
        {
            try
            {
                List<string> lstOfFields = new List<string>();

                if (fields != null)
                {
                    lstOfFields = fields.ToLower().Split(',').ToList();
                }

                var totalCount = _ProductRepo.Get().Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                var urlHelper = new UrlHelper(Request);
                var prevLink = page > 1 ? urlHelper.Link("ProductList",
                    new
                    {
                        page = page - 1,
                        pageSize = pageSize,
                        fields = fields,
                        sort = sort
                    }) : "";
                var nextLink = page < totalPages ? urlHelper.Link("ProductList",
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


                var products = _ProductRepo.Get();

                if (searchCode != null)
                {
                    products = products.Where(p => p.ProductCode.StartsWith(searchCode));
                }

                var query = products
                    .ApplySort(sort)
                    .Skip(pageSize * (page - 1))
                    .Take(pageSize)
                    .ToList()
                    .Select(prod => prod.ToDTO().ToDataShapeObject(lstOfFields));

                return Ok(query);

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/Product/5
        [Route("api/product/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var product = _ProductRepo.Get()
                    .First<Product>(c => c.ID == id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(product.ToDTO());
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

        }

        // POST: api/Comapny
        [Route("api/product")]
        public IHttpActionResult Post([FromBody]ProductDTO value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest();
                }

                var product = value.ToDomain();

                _ProductRepo.Add(product);

                _uow.SaveChanges();

                if (product.ID > 0)
                {
                    return Created<ProductDTO>(Request.RequestUri + "/" + product.ID, product.ToDTO());
                }

                return BadRequest();

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

        // PUT: api/Comapny/5
        [Route("api/product/{id}")]
        public IHttpActionResult Put(int id, [FromBody]ProductDTO value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            var originalProduct = _ProductRepo.Get().FirstOrDefault<Product>(c => c.ID == id);
            if (originalProduct == null)
            {
                return NotFound();
            }

            var product = value.ToDomain(originalProduct);
            product.Concurrency = Guid.NewGuid();

            _ProductRepo.Update(product);
            try
            {
                _uow.SaveChanges();

                return Created<ProductDTO>(Request.RequestUri + "/" + product.ID, product.ToDTO());
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
        [Route("api/product/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                if (_ProductRepo.Get().First<Product>(c => c.ID == id) == null)
                {
                    return NotFound();
                }

                _ProductRepo.Delete(id);

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
