using Scheduler.Domain.Data.BusinessEntities;
using Scheduler.Domain.Data.Repositories;
using System;

namespace Scheduler.Domain.Data
{
    public interface IDbContext : IDisposable
    {
        IRepository<JobDetail> JobDetails { get; }
        IRepository<SchedulerInstance> SchedulerInstances { get; }
        IRepository<SchedulerInstanceSetting> SchedulerInstanceSettings { get; }

        void Commit();
    }
}
