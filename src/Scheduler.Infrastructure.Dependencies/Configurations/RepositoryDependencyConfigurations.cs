using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Domain.Data.Repositories;
using Scheduler.Infrastructure.Data.EntityFramework.Repositories;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public sealed class RepositoryDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            Container.RegisterType<IJobDetailRepository, JobDetailRepository>();
            Container.RegisterType<IInstanceRepository, InstanceRepository>();
            Container.RegisterType<ISettingRepository, SettingRepository>();
        }
    }
}
