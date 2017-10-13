using Scheduler.Infrastructure.Hubs;
using Scheduler.Infrastructure.Utils;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;
using Scheduler.Core.Engine;
using Scheduler.Infrastructure.Configuration;

namespace Scheduler.WebConsole
{
    public class HubsConfig
    {
        public static void RegisterHubs(IUnityContainer container)
        {
            container.RegisterType<SchedulerHub, SchedulerHub>();
            container.RegisterType<IHubActivator, UnityHubActivator>();
            container.RegisterType<SchedulerSettings, SchedulerHubSettings>();

            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new UnityHubActivator(container));
        }   
    }
}
