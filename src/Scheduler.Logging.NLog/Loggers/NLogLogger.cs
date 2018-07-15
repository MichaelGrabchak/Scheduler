using System;

using Scheduler.Core;

using INLogLogger = NLog.ILogger;
using NLogLogManager = NLog.LogManager;

namespace Scheduler.Logging.NLog.Loggers
{
    public class NLogLogger : BaseLogger, ILogger
    {
        private readonly INLogLogger _logger;

        public NLogLogger()
        {
            _logger = NLogLogManager.GetCurrentClassLogger();
        }

        public NLogLogger(string logName)
        {
            if (logName == Constants.Scheduler.System.DefaultLogger || string.IsNullOrEmpty(logName))
            {
                _logger = NLogLogManager.GetCurrentClassLogger();
                return;
            }

            _logger = NLogLogManager.GetLogger(logName);
        }

        public void Debug(string message) => _logger.Debug(message);

        public void Info(string message) => _logger.Info(message);

        public void Warn(string message) => _logger.Warn(message);

        public void Warn(Exception exception) => _logger.Warn(exception);

        public void Warn(Exception exception, string message) => _logger.Warn(exception, message);

        public void Error(string message) => _logger.Error(message);

        public void Error(Exception exception) => _logger.Error(exception);

        public void Error(Exception exception, string message) => _logger.Error(exception, message);
    }
}
