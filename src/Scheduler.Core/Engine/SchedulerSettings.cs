using System.Configuration;

namespace Scheduler.Core.Engine
{
    public abstract class SchedulerSettings
    {
        public string InstanceId { get; set; } = ConfigurationManager.AppSettings.Get("SchedulerInstanceId");
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

        public JobOperationEventHandler BeforeJobExecution;
        public JobOperationEventHandler AfterJobExecution;
        public JobOperationEventHandler JobExecutionSucceeded;
        public JobOperationEventHandler JobExecutionFailed;
        public JobOperationEventHandler JobExecutionSkipped;
    }
}
