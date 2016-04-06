using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AccBroker.Data;
using AccBroker.RepositoryInterface;

namespace AccBroker.DI
{
    public class AccBrokerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        { 
            builder
                .RegisterType<AccountDBContextFactory>()
                .As<IDbContextFactory>()
                .InstancePerRequest();

            builder
                .RegisterType<AccountUnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerRequest();

            builder
                .RegisterType<CompanyRepository>()
                .As<ICompanyRepository>()
                .InstancePerRequest();

            builder
               .RegisterType<ClientRepository>()
               .As<IClientRepository>()
               .InstancePerRequest();

            builder
              .RegisterType<AddressRepository>()
              .As<IAddressRepository>()
              .InstancePerRequest();

            builder
               .RegisterType<ContactRepository>()
               .As<IContactRepository>()
               .InstancePerRequest();

            builder
              .RegisterType<ProductRepository>()
              .As<IProductRepository>()
              .InstancePerRequest();

            builder
              .RegisterType<InvoiceRepository>()
              .As<IInvoiceRepository>()
              .InstancePerRequest();

            builder
              .RegisterType<PaymentRepository>()
              .As<IPaymentRepository>()
              .InstancePerRequest();

            base.Load(builder);
        }
    }
}
