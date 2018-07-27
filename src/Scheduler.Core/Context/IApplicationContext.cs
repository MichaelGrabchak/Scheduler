using System;

namespace Scheduler.Core.Context
{
    public interface IApplicationContext
    {
        Guid InstanceId { get; }

        string ConnectionString { get; }
    }
}
