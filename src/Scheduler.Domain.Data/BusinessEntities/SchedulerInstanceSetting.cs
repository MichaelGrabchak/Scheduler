using System;

namespace Scheduler.Domain.Data.BusinessEntities
{
    public class SchedulerInstanceSetting
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Guid InstanceId { get; set; }
        public virtual SchedulerInstance Instance { get; set; }
    }
}
