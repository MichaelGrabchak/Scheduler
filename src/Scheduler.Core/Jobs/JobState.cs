namespace Scheduler.Core.Jobs
{
    public enum JobState
    {
        None,

        Normal,
        Paused,

        Skipped,
        ToBeExecuted,
        Executing,

        Failed,
        Succeeded
    }
}
