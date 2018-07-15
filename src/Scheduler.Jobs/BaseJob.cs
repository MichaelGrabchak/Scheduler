using Scheduler.Jobs.Extensions;
using Scheduler.Logging;
using Scheduler.Logging.Loggers;

namespace Scheduler.Jobs
{
    public abstract class BaseJob
    {
        private readonly ILogger _logger;

        protected BaseJob()
        {
            _logger = LogManager.GetLogger(this.GetLogger());
        }

        public abstract string Schedule { get; }

        public void Execute()
        {
            _logger.Info($"Starting {this.GetGroup()}.{this.GetName()} job...");

            ExecuteJob();

            _logger.Info("Finishing execution of the job.");
        }

        public abstract void ExecuteJob();
    }
}
