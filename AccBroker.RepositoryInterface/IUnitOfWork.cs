using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccBroker.RepositoryInterface
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        void Dispose();
    }
}
