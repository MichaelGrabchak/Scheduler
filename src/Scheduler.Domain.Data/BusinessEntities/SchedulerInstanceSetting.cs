using Scheduler.Domain.Data.Attributes;
using System;

namespace Scheduler.Domain.Data.BusinessEntities
{
    public class SchedulerInstanceSetting
    {
        [Key]
        [Column("INSTANCE_ID")]
        public Guid InstanceId { get; set; }
        public bool IsImmediateEngineStartEnabled { get; set; }
        public bool IsJobsDirectoryTrackingEnabled { get; set; }
    }
}
