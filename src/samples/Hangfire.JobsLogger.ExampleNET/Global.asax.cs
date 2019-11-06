using Hangfire.LiteDB;
using Hangfire.JobsLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Hangfire.JobsLogger.ExampleShared;

namespace Hangfire.JobsLogger.ExampleNET
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalConfiguration.Configuration.UseLiteDbStorage(TaskExample.ConnectiongStringLiteDb).UseJobsLogger();
        }
    }
}
