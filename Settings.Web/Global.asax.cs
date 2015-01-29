using Settings.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Settings.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            BootStrapper.BootStrap();
            DependencyResolver.SetResolver(new NinjectDependencyResolver { Kernel = BootStrapper.NinjectContainer });
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver4API(BootStrapper.NinjectContainer);

            ViewEngines.Engines.Clear();   
            ViewEngines.Engines.Add(new RazorViewEngine());  

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            MvcHandler.DisableMvcResponseHeader = true;

            (BootStrapper.NinjectContainer.GetService(typeof(Settings.Model.ISqlService)) as Settings.Model.ISqlService).InitDatabase();
        }

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpContext.Current.Response.Headers.Remove("Server");
        }
    }
}
