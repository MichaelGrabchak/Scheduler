namespace Scheduler.Core.Jobs
{
    public enum JobActionState
    {
        None,

        Skipped,
        ToBeExecuted,
        Executing,

        Failed,
        Succeeded
    }

    public enum JobState
    {
        Normal,
        Paused
    }
}
