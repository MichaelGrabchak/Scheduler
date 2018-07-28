using System.Web.Mvc;
using System.Web.Routing;

using Scheduler.WebConsole.Dependencies;

namespace Scheduler.WebConsole
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // register dependencies
            var dependenciesManager = new WebAppDependencyManager();
            dependenciesManager.RegisterDependencies();
        }
    }
}
