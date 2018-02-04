using System;

namespace Scheduler.Domain.Data.BusinessEntities
{
    public class SchedulerInstanceDetails
    {
        public Guid Id { get; set; }
        public string InstanceName { get; set; }
        public bool IsImmediateEngineStartEnabled { get; set; }
        public bool IsJobsDirectoryTrackingEnabled { get; set; }
    }
}
