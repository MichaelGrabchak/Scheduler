using Quartz;

using System;

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
    }
}