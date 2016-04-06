using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Microsoft.Owin.Diagnostics;
using Owin;
using AccBroker.WebAPI;
using System.Web;
using System.Web.Http;
using Autofac;
using AccBroker.DI;
using Autofac.Integration.WebApi;
using Autofac.Integration.Owin;
using System.IO;


namespace AccBroker.WebAPI.Tests.Integration
{
   
    public class MyStartup
    {
        public void Configuration(IAppBuilder app)
        {

            //Startup.ConfigureAuth();
            app.UseErrorPage(); // See Microsoft.Owin.Diagnostics
            app.UseWelcomePage("/Welcome"); // See Microsoft.Owin.Diagnostics 
            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Hello world using OWIN TestServer");
            //});

            //Get your HttpConfiguration. In OWIN, you'll create one
            //rather than using GlobalConfiguration.
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            var builder = new ContainerBuilder();
            builder.RegisterModule<AccBrokerTestModule>();

            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);
            var container = builder.Build();
            //GlobalConfiguration.Configuration.DependencyResolver =
            // new AutofacWebApiDependencyResolver(container);

            //// OWIN WEB API SETUP:

            //// Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            //// and finally the standard Web API middleware.
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
           // app.UseWebMvc();
            

            //app.Use(async (context, next) =>
            //{
            //    var lifetimeScope = context.GetAutofacLifetimeScope();
            //    var httpContext = context; // new HttpContextWrapper(HttpContext.Current);
            //    HttpContext.Current = context;

            //    //if (lifetimeScope != null && httpContext != null)
            //    //    httpContext.Items[typeof(ILifetimeScope)] = lifetimeScope;

            //    await next();
            //});

            HttpContext.Current = new HttpContext(new HttpRequest("", "http://tempuri.org", ""),
          new HttpResponse(new StringWriter())
          );
           
        }
    }
}
