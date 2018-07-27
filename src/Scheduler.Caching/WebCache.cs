using System;
using System.Web;
using System.Web.Caching;

using Scheduler.Core.Configurations;
using Scheduler.Core.Extensions;

namespace Scheduler.Caching
{
    public class WebCache : ICache
    {
        private static Cache SystemCache => HttpContext.Current.Cache;

        private readonly int _expirationMinutes;

        public WebCache(ICacheConfiguration configuration)
        {
            _expirationMinutes = configuration.CacheExpiration ?? Constants.DefaultExpirationInMinutes;
        }

        public bool IsCached(string key)
        {
            return SystemCache[key] != null;
        }

        public T Get<T>(string key)
        {
            if (IsCached(key))
            {
                return SystemCache[key].To<T>();
            }

            return default(T);
        }

        public void Set(string key, object value)
        {
            if (IsCached(key))
            {
                Remove(key);
            }

            Add(key, value, _expirationMinutes);
        }

        public void Remove(string key)
        {
            SystemCache.Remove(key);
        }

        #region Helpers

        private static void Add(string key, object value, int expirationMinutes)
        {
            SystemCache.Add(key, value, null, DateTime.Now.AddMinutes(expirationMinutes),
                Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
        }

        #endregion
    }
}
