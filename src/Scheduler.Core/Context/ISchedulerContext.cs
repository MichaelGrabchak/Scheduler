using System;

namespace Scheduler.Core.Context
{
    public interface ISchedulerContext
    {
        Guid InstanceId { get; }

        string ConnectionString { get; }
    }
}
