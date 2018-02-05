using Scheduler.Domain.Data.Dto;
using Scheduler.Domain.Data.Repositories;
using Scheduler.Domain.Data.Services;

namespace Scheduler.Infrastructure.Data.Services
{
    public class SchedulerInstanceService : ISchedulerInstanceService
    {
        private readonly ISchedulerInstanceSettingRepository _schedulerInstanceSettingRepository;

        public SchedulerInstanceService(ISchedulerInstanceSettingRepository schedulerInstanceSettingRepository)
        {
            _schedulerInstanceSettingRepository = schedulerInstanceSettingRepository;
        }

        public InstanceSettings GetSettings()
        {
            var schedulerInstance = _schedulerInstanceSettingRepository.GetInstanceSettings();

            if(schedulerInstance != null)
            {
                return new InstanceSettings
                {
                    InstanceId = schedulerInstance.Instance.Id.ToString(),
                    InstanceName = schedulerInstance.Instance.InstanceName,
                    IsImmediateEngineStartEnabled = schedulerInstance.IsImmediateEngineStartEnabled,
                    IsJobsDirectoryTrackingEnabled = schedulerInstance.IsJobsDirectoryTrackingEnabled
                };
            }

            return new InstanceSettings
            {
                InstanceName = "<unregistered>"
            };
        }
    }
}
