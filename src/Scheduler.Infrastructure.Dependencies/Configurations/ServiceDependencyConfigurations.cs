using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Domain.Services;
using Scheduler.Infrastructure.Services;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public class ServiceDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            RegisterSchedulerManagerService();
        }

        protected virtual void RegisterSchedulerManagerService()
        {
            Container.RegisterType<ISchedulerManagerService, SchedulerManagerService>();
        }
    }
}
