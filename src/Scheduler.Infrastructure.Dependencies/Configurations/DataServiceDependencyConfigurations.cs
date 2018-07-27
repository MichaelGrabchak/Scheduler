using System;

using Scheduler.Core.Configurations;
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
            Container.RegisterType<ISchedulerSettingsService, SchedulerSettingsService>();
            Container.RegisterType<IJobDetailService, JobDetailService>();

            Func<ApplicationConfiguration> configurationFactory = () => Container.Resolve<ISchedulerSettingsService>().GetSettings<ApplicationConfiguration>();
            Container.RegisterFactory(configurationFactory);
            Container.RegisterFactory<ICacheConfiguration>(configurationFactory);
            Container.RegisterFactory<IEngineConfiguration>(configurationFactory);
        }
    }
}
