using Scheduler.Infrastructure.Dependencies.Configurations;
using Scheduler.WebConsole.Utilities.Dependencies;

namespace Scheduler.WebConsole
{
    public class UnityConfig
    {
        public static void Configure()
        {
            new BasicDependencyConfigurations();
            new DataServiceDependencyConfigurations();
            new RepositoryDependencyConfigurations();
            new ServiceDependencyConfigurations();
            new HubDependencyConfigurations();
            new MvcDependencyConfigurations();
        }
    }
}