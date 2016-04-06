using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccBroker.RepositoryInterface;
using System.Data.Entity;


namespace AccBroker.Data
{

    public interface IDbContextFactory
    {
        AccountDBContext Get();
    }

    public class AccountDBContextFactory : IDbContextFactory
    {
        private AccountDBContext _context;

        public AccountDBContext Get()
        {
            if (_context == null) InitialiseContext();

            return _context;
        }

        private void InitialiseContext()
        {

            //When Using SQLDeploy, we want to update the database ourselves.
            //System.Data.Entity.Database.SetInitializer<MusicStoreDbContext>(null);

            //To Use CodeFirst and have it create the sample data ..  this initialiser will create the database and insert sample data.
            //System.Data.Entity.Database.SetInitializer<AccountDBContext>(new AccountDbInitializer());
            //once up and running, use the following
            //System.Data.Entity.Database.SetInitializer<MusicStoreDbContext>(null);

            //this is a sample of an alternative method
            //System.Data.Entity.Database.SetInitializer<MusicStoreEntities>(new CreateDatabaseIfNotExists<MusicStoreEntities>());

            _context = new AccountDBContext();
        }
    }

    public class AccountDBTestContextFactory : IDbContextFactory
    {
        private AccountDBContext _context;

        public AccountDBContext Get()
        {
            if (_context == null) InitialiseContext();

            return _context;
        }

        private void InitialiseContext()
        {

            //When Using SQLDeploy, we want to update the database ourselves.
            //System.Data.Entity.Database.SetInitializer<AccountDBContext>(null);

            //To Use CodeFirst and have it create the sample data ..  this initialiser will create the database and insert sample data.
            System.Data.Entity.Database.SetInitializer<AccountDBContext>(new AccountDbInitializer());
            //once up and running, use the following
            //System.Data.Entity.Database.SetInitializer<AccountDBContext>(null);

            //this is a sample of an alternative method
            //System.Data.Entity.Database.SetInitializer<AccountDBContext>(new DropCreateDatabaseAlways<AccountDBContext>());

            _context = new AccountDBContext();
        }
    }
}
