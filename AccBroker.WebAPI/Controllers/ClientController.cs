using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class ClientController : ApiController
    {
        private IClientRepository _ClientRepo;
        private IUnitOfWork _uow;

        public ClientController(IClientRepository ClientRepo, IUnitOfWork uow)
        {
            _ClientRepo = ClientRepo;
            _uow = uow;
        }

        // GET: api/Client
        public IHttpActionResult Get()
        {
            try
            {
                var clients = _ClientRepo.Get();
                var query = clients
                    .ToList()
                    .Select(cl => cl.ToDTO()).ToList();

                return Ok(query);

            }
             catch (Exception ex)
             {
                 return InternalServerError(ex);
             }
        }

        // GET: api/Client/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                var client = _ClientRepo.Get()
                    .Include(c => c.Addresses)
                    .Include(c => c.Contacts)
                    .First<Client>(c => c.ID == id).ToDTO();
                if (client == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(client);
                }
            }
            catch(Exception ex)
            {
                return InternalServerError();
            }
           
        }

        // POST: api/Comapny
        public IHttpActionResult Post([FromBody]ClientDTO value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest();
                }

                    var client = value.ToDomain();

                    _ClientRepo.Add(client);

                    _uow.SaveChanges();

                    if (client.ID > 0)
                    {
                        return Created<ClientDTO>(Request.RequestUri + "/" + client.ID, client.ToDTO());
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
            catch(Exception ex)
            {
               return InternalServerError();
            }
        }

        // PUT: api/Comapny/5
        public IHttpActionResult Put(int id, [FromBody]ClientDTO value)
        {  
                if (value == null)
                {
                    return BadRequest();
                }
             var originalClient = _ClientRepo.Get()
                    .Include(c => c.Addresses)
                    .Include(c => c.Contacts)
                    .FirstOrDefault<Client>(c => c.ID == id);

             if (originalClient == null)
                {
                    return NotFound();
                }

                var client = value.ToDomain(originalClient);
                client.Concurrency = Guid.NewGuid();

                _ClientRepo.Update(client);
 try
            {
                _uow.SaveChanges();

                return Created<ClientDTO>(Request.RequestUri + "/" + client.ID, client.ToDTO());
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
                if (_ClientRepo.Get().First<Client>(c => c.ID == id) == null)
                {
                    return NotFound();
                }

                _ClientRepo.Delete(id);

                _uow.SaveChanges();

                return  StatusCode(HttpStatusCode.NoContent);
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
