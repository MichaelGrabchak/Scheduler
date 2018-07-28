using System.Configuration;

using Scheduler.Core.Extensions;

using CacheKey = Scheduler.Caching.Constants.ConfigurationKeys;

namespace Scheduler.Caching.Configurations
{
    public class CacheConfiguration : ICacheConfiguration
    {
        public int? CacheExpiration { get; }

        public CacheConfiguration()
        {
            CacheExpiration = ConfigurationManager.AppSettings[CacheKey.CacheExpirationKey].To<int?>();
        }
    }
}