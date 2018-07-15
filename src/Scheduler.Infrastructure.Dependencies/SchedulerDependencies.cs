using Scheduler.Infrastructure.Dependencies.Configurations;

namespace Scheduler.Infrastructure.Dependencies
{
    public static class SchedulerDependencies
    {
        public static void Configure()
        {
            new BasicDependencyConfigurations().Configure();
            new ServiceDependencyConfigurations().Configure();
            new DataServiceDependencyConfigurations().Configure();
            new RepositoryDependencyConfigurations().Configure();
        }
    }
}
