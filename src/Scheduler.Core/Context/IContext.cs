using System;

namespace Scheduler.Core.Context
{
    public interface IContext
    {
        Guid InstanceId { get; }

        string ConnectionString { get; }
    }
}
