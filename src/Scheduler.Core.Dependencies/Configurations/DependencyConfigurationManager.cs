namespace Scheduler.Core.Dependencies.Configurations
{
    public static class DependencyConfigurationManager
    {
        public static void AddConfiguration<T>() where T : IDependencyConfiguration, new()
        {
            var dependencies = new T();

            dependencies.Configure();
        }
    }
}
