using System;

using Scheduler.Domain.Data.BusinessEntities;

namespace Scheduler.Domain.Data.Repositories
{
    public interface ISchedulerInstanceRepository : IRepository<SchedulerInstance, Guid>
    {

    }
}
