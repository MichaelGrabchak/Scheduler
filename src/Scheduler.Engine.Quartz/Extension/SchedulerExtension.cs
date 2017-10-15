using System;

using Scheduler.Core.Jobs;

using Quartz;

namespace Scheduler.Engine.Quartz.Extension
{
    public static class SchedulerExtension
    {
        public static IJobDetail GetJobDetail(this IScheduler scheduler, string jobName, string jobGroup)
        {
            if (scheduler == null)
            {
                throw new ArgumentNullException(nameof(scheduler));
            }

            if (string.IsNullOrEmpty(jobName))
            {
                throw new ArgumentNullException(nameof(jobName));
            }

            if (string.IsNullOrEmpty(jobGroup))
            {
                throw new ArgumentNullException(nameof(jobGroup));
            }

            var jobKey = JobKey.Create(jobName, jobGroup);

            if (jobKey == null)
            {
                throw new NullReferenceException($"Could not find the job by specified name({jobName}) and group({jobGroup})");
            }

            return scheduler.GetJobDetail(jobKey);
        }

        public static JobInfo GetJobInfo(this IJobExecutionContext context, JobState state = JobState.None)
        {
            var jobDetail = context.JobDetail?.Key;

            if (jobDetail != null)
            {
                return new JobInfo
                {
                    Name = jobDetail.Name,
                    Group = jobDetail.Group,
                    State = (state != JobState.None) ? state.ToString() : string.Empty,
                    PrevFireTimeUtc = context.ScheduledFireTimeUtc,
                    NextFireTimeUtc = context.NextFireTimeUtc
                };
            }

            return null;
        }

        public static JobState GetJobState(this TriggerState triggerState)
        {
            switch(triggerState)
            {
                case TriggerState.Normal:
                    return JobState.Normal;

                case TriggerState.Paused:
                    return JobState.Paused;

                case TriggerState.Complete:
                    return JobState.Succeeded;

                case TriggerState.Error:
                    return JobState.Failed;

                case TriggerState.Blocked:
                    return JobState.Skipped;

                default:
                    return JobState.None;
            }
        }
    }
}