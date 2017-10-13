using System;
using System.Web.Mvc;

using Scheduler.Core.Engine;
using Scheduler.Domain.Services;
using Scheduler.Infrastructure.Services;
using Scheduler.Engine.Quartz;


using Microsoft.Practices.Unity;
using Unity.Mvc5;

namespace Scheduler.WebConsole
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();

            RegisterComponents(container);

            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void RegisterTypes()
        {
            RegisterComponents(GetConfiguredContainer());
        }

        private static void RegisterComponents(IUnityContainer container)
        {
            container.RegisterType<ISchedulerEngine, QuartzScheduler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(SchedulerSettings)));
            container.RegisterType<ISchedulerManagerService, SchedulerManagerService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}