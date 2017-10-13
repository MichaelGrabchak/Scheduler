using System.Web.Mvc;
using System.Web.Routing;

namespace Scheduler.WebConsole
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            UnityConfig.RegisterTypes();
            HubsConfig.RegisterHubs(UnityConfig.GetConfiguredContainer());
        }
    }
}
