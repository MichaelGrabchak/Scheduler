using Scheduler.Core;

using NLog;
using Quartz;

namespace Scheduler.Engine.Quartz.Listeners
{
    public class DependentJobListener : IJobListener
    {
        private static ILogger Logger = LogManager.GetLogger(Constants.System.DefaultSchedulerLoggerName);

        public string Name => "DependentJobListener";

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            var jobDetail = context.JobDetail?.Key;

            Logger.Debug($"The execution of job '{jobDetail.Group}.{jobDetail.Name}' has been skipped due to failed dependent job state");
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            var jobDetail = context.JobDetail?.Key;

            Logger.Debug($"Preparing to execute job '{jobDetail.Group}.{jobDetail.Name}'...");
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            var jobDetail = context.JobDetail?.Key;

            if(jobException != null)
            {
                Logger.Warn(jobException, $"An error has occurred during execution of '{jobDetail.Group}.{jobDetail.Name}' job: ");
            }

            Logger.Debug($"The job '{jobDetail.Group}.{jobDetail.Name}' has been executed successfully");
        }
    }
}
