namespace Scheduler.Core.Context
{
    public interface ISchedulerContext
    {
        string InstanceId { get; }

        string ConnectionString { get; }
    }
}
