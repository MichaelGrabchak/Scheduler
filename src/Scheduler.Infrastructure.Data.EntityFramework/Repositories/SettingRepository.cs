using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using Scheduler.Core.Context;
using Scheduler.Domain.Data.BusinessEntities;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;
using Scheduler.Domain.Data.Repositories;

namespace Scheduler.Infrastructure.Data.EntityFramework.Repositories
{
    public class SettingRepository : BaseRepository<Setting, int>, ISettingRepository
    {
        public SettingRepository(IDbContextProvider dbContextProvider, IApplicationContext schedulerContext)
            : base(dbContextProvider, schedulerContext)
        {

        }

        public IList<Setting> GetInstanceSettings()
        {
            return DbSet
                   .Include(x => x.Instance)
                   .Where(x => x.InstanceId == SchedulerContext.InstanceId)
                   .ToList();
        }
    }
}
