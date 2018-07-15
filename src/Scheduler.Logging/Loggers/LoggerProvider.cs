using System;
using System.Collections.Generic;

using Scheduler.Core.Dependencies;

namespace Scheduler.Logging.Loggers
{
    public class LoggerProvider : ILoggerProvider
    {
        public virtual ILogger GetLogger()
        {
            return ResolveLogger();
        }

        public virtual ILogger GetLogger(string logName)
        {
            return ResolveLogger(logName);
        }

        public virtual ILogger GetLogger(string job, string group)
        {
            return ResolveLogger(job, group);
        }

        protected string BuildName(string job, string group)
        {
            if(string.IsNullOrEmpty(job) || string.IsNullOrEmpty(group))
                throw new ArgumentException("The job or group name is missing");

            return $"{job}.{group}";
        }

        #region Helpers

        private ILogger ResolveLogger()
        {
            return Container.Resolve<ILogger>();
        }

        private ILogger ResolveLogger(string logName)
        {
            if (string.IsNullOrEmpty(logName))
            {
                return ResolveLogger();
            }

            return Container.Resolve<ILogger>(new KeyValuePair<string, object>("logName", logName));
        }

        private ILogger ResolveLogger(string job, string group)
        {
            if (string.IsNullOrEmpty(job) || string.IsNullOrEmpty(group))
            {
                return ResolveLogger();
            }

            return ResolveLogger(BuildName(job, group));
        }

        #endregion
    }
}
