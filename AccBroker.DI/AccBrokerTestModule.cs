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
    public class AccBrokerTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            
            builder.RegisterModule<AccBrokerModule>();

            builder
                .RegisterType<AccountDBTestContextFactory>()
                .As<IDbContextFactory>()
                .InstancePerRequest();

            base.Load(builder);
        }
    }
}
