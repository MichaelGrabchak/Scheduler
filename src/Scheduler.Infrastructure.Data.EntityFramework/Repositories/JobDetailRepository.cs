﻿using System.Linq;

using Scheduler.Core.Context;
using Scheduler.Domain.Data.BusinessEntities;
using Scheduler.Domain.Data.EntityFramework.ContextProviders;
using Scheduler.Domain.Data.Repositories;

namespace Scheduler.Infrastructure.Data.EntityFramework.Repositories
{
    public class JobDetailRepository : BaseRepository<JobDetail, int>, IJobDetailRepository
    {
        public JobDetailRepository(IDbContextProvider dbContextProvider, IApplicationContext schedulerContext)
            :base(dbContextProvider, schedulerContext)
        {

        }

        public JobDetail GetJobDetail(string name, string group)
        {
            return DbSet
                .FirstOrDefault(
                    entity => 
                        entity.InstanceId == SchedulerContext.InstanceId && 
                        entity.JobName == name && 
                        entity.JobGroup == group);
        }

        protected override void AddEntity(JobDetail entity)
        {
            entity.InstanceId = SchedulerContext.InstanceId;
            base.AddEntity(entity);
        }
    }
}
