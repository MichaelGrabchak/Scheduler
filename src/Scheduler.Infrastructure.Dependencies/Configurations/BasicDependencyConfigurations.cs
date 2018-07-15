using Scheduler.Core;
using Scheduler.Core.Context;
using Scheduler.Dependencies;
using Scheduler.Dependencies.Configurations;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;
using Scheduler.Domain.Data.Services;
using Scheduler.Engine;
using Scheduler.Engine.Jobs;
using Scheduler.Engine.Quartz;
using Scheduler.Infrastructure.Data.EntityFramework.Configuration.Context.Providers;
using Scheduler.Logging;
using Scheduler.Logging.Loggers;
using Scheduler.Logging.NLog;
using Scheduler.Logging.NLog.Loggers;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public class BasicDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            Container.RegisterType<ILogger, NLogLogger>(Constants.Scheduler.System.DefaultLogger);
            Container.RegisterType<ILoggerProvider, LoggerProvider>();

            Container.RegisterType<JobMetadata, QuartzJobMetadata>();
            Container.RegisterType<IDbContextProvider, SchedulerDbContextProvider>();
            Container.RegisterType<ISchedulerContext, SchedulerContext>();

            Container.RegisterSingleton<ISchedulerEngine, QuartzScheduler>(typeof(SchedulerSettings), typeof(JobMetadata), typeof(IJobDetailService));
        }
    }
}
