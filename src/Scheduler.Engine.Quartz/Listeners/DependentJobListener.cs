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
        public event JobOperationEventHandler WasExecuted;

        public string Name => "DependentJobListener";

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            var jobDetail = context.JobDetail?.Key;

            if(jobDetail != null)
            {
                Logger.Debug($"The execution of job '{jobDetail.Group}.{jobDetail.Name}' has been skipped due to failed condition(s)");

                OnExecutionVetoed(new JobInfo { Name = jobDetail.Name, Group = jobDetail.Group });
            }
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            var jobDetail = context.JobDetail?.Key;

            if(jobDetail != null)
            {
                Logger.Debug($"Preparing to execute job '{jobDetail.Group}.{jobDetail.Name}'...");

                OnToBeExecuted(new JobInfo { Name = jobDetail.Name, Group = jobDetail.Group });
            }
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            var jobDetail = context.JobDetail?.Key;

            if(jobDetail != null)
            {
                var jobInfo = new JobInfo { Name = jobDetail.Name, Group = jobDetail.Group };

                if (jobException != null)
                {
                    Logger.Warn(jobException, $"An error has occurred during execution of '{jobDetail.Group}.{jobDetail.Name}' job: ");

                    OnExecutionFailed(jobInfo);

                    return;
                }
                else
                {
                    OnExecutionSucceeded(jobInfo);

                    Logger.Debug($"The job '{jobDetail.Group}.{jobDetail.Name}' has been executed successfully");
                }

                OnWasExecuted(jobInfo);
            }
        }

        private void OnToBeExecuted(JobInfo jobInfo)
        {
            ToBeExecuted?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
        }

        private void OnWasExecuted(JobInfo jobInfo)
        {
            WasExecuted?.Invoke(this,
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
