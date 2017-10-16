using System;

using Scheduler.Core;
using Scheduler.Core.Jobs;
using Scheduler.Core.Helpers;
using Scheduler.Core.Extensions;

using NLog;

using Quartz;

namespace Scheduler.Engine.Quartz
{
    public class QuartzJob : IJob
    {
        private static ILogger Logger = LogManager.GetLogger(Constants.System.DefaultSchedulerLoggerName);

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var TypeFullName = context.JobDetail?.JobDataMap?.GetString("TypeFullName");
                if (string.IsNullOrEmpty(TypeFullName))
                {
                    Logger.Warn($"The type full name is missing. Skipping execution of '{context.JobDetail.Key}'");
                    return;
                }

                var jobType = AssemblyLoaderManager.GetType(TypeFullName);

                if (jobType != null)
                {
                    var jobObj = jobType.Resolve<BaseJob>();

                    jobObj.Execute();
                }
            }
            catch (Exception ex)
            {
                throw new JobExecutionException("An unexpected error has occurred during job execution", ex, false);
            }
        }
    }
}