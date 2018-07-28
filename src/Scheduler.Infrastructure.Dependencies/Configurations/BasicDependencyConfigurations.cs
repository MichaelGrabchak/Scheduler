using System;

using Scheduler.Caching;
using Scheduler.Caching.Configurations;
using Scheduler.Core.Configurations;
using Scheduler.Core.Context;
using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;
using Scheduler.Domain.Data.Services;
using Scheduler.Engine;
using Scheduler.Engine.Jobs;
using Scheduler.Engine.Quartz;
using Scheduler.Infrastructure.Data.EntityFramework.Context.Providers;
using Scheduler.Logging;
using Scheduler.Logging.Loggers;
using Scheduler.Logging.NLog.Loggers;

using LoggingConstants = Scheduler.Logging.Constants;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public class BasicDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            Container.RegisterType<ICache, WebCache>();
            Container.RegisterType<ICacheConfiguration, CacheConfiguration>();
            Container.RegisterType<ILogger, NLogLogger>(LoggingConstants.LoggerNames.DefaultLogger);
            Container.RegisterType<ILoggerProvider, LoggerProvider>();

            Func<ApplicationConfiguration> configurationFactory = () => Container.Resolve<ISchedulerSettingsService>().GetSettings<ApplicationConfiguration>();
            Container.RegisterFactory<IEngineConfiguration>(configurationFactory);

            Container.RegisterType<IApplicationContext, ApplicationContext>();
            Container.RegisterType<IDataWarehouseContext, DataWarehouseContext>();
            Container.RegisterType<IDbContextProvider, SchedulerDbContextProvider>();

            Container.RegisterType<JobMetadata, QuartzJobMetadata>();
            Container.RegisterSingleton<ISchedulerEngine, QuartzScheduler>(typeof(SchedulerSettings), typeof(JobMetadata), typeof(IJobDetailService));
        }
    }
}
