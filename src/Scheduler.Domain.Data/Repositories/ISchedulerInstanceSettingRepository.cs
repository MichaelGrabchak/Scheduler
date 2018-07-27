using System.Collections.Generic;

using Scheduler.Domain.Data.BusinessEntities;

namespace Scheduler.Domain.Data.Repositories
{
    public interface ISchedulerInstanceSettingRepository : IRepository<SchedulerInstanceSetting, int>
    {
        IList<SchedulerInstanceSetting> GetInstanceSettings();
    }
}
