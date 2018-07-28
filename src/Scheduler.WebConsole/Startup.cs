using Owin;
using Microsoft.Owin;

using Scheduler.WebConsole.Hubs;

[assembly: OwinStartup(typeof(Scheduler.WebConsole.Startup))]
namespace Scheduler.WebConsole
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            SignalRConfig.SetupSinglaR(app);
        }
    }
}