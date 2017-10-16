using System;

using NLog;
using System.ComponentModel;

namespace Scheduler.Core.Jobs
{
    public abstract class BaseJob
    {
        private readonly ILogger Logger;

        public BaseJob()
        {
            Logger = LogManager.GetLogger(Constants.System.DefaultSchedulerLoggerName);
        }

        public BaseJob(string loggerName)
        {
            Logger = LogManager.GetLogger(loggerName);
        }

        public abstract string Schedule { get; }

        public void Execute()
        {
            Logger.Info("Starting {0} job...", GetType().Name);

            ExecuteJob();

            Logger.Info("Finishing execution of the job.");
        }

        public abstract void ExecuteJob();

        public string GetDescription()
        {
            var descriptions = (DescriptionAttribute[])GetType().GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (descriptions.Length == 0)
            {
                return string.Empty;
            }

            return descriptions[0].Description;
        }

        protected void LogTrace(string message)
        {
            Logger.Trace(message);
        }

        protected void LogDebug(string message)
        {
            Logger.Debug(message);
        }

        protected void LogInfo(string message)
        {
            Logger.Info(message);
        }

        protected void LogWarning(string message)
        {
            Logger.Warn(message);
        }

        protected void LogWarning(Exception exception, string message)
        {
            Logger.Warn(exception, message);
        }

        protected void LogError(string message)
        {
            Logger.Error(message);
        }

        protected void LogError(Exception exception, string message)
        {
            Logger.Error(exception, message);
        }

        protected void LogFatal(Exception exception, string message)
        {
            Logger.Fatal(exception, message);
        }
    }
}
