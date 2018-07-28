using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Engine;
using Scheduler.WebConsole.Hubs;
using Scheduler.WebConsole.Utilities.Activators;
using Scheduler.WebConsole.Utilities.Settings;

namespace Scheduler.WebConsole.Utilities.Dependencies
{
    public class HubDependencyConfigurations : DependencyConfiguration
    {
        public override void Configure()
        {
            Container.RegisterType<SchedulerHub, SchedulerHub>();
            Container.RegisterType<IHubActivator, HubActivator>();
            Container.RegisterType<SchedulerSettings, SchedulerHubSettings>();

            GlobalHost.DependencyResolver.Register(typeof(IHubActivator), () => new HubActivator());
        }
    }
}
