using System;

using Scheduler.Core.Configurations;
using Scheduler.Core.Context;
using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;
using Scheduler.Domain.Data.Services;
using Scheduler.Infrastructure.Data.EntityFramework.Context.Providers;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public sealed class BasicDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            RegisterConfiguration();
            RegisterContext();
            RegisterDbContextProvider();
        }

        private void RegisterConfiguration()
        {
            Func<ApplicationConfiguration> configurationFactory = () => Container.Resolve<ISettingsService>().GetSettings<ApplicationConfiguration>();
            Container.RegisterFactory<IEngineConfiguration>(configurationFactory);
            Container.RegisterFactory<IApplicationConfiguration>(configurationFactory);
        }

        private void RegisterContext()
        {
            Container.RegisterType<IApplicationContext, ApplicationContext>();
            Container.RegisterType<IDataWarehouseContext, DataWarehouseContext>();
        }

        private void RegisterDbContextProvider()
        {
            Container.RegisterType<IDbContextProvider, SchedulerDbContextProvider>();
        }
    }
}
