using System;
using System.IO;
using System.Reflection;

namespace Scheduler.Engine
{
    public abstract class SchedulerSettings
    {
        public string InstanceId { get; set; }
        public string InstanceName { get; set; }

        public string JobsDirectory { get; set; }

        public bool StartEngineImmediately { get; set; }
        public bool EnableJobsDirectoryTracking { get; set; }

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

        protected SchedulerSettings()
        {
            JobsDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

            StartEngineImmediately = true;
            EnableJobsDirectoryTracking = true;
        }
    }
}
