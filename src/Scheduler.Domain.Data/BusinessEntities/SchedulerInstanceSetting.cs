using System;

namespace Scheduler.Domain.Data.BusinessEntities
{
    public class SchedulerInstanceSetting
    {
        public int Id { get; set; }
        public Guid InstanceId { get; set; }
        public bool IsImmediateEngineStartEnabled { get; set; }
        public bool IsJobsDirectoryTrackingEnabled { get; set; }
        public string JobsDirectory { get; set; }

        public virtual SchedulerInstance Instance { get; set; }
    }
}
