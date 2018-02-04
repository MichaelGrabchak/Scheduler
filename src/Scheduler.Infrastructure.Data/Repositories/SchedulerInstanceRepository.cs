using System.Data;
using System.Linq;

using Dapper;

using Scheduler.Domain.Data.BusinessEntities;
using Scheduler.Domain.Data.Repositories;

namespace Scheduler.Infrastructure.Data.Repositories
{
    public class SchedulerInstanceRepository : Repository<SchedulerInstance>, ISchedulerInstanceRepository
    {
        public SchedulerInstanceRepository(IDbTransaction dbTransaction)
            : base(dbTransaction)
        {

        }

        public SchedulerInstanceDetails GetInstanceDetails(string instanceId)
        {
            return Connection.Query<SchedulerInstanceDetails>(
                "usp_GETSCHEDULERINSTANCEDETAILS", 
                param: new { Id = instanceId },
                commandType: CommandType.StoredProcedure,
                transaction: Transaction
            ).SingleOrDefault();
        }
    }
}
