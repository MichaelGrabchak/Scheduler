using System;

using Scheduler.Core.Helpers;
using Scheduler.Dependencies;
using Scheduler.Jobs;

using Quartz;
using System.Threading.Tasks;

namespace Scheduler.Engine.Quartz
{
    public class QuartzJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() => 
            {
                try
                {
                    var typeFullName = context.JobDetail?.JobDataMap?.GetString("TypeFullName");

                    var jobType = AssemblyLoaderManager.GetType(typeFullName);

                    if (jobType != null)
                    {

                    }
                    var jobObj = Container.Resolve<BaseJob>(jobType);

                    jobObj.Execute();
                }
                catch (Exception ex)
                {
                    throw new JobExecutionException("An unexpected error has occurred during job execution", ex, false);
                }
            });
        }
    }
}