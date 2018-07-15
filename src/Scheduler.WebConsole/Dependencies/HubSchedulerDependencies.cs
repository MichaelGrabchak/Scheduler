using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using Scheduler.Core.Dependencies;
using Scheduler.Engine;
using Scheduler.Infrastructure.Dependencies.Activators;
using Scheduler.Infrastructure.Hubs;
using Scheduler.WebConsole.Settings;

namespace Scheduler.WebConsole.Dependencies
{
    public static class HubSchedulerDependencies
    {
        public static void Configure()
        {
            Container.RegisterType<SchedulerHub, SchedulerHub>();
            Container.RegisterType<IHubActivator, UnityHubActivator>();
            Container.RegisterType<SchedulerSettings, SchedulerHubSettings>();

            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new UnityHubActivator(Container.GetInstance()));
        }
    }
}
