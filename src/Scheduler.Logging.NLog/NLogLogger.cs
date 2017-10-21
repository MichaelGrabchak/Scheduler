using System;

using Scheduler.Core.Logging;

using NLog;

namespace Scheduler.Logging.NLog
{
    public class NLogLogger : BaseLogger
    {
        private readonly ILogger _logger;

        public NLogLogger(string loggerName)
            : base(loggerName)
        {
            _logger = LogManager.GetLogger(loggerName);
        }

        public override void Debug(string message) => _logger.Debug(message);

        public override void Info(string message) => _logger.Info(message);

        public override void Error(string message) => _logger.Error(message);

        public override void Error(Exception exception) => _logger.Error($"An unexpected error has occurred:{Environment.NewLine}{exception}");

        public override void Error(Exception exception, string message) => _logger.Error($"{message}. Exception(s):{Environment.NewLine}{exception}");

        public override void Warn(string message) => _logger.Warn(message);

        public override void Warn(Exception exception) => _logger.Warn($"An unexpected error has occurred:{Environment.NewLine}{exception}");

        public override void Warn(Exception exception, string message) => _logger.Warn($"{message}. Exception(s):{Environment.NewLine}{exception}");
    }
}
