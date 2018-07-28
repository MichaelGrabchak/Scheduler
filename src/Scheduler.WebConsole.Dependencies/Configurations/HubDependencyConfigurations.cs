using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Engine;
using Scheduler.WebConsole.Hubs;
using Scheduler.WebConsole.Hubs.Activators;
using Scheduler.WebConsole.Hubs.Settings;

namespace Scheduler.WebConsole.Dependencies.Configurations
{
    public class HubDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            Container.RegisterType<SchedulerHub, SchedulerHub>();
            Container.RegisterType<IHubActivator, HubActivator>();
            Container.RegisterType<SchedulerSettings, SchedulerHubSettings>();

            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new HubActivator());
        }
    }
}
