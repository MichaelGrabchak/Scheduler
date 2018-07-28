using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Domain.Services;
using Scheduler.Infrastructure.Services;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public class ServiceDependencyConfigurations : DependencyConfiguration
    {
        public override void Configure()
        {
            Container.RegisterType<ISchedulerManagerService, SchedulerManagerService>();
        }
    }
}
