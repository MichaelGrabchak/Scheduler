using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Domain.Data.Services;
using Scheduler.Infrastructure.Data.Services;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public class DataServiceDependencyConfigurations : DependencyConfiguration
    {
        public override void Configure()
        {
            Container.RegisterType<ISchedulerSettingsService, SchedulerSettingsService>();
            Container.RegisterType<IJobDetailService, JobDetailService>();
        }
    }
}
