using System;

using Scheduler.Domain.Data.BusinessEntities;

namespace Scheduler.Domain.Data.Repositories
{
    public interface ISchedulerInstanceSettingRepository : IRepository<SchedulerInstanceSetting, int>
    {
        SchedulerInstanceSetting GetInstanceSettings();
    }
}
