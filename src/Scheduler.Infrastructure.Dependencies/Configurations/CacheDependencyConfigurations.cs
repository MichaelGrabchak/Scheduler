using Scheduler.Caching;
using Scheduler.Caching.Configurations;
using Scheduler.Core.Dependencies;
using Scheduler.Core.Dependencies.Configurations;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public class CacheDependencyConfigurations : IDependencyConfiguration
    {
        public void Configure()
        {
            RegisterCacheConfig();
            RegisterCacheContainer();
        }

        protected virtual void RegisterCacheConfig()
        {
            Container.RegisterType<ICacheConfiguration, CacheConfiguration>();
        }

        protected virtual void RegisterCacheContainer()
        {
            Container.RegisterType<ICache, NoCache>();
        }
    }
}
