namespace Scheduler.Core.Engine
{
    public abstract class SchedulerSettings
    {
        public string JobsDirectory { get; set; } = Constants.Scheduler.DefaultJobsPath;

        public bool StartEngineImmediately { get; set; } = true;
        public bool EnableJobsDirectoryTracking { get; set; } = true;

        public EngineOperationEventHandler EngineStarted;
        public EngineOperationEventHandler EnginePaused;
        public EngineOperationEventHandler EngineTerminated;

        public SchedulerEventHandler JobsDiscovered;

        public JobOperationEventHandler JobScheduled;
        public JobOperationEventHandler JobUnscheduled;
        public JobOperationEventHandler JobPaused;
        public JobOperationEventHandler JobResumed;
        public JobOperationEventHandler JobTriggered;
    }
}
