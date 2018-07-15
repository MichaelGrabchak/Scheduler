using System;

namespace Scheduler.Logging
{
    public interface ILogger
    {
        void Debug(string message);
        void Info(string message);

        void Warn(string message);
        void Warn(Exception exception);
        void Warn(Exception exception, string message);

        void Error(string message);
        void Error(Exception exception);
        void Error(Exception exception, string message);
    }
}
