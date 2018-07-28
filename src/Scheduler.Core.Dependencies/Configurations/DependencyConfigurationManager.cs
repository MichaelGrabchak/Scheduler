namespace Scheduler.Core.Dependencies.Configurations
{
    public static class DependencyConfigurationManager
    {
        public static void RegisterConfiguration(IDependencyConfiguration configuration)
        {
            configuration?.Configure();
        }
    }
}
