using System.Web.Mvc;

using Scheduler.Core.Dependencies;
using Scheduler.Engine;
using Scheduler.Infrastructure.Dependencies;
using Scheduler.WebConsole.Settings;

using MvcDependencyResolver = Unity.Mvc5.UnityDependencyResolver;

namespace Scheduler.WebConsole
{
    public static class UnityConfig
    {
        public static void RegisterTypes()
        {
            SchedulerDependencies.Configure();

            Container.RegisterType<SchedulerSettings, SchedulerHubSettings>();

            DependencyResolver.SetResolver(new MvcDependencyResolver(Container.GetInstance()));
        }
    }
}