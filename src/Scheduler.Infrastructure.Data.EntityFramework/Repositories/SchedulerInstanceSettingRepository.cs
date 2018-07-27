using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Scheduler.Core.Context;
using Scheduler.Domain.Data.BusinessEntities;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;
using Scheduler.Domain.Data.Repositories;

namespace Scheduler.Infrastructure.Data.EntityFramework.Repositories
{
    public class SchedulerInstanceSettingRepository : BaseRepository<SchedulerInstanceSetting, int>, ISchedulerInstanceSettingRepository
    {
        public SchedulerInstanceSettingRepository(IDbContextProvider dbContextProvider, IApplicationContext schedulerContext)
            : base(dbContextProvider, schedulerContext)
        {

        }

        public IList<SchedulerInstanceSetting> GetInstanceSettings()
        {
            return _dbSet
                   .Include(x => x.Instance)
                   .Where(x => x.InstanceId == _schedulerContext.InstanceId)
                   .ToList();
        }
    }
}
