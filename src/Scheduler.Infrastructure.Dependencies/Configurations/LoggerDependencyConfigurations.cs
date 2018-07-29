using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Logging;
using Scheduler.Logging.Loggers;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public class LoggerDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            RegisterLogger();
            RegisterProvider();
        }

        protected virtual void RegisterLogger()
        {
            Container.RegisterType<ILogger, NoLogger>(Constants.LoggerNames.DefaultLogger);
        }

        protected virtual void RegisterProvider()
        {
            Container.RegisterType<ILoggerProvider, LoggerProvider>();
        }
    }
}
