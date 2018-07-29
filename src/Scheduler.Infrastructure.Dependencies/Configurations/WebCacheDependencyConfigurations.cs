using Scheduler.Caching;
using Scheduler.Core.Dependencies;

namespace Scheduler.Infrastructure.Dependencies.Configurations
{
    public sealed class WebCacheDependencyConfigurations : CacheDependencyConfigurations
    {
        protected override void RegisterCacheContainer()
        {
            Container.RegisterType<ICache, WebCache>();
        }
    }
}
