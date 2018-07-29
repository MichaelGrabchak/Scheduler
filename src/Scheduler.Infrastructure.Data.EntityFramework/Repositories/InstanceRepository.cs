using System;

using Scheduler.Core.Context;
using Scheduler.Domain.Data.BusinessEntities;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;
using Scheduler.Domain.Data.Repositories;

namespace Scheduler.Infrastructure.Data.EntityFramework.Repositories
{
    public class InstanceRepository : BaseRepository<Instance, Guid>, IInstanceRepository
    {
        public InstanceRepository(IDbContextProvider dbContextProvider, IApplicationContext schedulerContext)
            : base(dbContextProvider, schedulerContext)
        {

        }
    }
}
