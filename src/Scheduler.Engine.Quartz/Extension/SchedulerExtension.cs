using System;
using System.Linq;
using System.Collections.Generic;

using Scheduler.Engine.Enums;
using Scheduler.Engine.Jobs;

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

        public static JobInfo GetJobInfo(this IJobExecutionContext context, JobActionState state = JobActionState.None)
        {
            var jobDetail = context.JobDetail?.Key;

            if (jobDetail != null)
            {
                return new JobInfo
                {
                    Name = jobDetail.Name,
                    Group = jobDetail.Group,
                    ActionState = (state != JobActionState.None) ? state.ToString() : string.Empty,
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

                default:
                    return JobState.Paused;
            }
        }

        public static bool ContainsJob(this IList<IJobExecutionContext> contextCollection, string jobName, string jobGroup = "DEFAULT")
        {
            if(contextCollection.Count == 0)
            {
                return false;
            }

            return contextCollection
                .Select(context => context.GetJobInfo())
                .Any(jobInfo 
                    => jobInfo.Name.Equals(jobName, StringComparison.InvariantCultureIgnoreCase) 
                    && jobInfo.Group.Equals(jobGroup, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}