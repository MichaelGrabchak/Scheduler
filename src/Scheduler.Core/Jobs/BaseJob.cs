using Scheduler.Core.Extensions;

using Scheduler.Core.Logging;

namespace Scheduler.Core.Jobs
{
    public abstract class BaseJob
    {
        private readonly ISchedulerLogger Logger;

        public BaseJob()
        {
            Logger = SchedulerLogManager.GetJobLogger(this.GetLogger());
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
