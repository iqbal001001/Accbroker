using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using System.Reflection;
using Autofac.Integration.WebApi;
using AccBroker.DI;
using AccBroker.RepositoryInterface;
using System.Configuration;
using System.Data.Entity.Migrations;

namespace AccBroker.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AccBrokerModule>();
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver =
             new AutofacWebApiDependencyResolver(container);

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //if (bool.Parse(ConfigurationManager.AppSettings["MigrateDatabaseToLatestVersion"]))
            //{
            //    var configuration = new IDbContextFactory.Migrations.Configuration();
            //    var migrator = new DbMigrator(configuration);
            //    migrator.Update();
            //}
        }
    }
}
