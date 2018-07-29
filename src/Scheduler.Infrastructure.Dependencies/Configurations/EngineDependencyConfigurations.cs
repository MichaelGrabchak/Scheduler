using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Domain.Data.Services;
using Scheduler.Engine;
using Scheduler.Engine.Jobs;
using Scheduler.Engine.Quartz;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public class EngineDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            RegisterJobMetadata();
            RegisterEngine();
        }

        protected virtual void RegisterJobMetadata()
        {
            Container.RegisterType<JobMetadata, QuartzJobMetadata>();
        }

        protected virtual void RegisterEngine()
        {
            Container.RegisterSingleton<ISchedulerEngine, QuartzScheduler>(typeof(SchedulerSettings), typeof(JobMetadata), typeof(IJobDetailService));
        }
    }
}
