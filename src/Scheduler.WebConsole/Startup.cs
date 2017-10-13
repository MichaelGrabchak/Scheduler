using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Scheduler.WebConsole.Startup))]
namespace Scheduler.WebConsole
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}