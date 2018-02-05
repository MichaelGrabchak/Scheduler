using System.Linq;

using Scheduler.Core.Context;
using Scheduler.Domain.Data.BusinessEntities;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;
using Scheduler.Domain.Data.Repositories;

namespace Scheduler.Infrastructure.Data.EntityFramework.Repositories
{
    public class JobDetailRepository : BaseRepository<JobDetail, int>, IJobDetailRepository
    {
        public JobDetailRepository(IDbContextProvider dbContextProvider, ISchedulerContext schedulerContext)
            :base(dbContextProvider, schedulerContext)
        {

        }

        public JobDetail GetJobDetail(string name, string group)
        {
            return _dbSet.FirstOrDefault(
                entity => 
                    entity.InstanceId == _schedulerContext.InstanceId && 
                    entity.JobName == name && 
                    entity.JobGroup == group);
        }

        protected override void AddEntity(JobDetail entity)
        {
            entity.InstanceId = _schedulerContext.InstanceId;
            base.AddEntity(entity);
        }

        //public JobDetail GetJobDetail(string name, string group, string instanceId)
        //{
        //    //return Connection.Query<JobDetail>(
        //    //    sql: $"SELECT * FROM dbo.JOBDETAIL WHERE INSTANCEID = @InstanceId AND JobName = @JobName AND JobGroup = @JobGroup",
        //    //    param: new { InstanceId = instanceId, JobName = name, JobGroup = group },
        //    //    transaction: Transaction
        //    //).FirstOrDefault();
        //}
    }
}
