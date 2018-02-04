using System.Data;

using Scheduler.Domain.Data.BusinessEntities;
using Scheduler.Domain.Data.Repositories;

namespace Scheduler.Infrastructure.Data.Repositories
{
    public class JobDetailRepository : Repository<JobDetail>, IJobDetailRepository
    {
        public JobDetailRepository(IDbTransaction dbTransaction)
            : base(dbTransaction)
        {

        }
    }
}
