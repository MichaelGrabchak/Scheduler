using Scheduler.Core.Logging;
using Scheduler.Jobs.Extensions;

namespace Scheduler.Jobs
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
