using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Infrastructure.Dependencies;
using Scheduler.Infrastructure.Dependencies.Configurations;
using Scheduler.WebConsole.Dependencies.Configurations;

namespace Scheduler.WebConsole.Dependencies
{
    public class WebAppDependencyManager : DependencyManager
    {
        protected override void RegisterAllDependencies()
        {
            base.RegisterAllDependencies();

            DependencyConfigurationManager.AddConfiguration<HubDependencyConfigurations>();
            DependencyConfigurationManager.AddConfiguration<MvcDependencyConfigurations>();
        }

        protected override void RegisterCacheDependencyConfig()
        {
            DependencyConfigurationManager.AddConfiguration<WebCacheDependencyConfigurations>();
        }

        protected override void RegisterLoggerDependencyConfig()
        {
            DependencyConfigurationManager.AddConfiguration<NLogLoggerDependencyConfigurations>();
        }
    }
}
