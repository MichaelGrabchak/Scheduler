using Scheduler.Dependencies;
using Scheduler.Dependencies.Configurations;
using Scheduler.Domain.Services;
using Scheduler.Infrastructure.Services;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public class ServiceDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            Container.RegisterType<ISchedulerManagerService, SchedulerManagerService>();
        }
    }
}
