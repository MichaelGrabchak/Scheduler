using System;
using System.Web.Mvc;

using Scheduler.Core;
using Scheduler.Core.Context;
using Scheduler.Core.Logging;
using Scheduler.Core.Configurations;
using Scheduler.Engine;
using Scheduler.Engine.Jobs;
using Scheduler.Engine.Quartz;
using Scheduler.Domain.Services;
using Scheduler.Domain.Data.Services;
using Scheduler.Domain.Data.Repositories;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;
using Scheduler.Infrastructure.Services;
using Scheduler.Infrastructure.Data.Services;
using Scheduler.Infrastructure.Data.EntityFramework.Repositories;
using Scheduler.Infrastructure.Data.EntityFramework.Configuration.Context.Providers;
using Scheduler.Logging.NLog;

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
            RegisterBasic(container);
            RegisterRepositories(container);
            RegisterServices(container);
            RegisterDataServices(container);

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            GlobalUnity.Container = container;
        }

        private static void RegisterBasic(IUnityContainer container)
        {
            container.RegisterType<JobMetadata, QuartzJobMetadata>();
            container.RegisterType<IDbContextProvider, SchedulerDbContextProvider>();
            container.RegisterType<ISchedulerContext, SchedulerContext>();
            container.RegisterType<ISchedulerLogger, NLogLogger>(new InjectionConstructor(Constants.System.DefaultSchedulerLoggerName));
            container.RegisterType<ISchedulerEngine, QuartzScheduler>(new ContainerControlledLifetimeManager(), new InjectionConstructor(typeof(SchedulerSettings), typeof(JobMetadata), typeof(IJobDetailService)));
        }

        private static void RegisterServices(IUnityContainer container)
        {
            container.RegisterType<ISchedulerManagerService, SchedulerManagerService>();
        }

        private static void RegisterDataServices(IUnityContainer container)
        {
            container.RegisterType<ISchedulerInstanceService, SchedulerInstanceService>();
            container.RegisterType<IJobDetailService, JobDetailService>();
        }

        private static void RegisterRepositories(IUnityContainer container)
        {
            container.RegisterType<IJobDetailRepository, JobDetailRepository>();
            container.RegisterType<ISchedulerInstanceRepository, SchedulerInstanceRepository>();
            container.RegisterType<ISchedulerInstanceSettingRepository, SchedulerInstanceSettingRepository>();
        }
    }
}