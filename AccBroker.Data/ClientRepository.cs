using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccBroker.Domain;
using AccBroker.RepositoryInterface;

namespace AccBroker.Data
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(IDbContextFactory contextFactory) : base(contextFactory) { }
    }

}