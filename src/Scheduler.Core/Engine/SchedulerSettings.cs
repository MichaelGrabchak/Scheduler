﻿using Scheduler.Core.Context;

namespace Scheduler.Core.Engine
{
    public abstract class SchedulerSettings
    {
        public string InstanceId { get; set; }

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

        public SchedulerSettings(ISchedulerContext schedulerContext)
        {
            InstanceId = schedulerContext.InstanceId;

            JobsDirectory = Constants.Scheduler.DefaultJobsPath;

            StartEngineImmediately = true;
            EnableJobsDirectoryTracking = true;
        }
    }
}
