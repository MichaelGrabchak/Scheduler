using System.Data;

using Scheduler.Domain.Data.BusinessEntities;
using Scheduler.Domain.Data.Repositories;

namespace Scheduler.Infrastructure.Data.Repositories
{
    public class SchedulerInstanceSettingRepository : Repository<SchedulerInstanceSetting>, ISchedulerInstanceSettingRepository
    {
        public SchedulerInstanceSettingRepository(IDbTransaction dbTransaction)
            : base(dbTransaction)
        {

        }
    }
}
