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
            Container.RegisterType<ILogger, NLogLogger>(LoggingConstants.LoggerNames.DefaultLogger);
            Container.RegisterType<ILoggerProvider, LoggerProvider>();

            Container.RegisterType<JobMetadata, QuartzJobMetadata>();
            Container.RegisterType<IDbContextProvider, SchedulerDbContextProvider>();
            Container.RegisterType<IContext, SchedulerContext>();

            Container.RegisterSingleton<ISchedulerEngine, QuartzScheduler>(typeof(SchedulerSettings), typeof(JobMetadata), typeof(IJobDetailService));
        }
    }
}
