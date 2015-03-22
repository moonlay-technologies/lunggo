using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Lunggo.Framework.Database;

namespace Lunggo.BackendWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InitDB();
        }
        private void InitDB()
        {
            const string connString = @"Data Source=""playdb.cloudapp.net, 63778"";Initial Catalog=Travorama;Persist Security Info=True;User ID=developer;Password=Standar1234";
            DbService.GetInstance().Init(connString);
        }

    }
}
