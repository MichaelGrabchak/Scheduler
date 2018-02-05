using System;

using Scheduler.Core.Context;
using Scheduler.Domain.Data.BusinessEntities;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;
using Scheduler.Domain.Data.Repositories;

namespace Scheduler.Infrastructure.Data.EntityFramework.Repositories
{
    public class SchedulerInstanceRepository : BaseRepository<SchedulerInstance, Guid>, ISchedulerInstanceRepository
    {
        public SchedulerInstanceRepository(IDbContextProvider dbContextProvider, ISchedulerContext schedulerContext)
            : base(dbContextProvider, schedulerContext)
        {

        }

        //public SchedulerInstanceDetails GetInstanceDetails(string instanceId)
        //{
        //    //return Connection.Query<SchedulerInstanceDetails>(
        //    //    "usp_GETSCHEDULERINSTANCEDETAILS", 
        //    //    param: new { Id = instanceId },
        //    //    commandType: CommandType.StoredProcedure,
        //    //    transaction: Transaction
        //    //).SingleOrDefault();
        //}
    }
}
