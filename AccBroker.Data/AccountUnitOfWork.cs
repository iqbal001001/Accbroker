using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccBroker.Data;
using  AccBroker.RepositoryInterface;

namespace AccBroker.Data
{
    public class AccountUnitOfWork : IUnitOfWork
    {
        private IDbContextFactory _contextFactory;
        private AccountDBContext _context;

        public AccountUnitOfWork(IDbContextFactory contextFactory)
        {
            if (contextFactory == null)
            {
                throw new ArgumentNullException("contextFactory");
            }

            _contextFactory = contextFactory;
        }

        protected AccountDBContext Context
        {
            get { return _context ?? (_context = _contextFactory.Get()); }
        }

        public void SaveChanges()
        {
            //Context.Commit();
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }

}
