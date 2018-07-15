using Scheduler.Dependencies;
using Scheduler.Dependencies.Configurations;
using Scheduler.Domain.Data.Repositories;
using Scheduler.Infrastructure.Data.EntityFramework.Repositories;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public class RepositoryDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            Container.RegisterType<IJobDetailRepository, JobDetailRepository>();
            Container.RegisterType<ISchedulerInstanceRepository, SchedulerInstanceRepository>();
            Container.RegisterType<ISchedulerInstanceSettingRepository, SchedulerInstanceSettingRepository>();
        }
    }
}
