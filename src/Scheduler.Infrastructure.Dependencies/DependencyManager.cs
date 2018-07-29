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
            RegisterBasicDependencyConfig();
            RegisterCacheDependencyConfig();
            RegisterLoggerDependencyConfig();
            RegisterDataServiceDependencyConfig();
            RegisterRepositoryDependencyConfig();
            RegisterServiceDependencyConfig();
            RegisterEngineDependencyConfig();
        }

        protected void RegisterBasicDependencyConfig()
        {
            DependencyConfigurationManager.AddConfiguration<BasicDependencyConfigurations>();
        }

        protected virtual void RegisterCacheDependencyConfig()
        {
            DependencyConfigurationManager.AddConfiguration<CacheDependencyConfigurations>();
        }

        protected virtual void RegisterLoggerDependencyConfig()
        {
            DependencyConfigurationManager.AddConfiguration<LoggerDependencyConfigurations>();
        }

        protected virtual void RegisterDataServiceDependencyConfig()
        {
            DependencyConfigurationManager.AddConfiguration<DataServiceDependencyConfigurations>();
        }

        protected void RegisterRepositoryDependencyConfig()
        {
            DependencyConfigurationManager.AddConfiguration<RepositoryDependencyConfigurations>();
        }

        protected virtual void RegisterServiceDependencyConfig()
        {
            DependencyConfigurationManager.AddConfiguration<ServiceDependencyConfigurations>();
        }

        protected virtual void RegisterEngineDependencyConfig()
        {
            DependencyConfigurationManager.AddConfiguration<EngineDependencyConfigurations>();
        }
    }
}
