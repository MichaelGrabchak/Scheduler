using System;

namespace Scheduler.Core.Logging
{
    public abstract class BaseLogger : ISchedulerLogger
    {
        public BaseLogger(string loggerName)
        {

        }

        public abstract void Debug(string message);
        public abstract void Error(string message);
        public abstract void Error(Exception exception);
        public abstract void Error(Exception exception, string message);
        public abstract void Info(string message);
        public abstract void Warn(string message);
        public abstract void Warn(Exception exception);
        public abstract void Warn(Exception exception, string message);
    }
}
