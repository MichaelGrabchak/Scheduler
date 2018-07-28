using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Infrastructure.Dependencies.Configurations;

namespace Scheduler.Infrastructure.Dependencies
{
    public abstract class DependencyManager
    {
        public void RegisterDependencies()
        {
            RegisterAllDependencies();
        }

        protected virtual void RegisterAllDependencies()
        {
            DependencyConfigurationManager.AddConfiguration<BasicDependencyConfigurations>();
            DependencyConfigurationManager.AddConfiguration<DataServiceDependencyConfigurations>();
            DependencyConfigurationManager.AddConfiguration<RepositoryDependencyConfigurations>();
            DependencyConfigurationManager.AddConfiguration<ServiceDependencyConfigurations>();
        }
    }
}
