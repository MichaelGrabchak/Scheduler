using Scheduler.Core.Dependencies;
using Scheduler.Logging;
using Scheduler.Logging.NLog.Loggers;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public sealed class NLogLoggerDependencyConfigurations : LoggerDependencyConfigurations
    {
        protected override void RegisterLogger()
        {
            Container.RegisterType<ILogger, NLogLogger>(Constants.LoggerNames.DefaultLogger);
        }
    }
}
