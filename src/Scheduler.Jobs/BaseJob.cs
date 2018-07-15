using Scheduler.Jobs.Extensions;
using Scheduler.Logging;
using Scheduler.Logging.Loggers;

namespace Scheduler.Jobs
{
    public abstract class BaseJob
    {
        protected readonly ILogger Logger;

        protected BaseJob()
        {
            Logger = LogManager.GetLogger(this.GetLogger());
        }

        public abstract string Schedule { get; }

        public void Execute()
        {
            Logger.Info($"Starting {this.GetGroup()}.{this.GetName()} job...");

            ExecuteJob();

            Logger.Info("Finishing execution of the job.");
        }

        public abstract void ExecuteJob();
    }
}
