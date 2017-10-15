using Scheduler.Engine.Quartz.Extension;

using Scheduler.Core;
using Scheduler.Core.Jobs;
using Scheduler.Core.Engine;

using NLog;
using Quartz;

namespace Scheduler.Engine.Quartz.Listeners
{
    public class DependentJobListener : IJobListener
    {
        private static ILogger Logger = LogManager.GetLogger(Constants.System.DefaultSchedulerLoggerName);

        public event JobOperationEventHandler ToBeExecuted;
        public event JobOperationEventHandler ExecutionVetoed;
        public event JobOperationEventHandler ExecutionSucceeded;
        public event JobOperationEventHandler ExecutionFailed;

        public string Name => "DependentJobListener";

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            var jobInfo = context.GetJobInfo(JobState.Skipped);

            if (jobInfo != null)
            {
                Logger.Debug($"The execution of job '{jobInfo.Group}.{jobInfo.Name}' has been skipped due to failed condition(s)");

                OnExecutionVetoed(jobInfo);
            }
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            var jobInfo = context.GetJobInfo(JobState.Executing);

            if (jobInfo != null)
            {
                Logger.Debug($"Preparing to execute job '{jobInfo.Group}.{jobInfo.Name}'...");

                OnToBeExecuted(jobInfo);
            }
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            var jobInfo = context.GetJobInfo();

            if (jobInfo != null)
            {
                if (jobException != null)
                {
                    jobInfo.State = JobState.Failed.ToString();

                    Logger.Warn(jobException, $"An error has occurred during execution of '{jobInfo.Group}.{jobInfo.Name}' job: ");

                    OnExecutionFailed(jobInfo);
                }
                else
                {
                    jobInfo.State = JobState.Succeeded.ToString();

                    OnExecutionSucceeded(jobInfo);

                    Logger.Debug($"The job '{jobInfo.Group}.{jobInfo.Name}' has been executed successfully");
                }
            }
        }

        private void OnToBeExecuted(JobInfo jobInfo)
        {
            ToBeExecuted?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        private void OnExecutionVetoed(JobInfo jobInfo)
        {
            ExecutionVetoed?.Invoke(this, 
                new JobOperationEventArgs { Job = jobInfo });
        }

        private void OnExecutionSucceeded(JobInfo jobInfo)
        {
            ExecutionSucceeded?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        private void OnExecutionFailed(JobInfo jobInfo)
        {
            ExecutionFailed?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }
    }
}
