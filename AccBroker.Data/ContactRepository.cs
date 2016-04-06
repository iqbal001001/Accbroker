using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccBroker.Domain;
using AccBroker.RepositoryInterface;

namespace AccBroker.Data
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(IDbContextFactory contextFactory) : base(contextFactory) { }
    }

}