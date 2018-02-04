using System;

using Scheduler.Domain.Data.Repositories;

namespace Scheduler.Domain.Data
{
    public interface IDbContext : IDisposable
    {
        IJobDetailRepository JobDetails { get; }
        ISchedulerInstanceRepository SchedulerInstances { get; }
        ISchedulerInstanceSettingRepository SchedulerInstanceSettings { get; }

        void Commit();
    }
}
