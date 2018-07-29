using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Domain.Data.Services;
using Scheduler.Infrastructure.Data.Services;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public class DataServiceDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            RegisterSettingsService();
            RegisterJobDetailService();
        }

        protected virtual void RegisterSettingsService()
        {
            Container.RegisterType<ISettingsService, SettingsService>();
        }

        protected virtual void RegisterJobDetailService()
        {
            Container.RegisterType<IJobDetailService, JobDetailService>();
        }
    }
}
