using System;
using System.Web.Mvc;

using Scheduler.Core;
using Scheduler.Core.Engine;
using Scheduler.Core.Logging;
using Scheduler.Core.Configurations;
using Scheduler.Engine.Quartz;
using Scheduler.Domain.Services;
using Scheduler.Infrastructure.Services;
using Scheduler.Logging.NLog;
using Scheduler.Domain.Data;
using Scheduler.Infrastructure.Data;
using Scheduler.Core.Context;

using Microsoft.Practices.Unity;
using Unity.Mvc5;
using Scheduler.Domain.Data.Services;
using Scheduler.Infrastructure.Data.Services;

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
            RegisterBasic(container);
            RegisterServices(container);
            RegisterDataServices(container);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalUnity.Container = container;
        }

        private static void RegisterBasic(IUnityContainer container)
        {
            container.RegisterType<IDbContext, DbContext>();
            container.RegisterType<ISchedulerContext, SchedulerContext>();
            container.RegisterType<ISchedulerLogger, NLogLogger>(new InjectionConstructor(Constants.System.DefaultSchedulerLoggerName));
            container.RegisterType<ISchedulerEngine, QuartzScheduler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(SchedulerSettings)));
        }

        private static void RegisterServices(IUnityContainer container)
        {
            container.RegisterType<ISchedulerManagerService, SchedulerManagerService>();
        }

        private static void RegisterDataServices(IUnityContainer container)
        {
            container.RegisterType<ISchedulerInstanceService, SchedulerInstanceService>();
        }
    }
}