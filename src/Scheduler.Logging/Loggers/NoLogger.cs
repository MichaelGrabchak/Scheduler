using System;

namespace Scheduler.Logging.Loggers
{
    public class NoLogger : BaseLogger, ILogger
    {
        public NoLogger(string logName)
            : base(logName)
        {

        }

        public void Debug(string message)
        {
            
        }

        public void Info(string message)
        {
            
        }

        public void Warn(string message)
        {
            
        }

        public void Warn(Exception exception)
        {
            
        }

        public void Warn(Exception exception, string message)
        {
            
        }

        public void Error(string message)
        {
            
        }

        public void Error(Exception exception)
        {
            
        }

        public void Error(Exception exception, string message)
        {
            
        }
    }
}
