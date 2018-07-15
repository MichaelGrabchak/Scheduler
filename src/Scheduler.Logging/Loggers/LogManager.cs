using Scheduler.Core.Dependencies;

namespace Scheduler.Logging.Loggers
{
    public static class LogManager
    {
        private static readonly ILoggerProvider LoggerProvider;

        static LogManager()
        {
            LoggerProvider = Container.Resolve<ILoggerProvider>();
        }

        public static ILogger GetLogger()
        {
            return LoggerProvider.GetLogger();
        }

        public static ILogger GetLogger(string logName)
        {
            return LoggerProvider.GetLogger(logName);
        }

        public static ILogger GetLogger(string job, string group)
        {
            return LoggerProvider.GetLogger(job, group);
        }
    }
}
