using System.Collections.Generic;
using System.Linq;

using Scheduler.Caching;
using Scheduler.Core.Extensions;
using Scheduler.Domain.Data.Repositories;
using Scheduler.Domain.Data.Services;

using CacheKeys = Scheduler.Core.Constants.Cache;

namespace Scheduler.Infrastructure.Data.Services
{
    public class SchedulerSettingsService : ISchedulerSettingsService
    {
        private readonly ICache _cache;
        private readonly ISchedulerInstanceSettingRepository _schedulerInstanceSettingRepository;

        public SchedulerSettingsService(ICache cache, ISchedulerInstanceSettingRepository schedulerInstanceSettingRepository)
        {
            _cache = cache;
            _schedulerInstanceSettingRepository = schedulerInstanceSettingRepository;
        }

        // TODO: Implement storing of the data into data warehouse
        public void SetSettings<T>(T settings)
        {
            _cache.Set(CacheKeys.ApplicationSettingsKey, settings.AsKeyValuePairs());
        }

        public T GetSettings<T>() where T : new() 
        {
            var result = new T();

            var kvps = new List<KeyValuePair<string, string>>();

            if (_cache.IsCached(CacheKeys.ApplicationSettingsKey))
            {
                kvps = _cache.Get<List<KeyValuePair<string, string>>>(CacheKeys.ApplicationSettingsKey);
            }

            if (!kvps.Any())
            {
                kvps = _schedulerInstanceSettingRepository
                       .GetInstanceSettings()
                       .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                       .ToList();

                _cache.Set(CacheKeys.ApplicationSettingsKey, kvps);
            }

            result.SetObject(kvps);

            return result;
        }
    }
}
