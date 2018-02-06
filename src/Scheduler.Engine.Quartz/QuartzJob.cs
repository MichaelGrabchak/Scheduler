using System;

using Scheduler.Core.Helpers;
using Scheduler.Core.Configurations;
using Scheduler.Jobs;

using Quartz;

namespace Scheduler.Engine.Quartz
{
    public class QuartzJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var typeFullName = context.JobDetail?.JobDataMap?.GetString("TypeFullName");

                var jobType = AssemblyLoaderManager.GetType(typeFullName);

                if (jobType != null)
                {
                    var jobObj = GlobalUnity.Resolve<BaseJob>(jobType);

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