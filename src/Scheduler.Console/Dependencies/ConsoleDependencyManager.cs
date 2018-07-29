using Scheduler.Console.Dependencies.Configurations;
using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Infrastructure.Dependencies;
using Scheduler.Infrastructure.Dependencies.Configurations;

namespace Scheduler.Console.Dependencies
{
    public class ConsoleDependencyManager : DependencyManager
    {
        protected override void RegisterAllDependencies()
        {
            base.RegisterAllDependencies();

            DependencyConfigurationManager.AddConfiguration<ConsoleAppDependencyConfiguration>();
        }

        protected override void RegisterLoggerDependencyConfig()
        {
            DependencyConfigurationManager.AddConfiguration<NLogLoggerDependencyConfigurations>();
        }
    }
}
