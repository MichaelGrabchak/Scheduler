using Scheduler.Core.Dependencies.Configurations;
using Scheduler.Infrastructure.Dependencies;
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
    }
}
