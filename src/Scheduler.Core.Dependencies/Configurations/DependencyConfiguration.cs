namespace Scheduler.Core.Dependencies.Configurations
{
    public abstract class DependencyConfiguration : IDependencyConfiguration
    {
        protected DependencyConfiguration()
        {
            DependencyConfigurationManager.RegisterConfiguration(this);
        }

        public abstract void Configure();
    }
}
