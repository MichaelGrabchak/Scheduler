using System.Threading;
using System.Threading.Tasks;

using Scheduler.Engine.Enums;
using Scheduler.Engine.Jobs;
using Scheduler.Engine.Quartz.Extension;

using Quartz;

using Scheduler.Logging.Loggers;

namespace Scheduler.Engine.Quartz.Listeners
{
    public class DependentJobListener : IJobListener
    {
        public event JobOperationEventHandler ToBeExecuted;
        public event JobOperationEventHandler Executed;
        public event JobOperationEventHandler ExecutionVetoed;
        public event JobOperationEventHandler ExecutionSucceeded;
        public event JobOperationEventHandler ExecutionFailed;

        public string Name => "DependentJobListener";

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Run(() =>
            {
                var jobInfo = context.GetJobInfo(JobActionState.Skipped);

                if (jobInfo != null)
                {
                    LogManager
                        .GetLogger(jobInfo.Group, jobInfo.Name)
                        .Debug($"The execution of job '{jobInfo.Group}.{jobInfo.Name}' has been skipped due to failed condition(s)");

                    OnExecutionVetoed(jobInfo);
                }
            }, cancellationToken);
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Run(() =>
            {
                var jobInfo = context.GetJobInfo(JobActionState.Executing);

                if (jobInfo != null)
                {
                    LogManager
                        .GetLogger(jobInfo.Group, jobInfo.Name)
                        .Debug($"Preparing to execute job '{jobInfo.Group}.{jobInfo.Name}'...");

                    OnToBeExecuted(jobInfo);
                }
            }, cancellationToken);
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.Run(() =>
            {
                var jobInfo = context.GetJobInfo();

                if (jobInfo != null)
                {
                    var logger = LogManager.GetLogger(jobInfo.Group, jobInfo.Name);

                    if (jobException != null)
                    {
                        jobInfo.ActionState = JobActionState.Failed.ToString();


                        logger.Warn(jobException.GetBaseException());

                        OnExecutionFailed(jobInfo);
                    }
                    else
                    {
                        jobInfo.ActionState = JobActionState.Succeeded.ToString();

                        OnExecutionSucceeded(jobInfo);

                        logger.Info($"The job '{jobInfo.Group}.{jobInfo.Name}' has been executed successfully");
                    }

                    OnExecuted(jobInfo);
                }
            }, cancellationToken);
        }

        #region Event handlers

        private void OnExecuted(JobInfo jobInfo)
        {
            Executed?.Invoke(this,
                new JobOperationEventArgs { Job = jobInfo });
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

        #endregion
    }
}
