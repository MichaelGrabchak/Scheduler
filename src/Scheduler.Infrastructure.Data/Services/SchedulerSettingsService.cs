using System.Collections.Generic;
using System.Linq;

using Scheduler.Core.Extensions;
using Scheduler.Domain.Data.Repositories;
using Scheduler.Domain.Data.Services;

namespace Scheduler.Infrastructure.Data.Services
{
    public class SchedulerSettingsService : ISchedulerSettingsService
    {
        private readonly ISchedulerInstanceSettingRepository _schedulerInstanceSettingRepository;

        public SchedulerSettingsService(ISchedulerInstanceSettingRepository schedulerInstanceSettingRepository)
        {
            _schedulerInstanceSettingRepository = schedulerInstanceSettingRepository;
        }

        // TODO: Implement storing of the data into data warehouse
        public void SetSettings<T>(T settings)
        {
            
        }

        public T GetSettings<T>() where T : new() 
        {
            var result = new T();

            var kvps = new List<KeyValuePair<string, string>>();

            if (!kvps.Any())
            {
                kvps = _schedulerInstanceSettingRepository
                       .GetInstanceSettings()
                       .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                       .ToList();
            }

            result.SetObject(kvps);

            return result;
        }
    }
}
